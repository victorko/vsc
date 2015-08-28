using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Vsc.Core
{
    public class Computer
    {
        private readonly double[] memory;
        private readonly double[] registers;
        private readonly Queue<double> inputQueue;

        public Computer(int memoryCapacity, double[] program)
        {
            Contract.Requires(memoryCapacity > 0);
            Contract.Requires(program != null && program.Length > 0);
            Contract.Requires(memoryCapacity >= program.Length);

            this.memory = new double[memoryCapacity];
            Array.Copy(program, this.memory, program.Length);

            var registersCount = Enum.GetNames(typeof(Register)).Length;
            this.registers = new double[registersCount];

            this.inputQueue = new Queue<double>();
        }

        public double GetRegisterValue(Register register)
        {
            return this.registers[(int)register];
        }

        public double GetMemoryCellValue(int address)
        {
            Contract.Requires(address >= 0);

            return this.memory[address];
        }

        public void Input(double value)
        {
            this.inputQueue.Enqueue(value);

            if (this.IsWaitingForInput)
            {
                this.CompleteInput();
                this.IsWaitingForInput = false;
            }
        }

        public event EventHandler<RegisterValueChangedEventArgs> RegisterValueChanged;

        public event EventHandler<MemoryCellValueChangedEventArgs> MemoryCellValueChanged;

        public event EventHandler InputRequested;

        public event EventHandler<OutputPerformedEventArgs> OutputPerformed;

        public event EventHandler<NotifyMicroActionEventArgs> NotifyMicroAction;

        public void Step()
        {
            throw new NotImplementedException();
        }

        public bool IsWorking { get; private set; }

        public bool IsWaitingForInput { get; private set; }


        private void CompleteInput()
        {
            var value = this.inputQueue.Dequeue();
            SetRegisterValue(Register.I, value);
        }

        private void MoveValue(Register from, Register to)
        {
            this.FireNotifyMicroAction($"{from} -> {to}");

            var value = this.GetRegisterValue(from);
            SetRegisterValue(to, value);
        }

        private void RequestInput()
        {
            FireNotifyMicroAction("Input");

            if (this.InputRequested != null)
            {
                this.InputRequested(this, EventArgs.Empty);
            }

            if (this.inputQueue.Count > 0)
            {
                this.CompleteInput();
            }
            else
            {
                this.IsWaitingForInput = true;
            }
        }

        private void Read()
        {
            FireNotifyMicroAction("Read");

            var address = (int)this.GetRegisterValue(Register.A);
            Contract.Assert(address < this.memory.Length);

            var value = GetMemoryCellValue(address);
            SetRegisterValue(Register.D, value);
        }

        private void Write()
        {
            FireNotifyMicroAction("Write");

            var address = (int)this.GetRegisterValue(Register.A);
            Contract.Assert(address < this.memory.Length);

            var value = GetRegisterValue(Register.D);
            SetMemoryCellValue(address, value);
        }

        private void Next()
        {
            FireNotifyMicroAction("Next");

            var value = GetRegisterValue(Register.PC);
            SetRegisterValue(Register.PC, value + 1);
        }

        private void Output()
        {
            FireNotifyMicroAction("Output");

            if (this.OutputPerformed != null)
            {
                var value = GetRegisterValue(Register.O);
                this.OutputPerformed(this, new OutputPerformedEventArgs(value));
            }
        }

        private void Add()
        {
            FireNotifyMicroAction("Add");

            var x = GetRegisterValue(Register.X);
            var y = GetRegisterValue(Register.Y);
            SetRegisterValue(Register.Z, x + y);
        }

        private void Substruct()
        {
            FireNotifyMicroAction("Substruct");

            var x = GetRegisterValue(Register.X);
            var y = GetRegisterValue(Register.Y);
            SetRegisterValue(Register.Z, x - y);
        }

        private void Multiply()
        {
            FireNotifyMicroAction("Multiply");

            var x = GetRegisterValue(Register.X);
            var y = GetRegisterValue(Register.Y);
            SetRegisterValue(Register.Z, x * y);
        }

        private void Divide()
        {
            FireNotifyMicroAction("Divide");

            var x = GetRegisterValue(Register.X);
            var y = GetRegisterValue(Register.Y);
            SetRegisterValue(Register.Z, x / y);
        }

        private void FireNotifyMicroAction(string description)
        {
            if (this.NotifyMicroAction != null)
            {
                this.NotifyMicroAction(this, new NotifyMicroActionEventArgs(description));
            }
        }

        private void SetRegisterValue(Register register, double value)
        {
            var oldValue = GetRegisterValue(register);
            this.registers[(int)register] = value;

            if (this.RegisterValueChanged != null)
            {
                this.RegisterValueChanged(this, new RegisterValueChangedEventArgs(register, oldValue, value));
            }
        }

        private void SetMemoryCellValue(int address, double value)
        {
            var oldValue = GetMemoryCellValue(address);
            this.memory[address] = value;

            if (this.MemoryCellValueChanged != null)
            {
                this.MemoryCellValueChanged(this, new MemoryCellValueChangedEventArgs(address, oldValue, value));
            }
        }
    }
}
