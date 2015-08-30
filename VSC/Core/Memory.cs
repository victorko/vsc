using System;
//using System.Diagnostics.Contracts;

namespace Vsc.Core
{
    public class Memory
    {
        private readonly double[] data;

        internal Memory(int capacity, double[] initialState)
        {
            //Contract.Requires(capacity > 0);
            //Contract.Requires(initialState != null);
            //Contract.Requires(initialState.Length > 0);
            //Contract.Requires(initialState.Length <= capacity);

            this.data = new double[capacity];

            Array.Copy(initialState, this.data, initialState.Length);
        }

        public int Capacity
        {
            get { return this.data.Length; }
        }

        public double this[int address]
        {
            get
            {
                //Contract.Requires(address >= 0);
                //Contract.Requires(address < this.Capacity);

                return this.data[address];
            }
            set
            {
                //Contract.Requires(address >= 0);
                //Contract.Requires(address < this.Capacity);

                var oldValue = this.data[address];
                this.data[address] = value;
                if (this.CellValueChanged != null)
                {
                    var eventArgs = new MemoryCellValueChangedEventArgs(address, oldValue, value);
                    this.CellValueChanged(this, eventArgs);
                }
            }
        }

        public event EventHandler<MemoryCellValueChangedEventArgs> CellValueChanged;
    }
}
