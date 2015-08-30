using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Vsc.Core;

namespace Vsc.Test
{
    [TestClass]
    public class CommandTests
    {
        private Computer CreateComputer(double[] program, double inputValue, List<string> log)
        {
            var computer = new Computer(100, program);
            computer.NotifyMicroAction += (sender, e) => log.Add(e.ActionDescription);
            computer.IO.InputRequested += (sender, e) => computer.IO.Input(inputValue);
            return computer;
        }

        [TestMethod]
        public void TestSTOP()
        {
            var program = new double[]
            {
                (int)OpCode.STOP
            };
            var input = 0.0;
            var expectedLog = "PC->A | Read | Decoded STOP | Stop";

            var log = new List<string>();
            var computer = CreateComputer(program, input, log);
            computer.Step();

            Assert.AreEqual(expectedLog, string.Join(" | ", log));
            Assert.IsTrue(computer.IsStopped);
        }

        [TestMethod]
        public void TestLDC_X()
        {
            var program = new double[]
            {
                (int)OpCode.LDC_X, 1
            };
            var input = 0.0;
            var expectedLog = "PC->A | Read | Decoded LDC_X | Next | PC->A | Read | D->X | Next";

            var log = new List<string>();
            var computer = CreateComputer(program, input, log);
            computer.Step();

            Assert.AreEqual(expectedLog, string.Join(" | ", log));
            Assert.AreEqual(1, computer.Registers[Register.X]);
            Assert.AreEqual(2, computer.Registers[Register.PC]);
        }
        [TestMethod]
        public void TestLDC_Y()
        {
            var program = new double[]
            {
                (int)OpCode.LDC_Y, 1
            };
            var input = 0.0;
            var expectedLog = "PC->A | Read | Decoded LDC_Y | Next | PC->A | Read | D->Y | Next";

            var log = new List<string>();
            var computer = CreateComputer(program, input, log);
            computer.Step();

            Assert.AreEqual(expectedLog, string.Join(" | ", log));
            Assert.AreEqual(1, computer.Registers[Register.Y]);
            Assert.AreEqual(2, computer.Registers[Register.PC]);
        }

        [TestMethod]
        public void TestLDC_P()
        {
            var program = new double[]
            {
                (int)OpCode.LDC_P, 1
            };
            var input = 0.0;
            var expectedLog = "PC->A | Read | Decoded LDC_P | Next | PC->A | Read | D->P | Next";

            var log = new List<string>();
            var computer = CreateComputer(program, input, log);
            computer.Step();

            Assert.AreEqual(expectedLog, string.Join(" | ", log));
            Assert.AreEqual(1, computer.Registers[Register.P]);
            Assert.AreEqual(2, computer.Registers[Register.PC]);
        }

        [TestMethod]
        public void TestIN_X()
        {
            var program = new double[]
            {
                (int)OpCode.IN_X
            };
            var input = 15.0;
            var expectedLog = "PC->A | Read | Decoded IN_X | Input | I->X | Next";

            var log = new List<string>();
            var computer = CreateComputer(program, input, log);
            computer.Step();

            Assert.AreEqual(expectedLog, string.Join(" | ", log));
            Assert.AreEqual(15, computer.Registers[Register.I]);
            Assert.AreEqual(1, computer.Registers[Register.PC]);
        }
    }
}
