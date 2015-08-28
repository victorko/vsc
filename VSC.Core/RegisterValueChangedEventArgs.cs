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

        public Register Register { get; private set; }

        public double OldValue { get; private set; }

        public double NewValue { get; private set; }
    }
}
