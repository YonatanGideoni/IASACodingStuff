using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricGrid
{
    class CircuitCalc
    {
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
            createCircuit(circuitArr, circuit, (byte[])(startWire.Clone()), "");

            return null;
        }


        private CircuitList createCircuit(byte[,] circuitArr, CircuitList circuit, byte[] currentWire, string dir)
        {
            bool foundComponent = false;
            bool[] nodeConnections = new bool[3];
            byte circuitSize = (byte)circuitArr.GetLength(0);

            while (!foundComponent)
            {
                if (currentWire[0] == (byte)(circuitSize - 1) && currentWire[1] == (byte)(circuitSize - 1) / 2)
                {
                    break;
                }
                switch (circuitArr[currentWire[0], currentWire[1]])
                {//continue along wire until node/resistor
                    case 2://vertical wire
                        if (dir == "up")
                        {
                            currentWire[1]++;
                        }
                        else
                        {
                            currentWire[1]--;
                        }
                        break;

                    case 3://horizontal wire
                        currentWire[0]++;
                        break;

                    case 4://node
                        foundComponent = true;

                        if (dir != "down" && currentWire[1] < circuitSize - 1 //check if continuation of circuit exists in all directions
                            && circuitArr[currentWire[0], currentWire[1] + 1] != 0
                            && !isConnected(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, "up"))
                        {//find and build next circuit branch
                            circuit.thirdWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, "up");
                        }

                        if (circuitArr[(byte)(currentWire[0] + 1), currentWire[1]] != 0)
                        {
                            circuit.secondWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                        }

                        if (dir != "up" && currentWire[1] > 0
                            && circuitArr[currentWire[0], currentWire[1] - 1] != 0
                            && !isConnected(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down"))
                        {
                            circuit.firstWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down");
                        }
                        break;
                }
            }

            return circuit;
        }

        /// <summary>
        /// Checks if the current wire has a current from the beginning of the circuit.
        /// </summary>
        /// <param name="circuitArr"></param>
        /// <param name="currentWire"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private bool isConnected(byte[,] circuitArr, byte[] currentWire, string dir)
        {
            byte circuitSize = (byte)circuitArr.GetLength(0);
            bool connected = false;

            if (currentWire[0] == 0 && currentWire[1] == (byte)(circuitSize - 1) / 2)//check if reached beginning of circuit
            {
                return true;
            }

            if (currentWire[1] > 0 && circuitArr[currentWire[0], currentWire[1] - 1] != 0 && dir != "up")//traces circuit to see if it is connected to beginning
            {
                connected = connected || isConnected(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down");
            }
            if (currentWire[0] > 0 && circuitArr[currentWire[0] - 1, currentWire[1]] != 0)
            {
                connected = connected || isConnected(circuitArr, new byte[2] { (byte)(currentWire[0] - 1), (byte)(currentWire[1]) }, "");
            }
            if (currentWire[1] < circuitSize - 1 && circuitArr[currentWire[0], currentWire[1] + 1] != 0 && dir != "down")
            {
                connected = connected || isConnected(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, "up");
            }

            return connected;
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
                if (currentWire[1] > 0 && circuit[currentWire[0], (byte)(currentWire[1] - 1)] != 0 && dir != "down")//check recursively in each direction for circuit end
                {
                    closed = closed || isClosed(circuit, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, circuitSize, "up");
                }
                if (currentWire[0] < circuitSize - 1 && circuit[(byte)(currentWire[0] + 1), currentWire[1]] != 0)
                {
                    closed = closed || isClosed(circuit, new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, circuitSize, "");
                }
                if (currentWire[1] < circuitSize - 1 && circuit[currentWire[0], (byte)(currentWire[1] + 1)] != 0 && dir != "up")
                {
                    closed = closed || isClosed(circuit, new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, circuitSize, "down");
                }
            }

            return closed;
        }
    }
}