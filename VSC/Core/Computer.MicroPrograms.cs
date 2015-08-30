using System;
using System.Diagnostics.Contracts;

namespace Vsc.Core
{
    public partial class Computer
    {
        private Action[] microPrograms;

		private void InitMicroPrograms()
        {
			this.microPrograms = new Action[]
            {
				this.PerformSTOP,
                this.PerformLDC_X, this.PerformLDC_Y, this.PerformLDC_P,
				this.PerformLDA_X, this.PerformLDA_Y, this.PerformLDA_P,
				this.PerformLDP_X, this.PerformLDP_Y,
				this.PerformSTA_X, this.PerformSTA_Y, this.PerformSTA_Z, this.PerformSTA_P,
				this.PerformSTP_X, this.PerformSTP_Y, this.PerformSTP_Z,
				this.PerformIN_X, this.PerformIN_Y,
				this.PerformOUT_X, this.PerformOUT_Y, this.PerformOUT_Z,
				this.PerformCZP, this.PerformCPX, this.PerformCPY,
				this.PerformJMP, this.PerformJIZ, this.PerformJIP, this.PerformJIN,
				this.PerformADD, this.PerformSUB, this.PerformMUL, this.PerformDIV
			};

            Contract.Assert(this.microPrograms.Length == Enum.GetNames(typeof(OpCode)).Length);
        }
        
        private void PerformSTOP()
        {
            this.Stop();
        }

        private void PerformLDC_X()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.X);
            this.Next();
        }

        private void PerformLDC_Y()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.Y);
            this.Next();
        }

        private void PerformLDC_P()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.P);
            this.Next();
        }

        private void PerformLDA_X()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.X);
            this.Next();
        }

        private void PerformLDA_Y()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.Y);
            this.Next();
        }

        private void PerformLDA_P()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.P);
            this.Next();
        }

        private void PerformLDP_X()
        {
            this.CopyValue(Register.P, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.X);
            this.Next();
        }

        private void PerformLDP_Y()
        {
            this.CopyValue(Register.P, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.Y);
            this.Next();
        }

        private void PerformSTA_X()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.A);
            this.CopyValue(Register.X, Register.D);
            this.Write();
            this.Next();
        }

        private void PerformSTA_Y()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.A);
            this.CopyValue(Register.Y, Register.D);
            this.Write();
            this.Next();
        }

        private void PerformSTA_Z()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.A);
            this.CopyValue(Register.Z, Register.D);
            this.Write();
            this.Next();
        }

        private void PerformSTA_P()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.A);
            this.CopyValue(Register.P, Register.D);
            this.Write();
            this.Next();
        }

        private void PerformSTP_X()
        {
            this.CopyValue(Register.P, Register.A);
            this.CopyValue(Register.X, Register.D);
            this.Write();
            this.Next();
        }

        private void PerformSTP_Y()
        {
            this.CopyValue(Register.P, Register.A);
            this.CopyValue(Register.Y, Register.D);
            this.Write();
            this.Next();
        }

        private void PerformSTP_Z()
        {
            this.CopyValue(Register.P, Register.A);
            this.CopyValue(Register.Z, Register.D);
            this.Write();
            this.Next();
        }

        private void PerformIN_X()
        {
            this.Input(() =>
            {
                this.CopyValue(Register.I, Register.X);
                this.Next();
            });
        }

        private void PerformIN_Y()
        {
            this.Input(() =>
            {
                this.CopyValue(Register.I, Register.Y);
                this.Next();
            });
        }

        private void PerformOUT_X()
        {
            this.CopyValue(Register.X, Register.O);
            this.Output();
            this.Next();
        }

        private void PerformOUT_Y()
        {
            this.CopyValue(Register.Y, Register.O);
            this.Output();
            this.Next();
        }

        private void PerformOUT_Z()
        {
            this.CopyValue(Register.Z, Register.O);
            this.Output();
            this.Next();
        }

        private void PerformCZP()
        {
            this.CopyValue(Register.Z, Register.P);
            this.Next();
        }

        private void PerformCPX()
        {
            this.CopyValue(Register.P, Register.X);
            this.Next();
        }

        private void PerformCPY()
        {
            this.CopyValue(Register.P, Register.Y);
            this.Next();
        }

        private void PerformJMP()
        {
            this.Next();
            this.CopyValue(Register.PC, Register.A);
            this.Read();
            this.CopyValue(Register.D, Register.PC);
        }

        private void PerformJIZ()
        {
            this.Next();
			if (this.Check(Flag.Zero))
            {
                this.CopyValue(Register.PC, Register.A);
                this.Read();
                this.CopyValue(Register.D, Register.PC);
            }
            else
            {
                this.Next();
            }
        }

        private void PerformJIP()
        {
            this.Next();
            if (this.Check(Flag.Positive))
            {
                this.CopyValue(Register.PC, Register.A);
                this.Read();
                this.CopyValue(Register.D, Register.PC);
            }
            else
            {
                this.Next();
            }
        }

        private void PerformJIN()
        {
            this.Next();
            if (this.Check(Flag.Negative))
            {
                this.CopyValue(Register.PC, Register.A);
                this.Read();
                this.CopyValue(Register.D, Register.PC);
            }
            else
            {
                this.Next();
            }
        }

        private void PerformADD()
        {
            this.Add();
            this.Next();
        }

        private void PerformSUB()
        {
            this.Substruct();
            this.Next();
        }

        private void PerformMUL()
        {
            this.Multiply();
            this.Next();
        }

        private void PerformDIV()
        {
            this.Divide();
            this.Next();
        }
    }
}
