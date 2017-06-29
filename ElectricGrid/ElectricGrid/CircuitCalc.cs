using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricGrid
{
    class CircuitCalc
    {
        public float[,] solveCircuit(byte[,] circuit)
        {
            byte circuitSize = (byte)circuit.GetLength(0);

            if (!isClosed(circuit, new byte[2] { 0, (byte)((circuitSize - 1) / 2) }, circuitSize, ""))
            {
                return null;
            }
            return null;
        }

        private bool isClosed(byte[,] circuit, byte[] currentWire, byte circuitSize, string dir)//dir to prevent backtracing
        {
            bool closed = false;

            if (currentWire[0] ==(byte)(circuitSize - 1) && currentWire[1]== (byte)((circuitSize - 1) / 2))
            {
                return true;//if reached endpoint, return true
            }
            else
            {
                if (currentWire[1] > 0 && circuit[currentWire[0], (byte)(currentWire[1] - 1)] != 0 && dir!="down")//check recursively in each direction for circuit end
                {
                    closed = closed || isClosed(circuit, new byte[2] { currentWire[0], (byte)(currentWire[1] - 1) }, circuitSize,"up");
                }
                if (currentWire[0] < circuitSize - 1 && circuit[(byte)(currentWire[0] + 1), currentWire[1]] != 0)
                {
                    closed = closed || isClosed(circuit, new byte[2] { (byte)(currentWire[0] + 1), currentWire[1] }, circuitSize,"");
                }
                if (currentWire[1] < circuitSize - 1 && circuit[currentWire[0], (byte)(currentWire[1] + 1)] != 0 && dir!="up")
                {
                    closed = closed || isClosed(circuit, new byte[2] { currentWire[0], (byte)(currentWire[1] + 1) },circuitSize,"down");
                }
            }

            return closed;
        }
    }
}