using System;
using System.Collections.Generic;

namespace Vsc.Core
{
    public class IO
    {
        private readonly Queue<double> inputQueue;
        private Action onInputReady;

        public IO()
        {
            this.inputQueue = new Queue<double>();
        }

        public void Input(double value)
        {
            this.inputQueue.Enqueue(value);
            if (this.onInputReady != null)
            {
                this.onInputReady();
                this.onInputReady = null;
            }
        }

        public event EventHandler InputRequested;

        public event EventHandler<OutputPerformedEventArgs> OutputPerformed;

        internal void RequestInput(Action onInputReady)
        {
            if (this.inputQueue.Count > 0)
            {
                onInputReady();
            }
            else
            {
                this.onInputReady = onInputReady;

                if (this.InputRequested != null)
                    this.InputRequested(this, EventArgs.Empty);
            }
        }

        internal bool TryGetInputValue(out double value)
        {
            if (this.inputQueue.Count > 0)
            {
                value = this.inputQueue.Dequeue();
                return true;
            }
            else
            {
                value = 0;
                return false;
            }
        }

        internal void Output(double value)
        {
            if (this.OutputPerformed != null)
            {
                var eventArgs = new OutputPerformedEventArgs(value);
                this.OutputPerformed(this, eventArgs);
            }
        }
    }
}
