using System;
//using System.Diagnostics.Contracts;

namespace Vsc.Core
{
    public partial class Computer
    {
        public Computer(int memoryCapacity, double[] program)
        {
            //Contract.Requires(memoryCapacity > 0);
            //Contract.Requires(program != null);
            //Contract.Requires(program.Length > 0);
            //Contract.Requires(memoryCapacity >= program.Length);

            this.InitMicroPrograms();

            this.Memory = new Memory(memoryCapacity, program);
            this.Registers = new Registers();
            this.IO = new IO();
        }

        public Memory Memory { get; }

        public Registers Registers { get; }

        public IO IO { get; }

        public bool IsStopped { get; private set; }

        public event EventHandler<NotifyMicroActionEventArgs> NotifyMicroAction;

        public void Step()
        {
            try
            {
                this.CopyValue(Register.PC, Register.A);
                this.Read();
                var microProgram = this.DecodeCommand();
                microProgram();
            }
            catch
            {
                this.Stop();
                throw;
            }
        }        


        #region Micro actions

        private Action DecodeCommand()
        {
            var command = (int)this.Registers[Register.D];
            
            if (command < 0 || command > this.microPrograms.Length)
                throw new Exception("Invalid command OpCode");
            this.FireNotifyMicroAction($"Decoded {(OpCode)command}");

            return this.microPrograms[command];
        }

        private void Stop()
        {
            this.FireNotifyMicroAction("Stop");

            this.IsStopped = true;
        }

        private void CopyValue(Register from, Register to)
        {
            this.FireNotifyMicroAction($"{from}->{to}");

            var value = this.Registers[from];
            this.Registers[to] = value;
        }
       
        private void Read()
        {
            this.FireNotifyMicroAction("Read");

            var address = (int)this.Registers[Register.A];
            var value = this.Memory[address];
            this.Registers[Register.D] = value;
        }

        private void Write()
        {
            this.FireNotifyMicroAction("Write");

            var address = (int)this.Registers[Register.A];
            var value = this.Registers[Register.D];
            this.Memory[address] = value;
        }

        private void Next()
        {
            this.FireNotifyMicroAction("Next");

            var value = this.Registers[Register.PC];
            this.Registers[Register.PC] = value + 1;
        }

        private void Input(Action onInputCompleted)
        {
            this.FireNotifyMicroAction("Input");

            this.IO.RequestInput(() =>
            {
                double value;
                if (this.IO.TryGetInputValue(out value))
                    this.Registers[Register.I] = value;
                onInputCompleted();
            });
        }
 
        private void Output()
        {
            this.FireNotifyMicroAction("Output");

            var value = this.Registers[Register.O];
            this.IO.Output(value);
        }

        private void Add()
        {
            this.FireNotifyMicroAction("Add");

            var x = this.Registers[Register.X];
            var y = this.Registers[Register.Y];
            this.Registers[Register.Z] = x + y;
        }

        private void Substruct()
        {
            this.FireNotifyMicroAction("Substruct");

            var x = this.Registers[Register.X];
            var y = this.Registers[Register.Y];
            this.Registers[Register.Z] = x - y;
        }

        private void Multiply()
        {
            this.FireNotifyMicroAction("Multiply");

            var x = this.Registers[Register.X];
            var y = this.Registers[Register.Y];
            this.Registers[Register.Z] = x * y;
        }

        private void Divide()
        {
            this.FireNotifyMicroAction("Divide");

            var x = this.Registers[Register.X];
            var y = this.Registers[Register.Y];
            this.Registers[Register.Z] = x / y;
        }

        private bool Check(Flag flag)
        {
            var value = this.Registers[Register.Z];
            switch (flag)
            {
                case Flag.Zero:
                    return value == 0;

                case Flag.Positive:
                    return value > 0;

                case Flag.Negative:
                    return value < 0;
            }
            throw new Exception($"Unexpected flag {flag}");
        }

        #endregion

        private void FireNotifyMicroAction(string description)
        {
            if (this.NotifyMicroAction != null)
            {
                var eventArgs = new NotifyMicroActionEventArgs(description);
                this.NotifyMicroAction(this, eventArgs);
            }
        }
    }
}
