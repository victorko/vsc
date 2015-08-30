using System;

namespace Vsc.Core
{
    public class RegisterValueChangedEventArgs : EventArgs
    {
        internal RegisterValueChangedEventArgs(Register register, double oldValue, double newValue)
        {
            this.Register = register;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public Register Register { get; }

        public double OldValue { get; }

        public double NewValue { get; }
    }
}
