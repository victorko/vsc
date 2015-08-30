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

        public int Address { get; }

        public double OldValue { get; }

        public double NewValue { get; }
    }
}
