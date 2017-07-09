using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectricGrid
{
    class CircuitCalc
    {
        private bool Debug = true;
        public bool debug
        {
            get
            {
                return Debug;
            }
            set
            {
                Debug = value;
            }
        }
        private enum wire
        {
            empty = 0,
            node = 4,
            vertical = 2,
            horizontal = 3,
            resistor1 = 1,
            resistor5 = 5,
            resistor10 = 10
        }

        private Random rand = new Random();

        /// <summary>
        /// Prints circuit. Debug function.
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        private string printCircuit(CircuitList circuit, string branch)
        {
            string retString = "";
            bool allNull = true;

            if (circuit.firstWire != null)
            {
                retString += printCircuit(circuit.firstWire, branch + " 1");
                allNull = false;
            }
            if (circuit.secondWire != null)
            {
                if (allNull)
                {
                    retString += printCircuit(circuit.secondWire, branch + " 2");
                    allNull = false;
                }
                else
                {
                    retString += printCircuit(circuit.secondWire, " 2");
                }

            }
            if (circuit.thirdWire != null)
            {
                if (allNull)
                {
                    retString += printCircuit(circuit.thirdWire, branch + " 3");
                    allNull = false;
                }
                else
                {
                    retString += printCircuit(circuit.thirdWire, " 3");
                }
            }

            if (allNull)
            {
                return branch + " █";
            }

            return retString;
        }

        /// <summary>
        /// Finds the circuit's voltage at every point.
        /// </summary>
        /// <param name="circuit"></param>
        /// <returns></returns>
        public float[,] solveCircuit(byte[,] circuitArr, byte inputVoltage)
        {
            byte circuitSize = (byte)circuitArr.GetLength(0);
            byte[] startWire = new byte[2] { 0, (byte)((circuitSize - 1) / 2) };

            if (!isClosed(circuitArr, startWire, circuitSize, ""))
            {
                return null;
            }

            CircuitList circuit = new CircuitList();
            circuit.voltage = inputVoltage;
            circuit.coords = new byte[2] { 0, (byte)((circuitSize - 1) / 2) };
            createCircuit(circuitArr, circuit, (byte[])(startWire.Clone()), "");

            while (fixCircuit(circuit, circuitSize)[1] != "end")
            {
                fixCircuit(circuit, circuitSize);
            }

            if (debug)
            {
                MessageBox.Show(printCircuit(circuit, "start 2"));
            }

            return null;
        }


        private object[] fixCircuit(CircuitList circuit, byte circuitSize)
        {
            object[] retObj = new object[2] { circuit, "not end" };
            //check if reached end of circuit
            if (circuit.connectedWires()==0)
            {
                return new object[2] { circuit, "end" };
            }

            if (circuit.connectedWires() == 1)
            {
                return new object[2]{circuit ,fixCircuit(circuit.MainWire, circuitSize)[1]};
            }
            else if (circuit.connectedWires() == 2)
            {
                CircuitList[] pointerCircuit = new CircuitList[2];
                if (circuit.firstWire != null)
                {
                    if (circuit.secondWire != null)
                    {
                        pointerCircuit[0] = circuit.firstWire;
                        pointerCircuit[1] = circuit.secondWire;                        
                    }
                    else
                    {
                        pointerCircuit[0] = circuit.firstWire;
                        pointerCircuit[1] = circuit.thirdWire;
                    }
                }
                else
                {
                    pointerCircuit[0] = circuit.secondWire;
                    pointerCircuit[1] = circuit.thirdWire; 
                }

                retObj = wireToConverge(pointerCircuit, circuitSize);
                convergeWire(circuit, (CircuitList)retObj[0], (byte[])retObj[2]);

            }
            else
            {
                CircuitList[] pointerCircuit = new CircuitList[2];
                pointerCircuit[0] = circuit.firstWire;
                pointerCircuit[1] = circuit.secondWire;

                retObj = wireToConverge(pointerCircuit, circuitSize);
                convergeWire(circuit, (CircuitList)retObj[0], (byte[])retObj[2]);

                pointerCircuit[0] = circuit.firstWire;
                pointerCircuit[1] = circuit.thirdWire;

                retObj = wireToConverge(pointerCircuit, circuitSize);
                convergeWire(circuit, (CircuitList)retObj[0], (byte[])retObj[2]);

                pointerCircuit[0] = circuit.secondWire;
                pointerCircuit[1] = circuit.thirdWire;

                retObj = wireToConverge(pointerCircuit, circuitSize);
                convergeWire(circuit, (CircuitList)retObj[0], (byte[])retObj[2]);
            }

            return new object[2] { circuit, retObj[1] };
        }


        private CircuitList convergeWire(CircuitList circuit, CircuitList convergenceBranch, byte[] convergeCoords)
        {
            if (circuit.coords.SequenceEqual(convergeCoords))
            {
                circuit = convergenceBranch;
                return circuit;
            }

            if (circuit.firstWire != null)
            {
                circuit.firstWire = convergeWire(circuit.firstWire, convergenceBranch, convergeCoords);
            }
            if (circuit.secondWire != null)
            {
                circuit.secondWire = convergeWire(circuit.secondWire, convergenceBranch, convergeCoords);
            }
            if (circuit.thirdWire != null)
            {
                circuit.thirdWire = convergeWire(circuit.thirdWire, convergenceBranch, convergeCoords);
            }

            return circuit;
        }

        /// <summary>
        /// Returns which branch needs to be converged and its coordinates.
        /// </summary>
        /// <param name="pointerCircuit"></param>
        /// <param name="circuitSize"></param>
        /// <returns></returns>
        private object[] wireToConverge(CircuitList[] pointerCircuit, byte circuitSize)
        {
            object[] retObj;
            byte noChange = 0;

            while (true)
            {
                if (pointerCircuit[0].MainWire != null && pointerCircuit[0].MainWire.connectedWires() > 1)
                {
                    noChange = 0;
                    pointerCircuit[0].MainWire = (CircuitList)(fixCircuit(pointerCircuit[0].MainWire, circuitSize)[0]);
                }
                else if (pointerCircuit[0].MainWire != null && pointerCircuit[1].coords[0] >= pointerCircuit[0].coords[0])
                {
                    noChange = 0;
                    pointerCircuit[0] = pointerCircuit[0].MainWire;

                    if (pointerCircuit[0].coords.SequenceEqual(pointerCircuit[1].coords))
                    {//if replica found, converge
                        pointerCircuit[1] = pointerCircuit[0];
                        retObj = fixCircuit(pointerCircuit[0], circuitSize);
                        retObj = new object[3] { retObj[0], retObj[1], pointerCircuit[0].coords };
                        break;
                    }
                }
                else
                {
                    noChange++;//to remove possibility of infinite loops
                }

                if (pointerCircuit[1].MainWire != null && pointerCircuit[1].MainWire.connectedWires() > 1)
                {
                    noChange = 0;
                    pointerCircuit[1].MainWire = (CircuitList)(fixCircuit(pointerCircuit[1].MainWire, circuitSize)[0]);
                }
                else if (pointerCircuit[1].MainWire != null && (pointerCircuit[0].coords[0] > pointerCircuit[1].coords[0] || noChange>50))
                {
                    noChange = 0;

                    pointerCircuit[1] = pointerCircuit[1].MainWire;

                    if (pointerCircuit[0].coords.SequenceEqual(pointerCircuit[1].coords))
                    {//if replica found, converge
                        pointerCircuit[1] = pointerCircuit[0];
                        retObj = fixCircuit(pointerCircuit[0], circuitSize);
                        retObj = new object[3] { retObj[0], retObj[1], pointerCircuit[0].coords };
                        break;
                    }
                }

            }

            return retObj;
        }


        private CircuitList createCircuit(byte[,] circuitArr, CircuitList circuit, byte[] currentWire, string dir)
        {
            bool foundComponent = false;
            bool[] nodeConnections = new bool[3];
            byte circuitSize = (byte)circuitArr.GetLength(0);
            if (debug)
            {
                circuit.voltage = rand.Next(999);
            }

            while (!foundComponent)
            {
                if (currentWire[0] == (byte)(circuitSize - 1) && currentWire[1] == (byte)(circuitSize - 1) / 2)
                {
                    break;
                }
                switch (circuitArr[currentWire[0], currentWire[1]])
                {//continue along wire until node/resistor
                    case (byte)wire.vertical://vertical wire
                        if (dir == "up")
                        {
                            currentWire[1]++;
                        }
                        else
                        {
                            currentWire[1]--;
                        }
                        break;

                    case (byte)wire.horizontal://horizontal wire
                        currentWire[0]++;
                        break;

                    case (byte)wire.node://node
                        foundComponent = true;

                        if (dir != "down" && currentWire[1] < circuitSize - 1 //check if continuation of circuit exists in all directions
                            && circuitArr[currentWire[0], currentWire[1] + 1] != (byte)wire.empty
                            && connectsToEnd(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, "up"))
                        {//find and build next circuit branch                            
                            circuit.thirdWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, "up");
                            circuit.thirdWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                        }

                        if (circuitArr[(byte)(currentWire[0] + 1), currentWire[1]] != (byte)wire.empty)
                        {                            
                            circuit.secondWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                            circuit.secondWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                        }

                        if (dir != "up" && currentWire[1] > 0
                            && circuitArr[currentWire[0], currentWire[1] - 1] != (byte)wire.empty
                            && connectsToEnd(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down"))
                        {                            
                            circuit.firstWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down");
                            circuit.firstWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                        }
                        break;
                    case (byte)wire.resistor1://1 Ohm resistor
                        foundComponent = true;
                        circuit.secondWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                        circuit.secondWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                        circuit.secondWire.resistance = 1;
                        break;
                    case (byte)wire.resistor5://5 Ohm resistor
                        circuit.secondWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                        circuit.secondWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                        circuit.secondWire.resistance = 5;
                        break;
                    case (byte)wire.resistor10://10 Ohm resistor
                        circuit.secondWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                        circuit.secondWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                        circuit.secondWire.resistance = 10;
                        break;
                }
            }            

            return circuit;
        }
        
        /// <summary>
        /// Checks if wire is connected to end of circuit.
        /// </summary>
        /// <param name="circuitArr"></param>
        /// <param name="currentWire"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private bool connectsToEnd(byte[,] circuitArr, byte[] currentWire, string dir)
        {
            bool endReached = false;
            byte circuitSize = (byte)circuitArr.GetLength(0);

            if (currentWire[0] == circuitSize-1 && currentWire[1] == (byte)(circuitSize - 1) / 2)//check if reached end of circuit
            {
                return true;
            }

            if (currentWire[1] > 0 && circuitArr[currentWire[0], currentWire[1] - 1] != (byte)wire.empty && dir != "up")//traces circuit to see if it is connected to end
            {
                endReached = endReached || connectsToEnd(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down");
            }
            if (currentWire[0] < circuitSize-1 && circuitArr[currentWire[0] + 1, currentWire[1]] != (byte)wire.empty)
            {
                endReached = endReached || connectsToEnd(circuitArr, new byte[2] { (byte)(currentWire[0] + 1), (byte)(currentWire[1]) }, "");
            }
            if (currentWire[1] < circuitSize - 1 && circuitArr[currentWire[0], currentWire[1] + 1] != (byte)wire.empty && dir != "down")
            {
                endReached = endReached || connectsToEnd(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, "up");
            }

            return endReached;
        }

        /// <summary>
        /// Recursively checks if the circuit is closed.
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="currentWire"></param>
        /// <param name="circuitSize"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private bool isClosed(byte[,] circuit, byte[] currentWire, byte circuitSize, string dir)//dir to prevent backtracing
        {
            bool closed = false;

            if (currentWire[0] == (byte)(circuitSize - 1) && currentWire[1] == (byte)((circuitSize - 1) / 2))
            {
                return true;//if reached endpoint, return true
            }
            else
            {
                if (currentWire[1] > 0 && circuit[currentWire[0], (byte)(currentWire[1] - 1)] != (byte)wire.empty && dir != "down")//check recursively in each direction for circuit end
                {
                    closed = closed || isClosed(circuit, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, circuitSize, "up");
                }
                if (currentWire[0] < circuitSize - 1 && circuit[(byte)(currentWire[0] + 1), currentWire[1]] != (byte)wire.empty)
                {
                    closed = closed || isClosed(circuit, new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, circuitSize, "");
                }
                if (currentWire[1] < circuitSize - 1 && circuit[currentWire[0], (byte)(currentWire[1] + 1)] != (byte)wire.empty && dir != "up")
                {
                    closed = closed || isClosed(circuit, new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, circuitSize, "down");
                }
            }

            return closed;
        }
    }
}