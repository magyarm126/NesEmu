using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NesEmu.Core;

namespace CPU_test
{
    [TestClass]
    public class RegisterTest
    {
        [TestMethod]
        public void GetValues()
        {
            CPU cPU = new CPU();

            Assert.AreEqual(0, cPU.A);
            Assert.AreEqual(0, cPU.P);
            Assert.AreEqual(0, cPU.PC);
            Assert.AreEqual(0, cPU.S);
            Assert.AreEqual(0, cPU.X);
            Assert.AreEqual(0, cPU.Y);
        }

        [TestMethod]
        public void SetValues()
        {
            CPU cPU = new CPU
            {
                A = 0b0000_0000,
                P = 0b0000_0001,
                PC= 0b0000_0010,
                S = 0b0000_0011,
                X = 0b0000_0100,
                Y = 0b0000_0101
            };

            Assert.AreEqual(0b0000_0000, cPU.A);
            Assert.AreEqual(0b0000_0001, cPU.P);
            Assert.AreEqual(0b0000_0010, cPU.PC);
            Assert.AreEqual(0b0000_0011, cPU.S);
            Assert.AreEqual(0b0000_0100, cPU.X);
            Assert.AreEqual(0b0000_0101, cPU.Y);
        }

        [TestMethod]
        public void Status_flags_individual()
        {
            CPU cPU = new CPU();

            Assert.AreEqual(false, cPU.SF.Carry);
            Assert.AreEqual(0b0000_0000, cPU.P);
            cPU.SF.Carry = true;
            Assert.AreEqual(true, cPU.SF.Carry);
            Assert.AreEqual(0b0000_0001, cPU.P);
            cPU.SF.Carry = false;

            Assert.AreEqual(false, cPU.SF.Zero);
            Assert.AreEqual(0b0000_0000, cPU.P);
            cPU.SF.Zero = true;
            Assert.AreEqual(true, cPU.SF.Zero);
            Assert.AreEqual(0b0000_0010, cPU.P);
            cPU.SF.Zero = false;

            Assert.AreEqual(false, cPU.SF.Interrupt_Disable);
            Assert.AreEqual(0b0000_0000, cPU.P);
            cPU.SF.Interrupt_Disable = true;
            Assert.AreEqual(true, cPU.SF.Interrupt_Disable);
            Assert.AreEqual(0b0000_0100, cPU.P);
            cPU.SF.Interrupt_Disable = false;

            Assert.AreEqual(false, cPU.SF.Decimal);
            Assert.AreEqual(0b0000_0000, cPU.P);
            cPU.SF.Decimal = true;
            Assert.AreEqual(true, cPU.SF.Decimal);
            Assert.AreEqual(0b0000_1000, cPU.P);
            cPU.SF.Decimal = false;

            Assert.AreEqual(false, cPU.SF.Bit4);
            Assert.AreEqual(0b0000_0000, cPU.P);
            cPU.SF.Bit4 = true;
            Assert.AreEqual(true, cPU.SF.Bit4);
            Assert.AreEqual(0b0001_0000, cPU.P);
            cPU.SF.Bit4 = false;

            Assert.AreEqual(false, cPU.SF.Bit5);
            Assert.AreEqual(0b0000_0000, cPU.P);
            cPU.SF.Bit5 = true;
            Assert.AreEqual(true, cPU.SF.Bit5);
            Assert.AreEqual(0b0010_0000, cPU.P);
            cPU.SF.Bit5 = false;

            Assert.AreEqual(false, cPU.SF.Overflow);
            Assert.AreEqual(0b0000_0000, cPU.P);
            cPU.SF.Overflow = true;
            Assert.AreEqual(true, cPU.SF.Overflow);
            Assert.AreEqual(0b0100_0000, cPU.P);
            cPU.SF.Overflow = false;

            Assert.AreEqual(false, cPU.SF.Negative);
            Assert.AreEqual(0b0000_0000, cPU.P);
            cPU.SF.Negative = true;
            Assert.AreEqual(true, cPU.SF.Negative);
            Assert.AreEqual(0b1000_0000, cPU.P);
            cPU.SF.Negative = false;
        }

        [TestMethod]
        public void Status_flags_seq()
        {
            CPU cPU = new CPU();


            cPU.SF.Bit4 = true;
            cPU.SF.Overflow = true;
            cPU.SF.Bit5 = true;
            cPU.SF.Negative = true;

            Assert.AreEqual(0b1111_0000, cPU.P);

            cPU.SF.Carry = true;
            cPU.SF.Interrupt_Disable = true;
            cPU.SF.Zero = true;
            cPU.SF.Decimal  = true;

            Assert.AreEqual(0b1111_1111, cPU.P);

            cPU.SF.Bit5 = false;
            cPU.SF.Negative = false;
            cPU.SF.Decimal = false;

            Assert.AreEqual(0b0101_0111, cPU.P);

        }

    }
}
