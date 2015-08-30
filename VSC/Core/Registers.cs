using System;

namespace Vsc.Core
{
    public class Registers
    {
        private readonly double[] data;

        public Registers()
        {
            var registerCount = Enum.GetNames(typeof(Register)).Length;
            this.data = new double[registerCount];
        }

        public double this[Register register]
        {
            get
            {
                return this.data[(int)register];
            }
            set
            {
                var index = (int)register;
                var oldValue = this.data[index];
                this.data[index] = value;
                if (this.RegisterValueChanged != null)
                {
                    var eventArgs = new RegisterValueChangedEventArgs(register, oldValue, value);
                    this.RegisterValueChanged(this, eventArgs);
                }
            }
        }

        public event EventHandler<RegisterValueChangedEventArgs> RegisterValueChanged;
    }
}
