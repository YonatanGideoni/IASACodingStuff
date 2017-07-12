using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricGrid
{
    class CircuitList
    {
        public CircuitList firstWire { get; set; }
        public CircuitList secondWire { get; set; }
        public CircuitList thirdWire { get; set; }
        private float Resistance = float.NaN;
        private float Voltage = float.NaN;
        private float Amperage = float.NaN;

        public bool converges { get; set; }
        public byte[] coords { get; set; }
        public float resistance
        {
            get
            {
                return Resistance;
            }
            set
            {
                Resistance = value;
            }
        }
        public float voltage
        {
            get
            {
                return Voltage;
            }
            set
            {
                Voltage = value;
            }
        }
        public float amperage
        {
            get
            {
                return Amperage;
            }
            set
            {
                Amperage = value;
            }
        }

        /// <summary>
        /// Gets a wire and returns active wire. Doesn't work for nodes with multiple wires.
        /// </summary>
        /// <param name="wire"></param>
        /// <returns></returns>
        public CircuitList MainWire
        {
            get
            {
                {
                    if (this.firstWire != null)
                    {
                        return this.firstWire;
                    }
                    else if (this.secondWire != null)
                    {
                        return this.secondWire;
                    }
                    else if (this.thirdWire != null)
                    {
                        return this.thirdWire;
                    }
                    return null;
                }
            }
            set
            {
                {
                    if (this.firstWire != null)
                    {
                        this.firstWire = value;
                    }
                    else if (this.secondWire != null)
                    {
                        this.secondWire = value;
                    }
                    this.thirdWire = value;
                }
            }
        }

        /// <summary>
        /// Returns wires which are not null. Only for nodes with >1 wires.
        /// </summary>
        /// <returns></returns>
        public CircuitList[] activeWires()
        {
            if (this.connectedWires() == 2)
            {
                if (this.firstWire != null)
                {
                    if (this.secondWire != null)
                    {
                        return new CircuitList[2] { this.firstWire, this.secondWire };
                    }
                    else
                    {
                        return new CircuitList[2] { this.firstWire, this.thirdWire };
                    }
                }
                else
                {
                    return new CircuitList[2] { this.secondWire, this.thirdWire };
                }
            }
            else if (this.connectedWires() == 3)
            {
                return new CircuitList[3] { this.firstWire, this.secondWire, this.thirdWire };
            }
            return null;
        }

        /// <summary>
        /// Gets the number of wires connected to a node.
        /// </summary>
        /// <param name="circuit"></param>
        /// <returns></returns>
        public byte connectedWires()
        {
            byte connectedWires = 0;

            if (this.firstWire != null)
            {
                connectedWires++;
            }
            if (this.secondWire != null)
            {
                connectedWires++;
            }
            if (this.thirdWire != null)
            {
                connectedWires++;
            }

            return connectedWires;
        }

        /// <summary>
        /// Returns a wire's amperage, voltage, and resistance. Returns null if one of them is null.
        /// </summary>
        /// <param name="wire"></param>
        /// <returns>{amperage,voltage,resistance}</returns>
        public float[] getWireVals(CircuitList wire)
        {
            if (float.IsNaN(wire.amperage) || float.IsNaN(wire.voltage) || float.IsNaN(wire.resistance))
            {
                return null;
            }

            return new float[3] { amperage, voltage, resistance };
        }
    }
}