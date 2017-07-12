﻿using System;
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
        private byte circuitSize;


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
        public float[][,] solveCircuit(byte[,] circuitArr, byte inputVoltage)
        {
            circuitSize = (byte)circuitArr.GetLength(0);
            byte[] startWire = new byte[2] { 0, (byte)((circuitSize - 1) / 2) };

            if (!isClosed(circuitArr, startWire, ""))
            {
                return null;
            }

            CircuitList circuit = new CircuitList();
            circuit.voltage = inputVoltage;
            circuit.coords = new byte[2] { 0, (byte)((circuitSize - 1) / 2) };
            createCircuit(circuitArr, circuit, (byte[])(startWire.Clone()), "");

            while (fixCircuit(circuit)[1] != "end")
            {
                fixCircuit(circuit);
            }

            circuit = addCircuitEnd(circuit);

            float[][,] retCircuits = calculateCircuitVals(circuit, inputVoltage,"voltage");

            printArray(retCircuits[0], "Amperage:");
            printArray(retCircuits[1], "Voltage:");

            return null;
        }

        private void printArray(float[,] arr, string title)
        {
            if (debug)
            {
                string message = title + Environment.NewLine;

                for (byte i = 0; i < arr.GetLength(0); i++)
                {
                    for (byte j = 0; j < arr.GetLength(1); j++)
                    {
                        message += (Math.Round(arr[j, i], 3)).ToString();

                        for (byte k = (byte)((Math.Round(arr[j, i], 3)).ToString().Length); k < 6; k++)
                        {
                            message += " ";
                        }
                    }
                    message += Environment.NewLine;
                }

                MessageBox.Show(message);
            }
        }


        private float[][,] calculateCircuitVals(CircuitList circuit, float inputParameter, string type)
        {
            float[,] amperageCircuit = new float[circuitSize, circuitSize];
            float[,] voltageCircuit = new float[circuitSize, circuitSize];
            byte[] endCoords = findEndOfCircuit(circuit);

            if (!circuit.coords.SequenceEqual(endCoords))
            {
                if (type == "voltage")
                {
                    if (float.IsNaN(circuit.amperage))
                    {
                        circuit.amperage = getAmperage(circuit, inputParameter);
                    }
                    else
                    {
                        circuit.amperage += getAmperage(circuit, inputParameter);
                    }

                    amperageCircuit[circuit.coords[0], circuit.coords[1]] = circuit.amperage;
                    circuit.voltage = circuit.amperage * getResistance(circuit, endCoords);
                    voltageCircuit[circuit.coords[0], circuit.coords[1]] = circuit.voltage;
                }
                else if (type == "amperage")
                {
                    circuit.amperage = inputParameter;
                    amperageCircuit[circuit.coords[0], circuit.coords[1]] = circuit.amperage;
                    circuit.voltage = circuit.amperage * getResistance(circuit, endCoords);
                    voltageCircuit[circuit.coords[0], circuit.coords[1]] = circuit.voltage;
                }
                
                switch (circuit.connectedWires())
                {
                    case 1:
                        amperageCircuit = unifyArrays(amperageCircuit, calculateCircuitVals(circuit.MainWire, circuit.amperage, "amperage")[0]);
                        voltageCircuit = unifyArrays(voltageCircuit, calculateCircuitVals(circuit.MainWire, circuit.amperage, "amperage")[1]);
                        break;

                    case 2:
                        amperageCircuit = unifyArrays(amperageCircuit, calculateCircuitVals(circuit.activeWires()[0], circuit.voltage, "voltage")[0]);
                        voltageCircuit = unifyArrays(voltageCircuit, calculateCircuitVals(circuit.activeWires()[0], circuit.voltage, "voltage")[1]);

                        amperageCircuit = unifyArrays(amperageCircuit, calculateCircuitVals(circuit.activeWires()[1], circuit.voltage, "voltage")[0]);
                        voltageCircuit = unifyArrays(voltageCircuit, calculateCircuitVals(circuit.activeWires()[1], circuit.voltage, "voltage")[1]);
                        break;

                    case 3:
                        amperageCircuit = unifyArrays(amperageCircuit, calculateCircuitVals(circuit.firstWire, circuit.voltage, "voltage")[0]);
                        voltageCircuit = unifyArrays(voltageCircuit, calculateCircuitVals(circuit.firstWire, circuit.voltage, "voltage")[1]);

                        amperageCircuit = unifyArrays(amperageCircuit, calculateCircuitVals(circuit.secondWire, circuit.voltage, "voltage")[0]);
                        voltageCircuit = unifyArrays(voltageCircuit, calculateCircuitVals(circuit.secondWire, circuit.voltage, "voltage")[1]);

                        amperageCircuit = unifyArrays(amperageCircuit, calculateCircuitVals(circuit.thirdWire, circuit.voltage, "voltage")[0]);
                        voltageCircuit = unifyArrays(voltageCircuit, calculateCircuitVals(circuit.thirdWire, circuit.voltage, "voltage")[1]);
                        break;
                }
            }

            return new float[2][,] { amperageCircuit, voltageCircuit };
        }


        private byte[] findEndOfCircuit(CircuitList circuit)
        {
            while (circuit.connectedWires() != 0)
            {
                circuit = circuit.MainWire;
            }

            return circuit.coords;
        }

        private float[,] unifyArrays(float[,] firstArray, float[,] secondArray)
        {
            float[,] unifiedArrays = (float[,])firstArray.Clone();

            for (byte i = 0; i < firstArray.GetLength(0); i++)
            {
                for (byte j = 0; j < firstArray.GetLength(1); j++)
                {
                    if (secondArray[i, j] != 0 && secondArray[i, j] != firstArray[i, j])
                    {
                        unifiedArrays[i,j] = secondArray[i, j];
                    }
                }
            }

            return unifiedArrays;
        }

        /// <summary>
        /// Fixes diverging branches in the circuit and forces them to converge.
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="circuitSize"></param>
        /// <returns></returns>
        private object[] fixCircuit(CircuitList circuit)
        {
            object[] retObj = new object[2] { circuit, "not end" };
            //check if reached end of circuit
            if (circuit.connectedWires() == 0)
            {
                return new object[2] { circuit, "end" };
            }

            if (circuit.connectedWires() == 1)
            {
                return new object[2] { circuit, fixCircuit(circuit.MainWire)[1] };
            }
            else if (circuit.connectedWires() == 2)
            {
                CircuitList[] pointerCircuit = circuit.activeWires();

                retObj = wireToConverge(pointerCircuit);
                convergeWire(circuit, (CircuitList)retObj[0], (byte[])retObj[2]);

            }
            else
            {
                CircuitList[] pointerCircuit = new CircuitList[2];
                pointerCircuit[0] = circuit.firstWire;
                pointerCircuit[1] = circuit.secondWire;

                retObj = wireToConverge(pointerCircuit);
                convergeWire(circuit, (CircuitList)retObj[0], (byte[])retObj[2]);

                pointerCircuit[0] = circuit.firstWire;
                pointerCircuit[1] = circuit.thirdWire;

                retObj = wireToConverge(pointerCircuit);
                convergeWire(circuit, (CircuitList)retObj[0], (byte[])retObj[2]);

                pointerCircuit[0] = circuit.secondWire;
                pointerCircuit[1] = circuit.thirdWire;

                retObj = wireToConverge(pointerCircuit);
                convergeWire(circuit, (CircuitList)retObj[0], (byte[])retObj[2]);
            }

            return new object[2] { circuit, retObj[1] };
        }

        /// <summary>
        /// Finds and converges branches based on pre-known data.
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="convergenceBranch"></param>
        /// <param name="convergeCoords"></param>
        /// <returns></returns>
        private CircuitList convergeWire(CircuitList circuit, CircuitList convergenceBranch, byte[] convergeCoords)
        {
            if (circuit.coords.SequenceEqual(convergeCoords))
            {
                circuit = convergenceBranch;
                circuit.converges = true;
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
        private object[] wireToConverge(CircuitList[] pointerCircuit)
        {
            object[] retObj;
            byte noChange = 0;

            while (true)
            {
                if (pointerCircuit[0].MainWire != null && pointerCircuit[0].MainWire.connectedWires() > 1)
                {
                    noChange = 0;
                    pointerCircuit[0].MainWire = (CircuitList)(fixCircuit(pointerCircuit[0].MainWire)[0]);
                }
                else if (pointerCircuit[0].MainWire != null && pointerCircuit[1].coords[0] >= pointerCircuit[0].coords[0])
                {
                    noChange = 0;
                    pointerCircuit[0] = pointerCircuit[0].MainWire;

                    if (pointerCircuit[0].coords.SequenceEqual(pointerCircuit[1].coords))
                    {//if replica found, converge
                        pointerCircuit[1] = pointerCircuit[0];
                        retObj = fixCircuit(pointerCircuit[0]);
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
                    pointerCircuit[1].MainWire = (CircuitList)(fixCircuit(pointerCircuit[1].MainWire)[0]);
                }
                else if (pointerCircuit[1].MainWire != null && (pointerCircuit[0].coords[0] > pointerCircuit[1].coords[0] || noChange > 50))
                {
                    noChange = 0;

                    pointerCircuit[1] = pointerCircuit[1].MainWire;

                    if (pointerCircuit[0].coords.SequenceEqual(pointerCircuit[1].coords))
                    {//if replica found, converge
                        pointerCircuit[1] = pointerCircuit[0];
                        retObj = fixCircuit(pointerCircuit[0]);
                        retObj = new object[3] { retObj[0], retObj[1], pointerCircuit[0].coords };
                        break;
                    }
                }

            }

            return retObj;
        }

        /// <summary>
        /// Creates circuit based on 2D enum array.
        /// </summary>
        /// <param name="circuitArr"></param>
        /// <param name="circuit"></param>
        /// <param name="currentWire"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private CircuitList createCircuit(byte[,] circuitArr, CircuitList circuit, byte[] currentWire, string dir)
        {
            bool foundComponent = false;
            bool[] nodeConnections = new bool[3];

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
                            circuit.thirdWire.resistance = 0;
                        }

                        if (circuitArr[(byte)(currentWire[0] + 1), currentWire[1]] != (byte)wire.empty)
                        {
                            circuit.secondWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                            circuit.secondWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                            circuit.secondWire.resistance = 0;
                        }

                        if (dir != "up" && currentWire[1] > 0
                            && circuitArr[currentWire[0], currentWire[1] - 1] != (byte)wire.empty
                            && connectsToEnd(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down"))
                        {
                            circuit.firstWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down");
                            circuit.firstWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                            circuit.firstWire.resistance = 0;
                        }
                        break;

                    case (byte)wire.resistor1://1 Ohm resistor
                        foundComponent = true;
                        circuit.secondWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                        circuit.secondWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                        circuit.secondWire.resistance = 1;
                        break;

                    case (byte)wire.resistor5://5 Ohm resistor
                        foundComponent = true;
                        circuit.secondWire = createCircuit(circuitArr, new CircuitList(), new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                        circuit.secondWire.coords = new byte[2] { currentWire[0], currentWire[1] };
                        circuit.secondWire.resistance = 5;
                        break;

                    case (byte)wire.resistor10://10 Ohm resistor
                        foundComponent = true;
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

            if (currentWire[0] == circuitSize - 1 && currentWire[1] == (byte)(circuitSize - 1) / 2)//check if reached end of circuit
            {
                return true;
            }

            if (currentWire[1] > 0 && circuitArr[currentWire[0], currentWire[1] - 1] != (byte)wire.empty && dir != "up")//traces circuit to see if it is connected to end
            {
                endReached = endReached || connectsToEnd(circuitArr, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "down");
            }
            if (currentWire[0] < circuitSize - 1 && circuitArr[currentWire[0] + 1, currentWire[1]] != (byte)wire.empty)
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
        /// Gets the resistance of a circuit.
        /// </summary>
        /// <param name="circuit"></param>
        /// <returns></returns>
        private float getResistance(CircuitList circuit, byte[] endCoords)
        {
            switch (circuit.connectedWires())
            {
                case 1:
                    return getSequentialResistance(circuit, endCoords);
                case 2:
                    return getParallelResistance(circuit.activeWires()[0], circuit.activeWires()[1]);
                case 3:
                    return getParallelResistance(circuit.activeWires());
            }

            return float.NaN;
        }

        /// <summary>
        /// Returns the wire with the biggest x coordinate.
        /// </summary>
        /// <param name="circuits"></param>
        /// <returns></returns>
        private CircuitList furthestWire(CircuitList[] circuits)
        {
            CircuitList retWire = circuits[0];

            for (byte i = 1; i < circuits.Length; i++)
            {
                if (circuits[i].coords[0] > retWire.coords[0])
                {
                    retWire = circuits[i];
                }
            }

            return retWire;
        }

        /// <summary>
        /// Gets the resistance of a sequential set of resistors.
        /// </summary>
        /// <param name="circuit"></param>
        /// <returns></returns>
        private float getSequentialResistance(CircuitList circuit, byte[] endCoords)
        {
            float resistance;

            if (!float.IsNaN(circuit.resistance))
            {
                resistance = circuit.resistance;
            }
            else
            {
                resistance = 0;
            }
            
            if (!endCoords.SequenceEqual(circuit.coords))
            {
                switch (circuit.connectedWires())
                {
                    case 1:
                        resistance += getSequentialResistance(circuit.MainWire, endCoords);
                        break;
                    case 2:
                        resistance += 1 / getParallelResistance(circuit.activeWires()[0], circuit.activeWires()[1]);
                        circuit = (CircuitList)wireToConverge(new CircuitList[2] { circuit.activeWires()[0], circuit.activeWires()[1] })[0];
                        resistance += getSequentialResistance(circuit, endCoords);
                        break;
                    case 3:
                        resistance += 1 / getParallelResistance(circuit.activeWires());
                        circuit = furthestWire(new CircuitList[3]{(CircuitList)wireToConverge(new CircuitList[2] { circuit.firstWire, circuit.secondWire })[0],
                                                                  (CircuitList)wireToConverge(new CircuitList[2] { circuit.firstWire, circuit.thirdWire })[0],
                                                                  (CircuitList)wireToConverge(new CircuitList[2] { circuit.thirdWire, circuit.secondWire })[0]});
                        resistance += getSequentialResistance(circuit, endCoords);
                        break;
                }
            }

            return resistance;
        }

        /// <summary>
        /// Gets the resistance of a node with a parallel circuit with 2 connected wires.
        /// </summary>
        /// <param name="firstCircuit"></param>
        /// <param name="secondCircuit"></param>
        /// <returns></returns>
        private float getParallelResistance(CircuitList firstCircuit, CircuitList secondCircuit)
        {
            byte[] endCoords = (byte[])wireToConverge(new CircuitList[2] { firstCircuit, secondCircuit })[2];
            float resistance = 0;

            resistance += 1 / getSequentialResistance(firstCircuit, endCoords);
            resistance += 1 / getSequentialResistance(secondCircuit, endCoords);

            return resistance;
        }

        /// <summary>
        /// Gets the resistance of a node with a parallel circuit with 3 connected wires.
        /// </summary>
        /// <param name="firstCircuit"></param>
        /// <param name="secondCircuit"></param>
        /// <param name="thirdCircuit"></param>
        /// <returns></returns>
        private float getParallelResistance(CircuitList[] circuits)
        {
            float resistance = 0;
            byte[] endCoords;

            if((byte)((byte[])wireToConverge(new CircuitList[2] { circuits[0], circuits[1] })[2])[0]<
                (byte)((byte[])wireToConverge(new CircuitList[2] { circuits[1], circuits[2] })[2])[0])//check which wires converge earlier
            {
                endCoords = (byte[])wireToConverge(new CircuitList[2] { circuits[0], circuits[1] })[2];

                resistance += 1 / getSequentialResistance(circuits[2], endCoords);
                resistance += 1/getParallelResistance(circuits[0], circuits[1])
                           + getSequentialResistance((CircuitList)wireToConverge(new CircuitList[2] { circuits[0], circuits[1] })[0], endCoords);                
            }
            else
            {
                endCoords = (byte[])wireToConverge(new CircuitList[2] { circuits[1], circuits[2] })[2];

                resistance += 1 / getSequentialResistance(circuits[0], endCoords);
                resistance += 1 / getParallelResistance(circuits[1], circuits[2])
                           + getSequentialResistance((CircuitList)wireToConverge(new CircuitList[2] { circuits[1], circuits[2] })[0], endCoords);
            }

            return 1 / resistance;
        }

        /// <summary>
        /// Gets a wires amperage.
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="inputVoltage"></param>
        /// <returns></returns>
        private float getAmperage(CircuitList circuit, float inputVoltage)
        {
            float resistance = 0;
            byte[] wireEnd = findEndOfCircuit(circuit);

            switch (circuit.connectedWires())
            {
                case 1:                    
                    resistance = getSequentialResistance(circuit, wireEnd);

                    return inputVoltage / resistance;
                case 0:
                    return 0;
            }

            return float.NaN;
        }

        /// <summary>
        /// Recursively checks if the circuit is closed.
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="currentWire"></param>
        /// <param name="circuitSize"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private bool isClosed(byte[,] circuit, byte[] currentWire, string dir)//dir to prevent backtracing
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
                    closed = closed || isClosed(circuit, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, "up");
                }
                if (currentWire[0] < circuitSize - 1 && circuit[(byte)(currentWire[0] + 1), currentWire[1]] != (byte)wire.empty)
                {
                    closed = closed || isClosed(circuit, new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, "");
                }
                if (currentWire[1] < circuitSize - 1 && circuit[currentWire[0], (byte)(currentWire[1] + 1)] != (byte)wire.empty && dir != "up")
                {
                    closed = closed || isClosed(circuit, new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) }, "down");
                }
            }

            return closed;
        }

        private CircuitList addCircuitEnd(CircuitList circuit)
        {
            if (circuit.connectedWires() != 0)
            {
                circuit.MainWire = addCircuitEnd(circuit.MainWire);
            }
            else
            {
                circuit.secondWire = new CircuitList();
                circuit.secondWire.coords = new byte[2] { (byte)(circuitSize - 1), (byte)((circuitSize - 1) / 2) };
            }

            return circuit;
        }
    }
}