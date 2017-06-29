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
        /// Gets a wire and returns active wire. Doesn't work for nodes.
        /// </summary>
        /// <param name="wire"></param>
        /// <returns></returns>
        public CircuitList MainWire(CircuitList wire)
        {
            if (firstWire != null)
            {
                return firstWire;
            }
            else if (secondWire != null)
            {
                return secondWire;
            }
            return thirdWire;
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

        /// <summary>
        /// Finds the wire's amperage or voltage if possible.
        /// </summary>
        /// <param name="wire"></param>
        public void findWireVals(CircuitList wire)
        {
            if (!float.IsNaN(wire.voltage) && !float.IsNaN(wire.resistance))
            {
                wire.amperage = wire.voltage / wire.resistance;
            }
            else if (!float.IsNaN(wire.resistance) && !float.IsNaN(wire.amperage))
            {
                wire.voltage = wire.amperage * wire.resistance;
            }
        }
    }
}