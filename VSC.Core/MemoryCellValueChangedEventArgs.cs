using System;

namespace Vsc.Core
{
    public class MemoryCellValueChangedEventArgs : EventArgs
    {
        internal MemoryCellValueChangedEventArgs(int address, double oldValue, double newValue)
        {
            this.Address = address;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public int Address { get; private set; }

        public double OldValue { get; private set; }

        public double NewValue { get; private set; }
    }
}
