using System;

namespace Vsc.Core
{
    public class OutputPerformedEventArgs : EventArgs
    {
        internal OutputPerformedEventArgs(double value)
        {
            this.Value = value;
        }

        public double Value { get; private set; }
    }
}
