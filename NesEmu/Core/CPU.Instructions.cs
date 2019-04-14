﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesEmu.Core
{
    public class OPCODE
    {
        //http://www.oxyron.de/html/opcodes02.html
        //http://www.thealmightyguru.com/Games/Hacking/Wiki/index.php/6502_Opcodes

        public byte _opcode;
        public AdressingMode _adressingmode;
        public int _cycles;

        public bool _pageboundarycrossed;
    }

    public partial class CPU
    {
        private Instruction[] _instructions;
        private string[] _instruction_names;
        private int[] _addressModes, _instructionSizes, _instructionCycles, _instructionPageCycles;
        private int _cycle; //cycle counter

        public delegate void Instruction(AdressingMode mode, ushort address);

        public void ExecuteOpCode(byte opcode, AdressingMode mode, ushort address)
        {
            _instructions[opcode](mode, address);
        }

        public string GetOpCodeName(byte opcode)
        {
            return _instruction_names[opcode];
        }

        #region InstructionHelperFunctions

        /// <summary>
        /// Clears the Negative Flag if the Operand is $#00-7F, otherwise sets it.
        /// </summary>
        /// <param name="Operand"></param>
        private void SetNegative(byte value)
        {
            SF.Negative = ((value >> 7) & 1) == 1;
            //0b1xxx_xxxx true
            //0b0xxx_xxxx false $#00-7F
        }

        /// <summary>
        /// Sets the Zero Flag if the Operand is $#00, otherwise clears it.
        /// </summary>
        /// <param name="value"></param>
        private void SetZero(byte value)
        {
            SF.Zero = value == 0;
        }

        private void Set_Negative_and_Zero(byte value)
        {
            SetNegative(value);
            SetZero(value);
        }

        #endregion

        #region Instructions

        private void SetInstructions()
        {
            _instructions = new Instruction[256]
            {
                //0,   1,   2,   3,   4,   5,   6,   7,   8,   9,   A,   B,   C,   D,   E,   F
                BRK, ORA, ___, ___, ___, ORA, ___, ___, ___, ORA, ___, ___, ___, ORA, ___, ___,//0
                ___, ORA, ___, ___, ___, ORA, ___, ___, ___, ORA, ___, ___, ___, ORA, ___, ___,//1
                ___, AND, ___, ___, ___, AND, ___, ___, ___, AND, ___, ___, ___, AND, ___, ___,//2
                ___, AND, ___, ___, ___, AND, ___, ___, ___, AND, ___, ___, ___, AND, ___, ___,//3
                ___, EOR, ___, ___, ___, EOR, ___, ___, ___, EOR, ___, ___, ___, EOR, ___, ___,//4
                ___, EOR, ___, ___, ___, EOR, ___, ___, ___, EOR, ___, ___, ___, EOR, ___, ___,//5
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//6
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//7
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//8
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//9
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//A
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//B
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//C
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//D
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//E
                ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___, ___,//F
            };

            _instruction_names = new String[256]
{
                //0,     1,     2,     3,     4,     5,     6,     7,     8,     9,     A,     B,     C,     D,     E,     F
                "BRK", "ORA", "___", "___", "___", "ORA", "___", "___", "___", "ORA", "___", "___", "___", "ORA", "___", "___",//0
                "___", "ORA", "___", "___", "___", "ORA", "___", "___", "___", "ORA", "___", "___", "___", "ORA", "___", "___",//1
                "___", "AND", "___", "___", "___", "AND", "___", "___", "___", "AND", "___", "___", "___", "AND", "___", "___",//2
                "___", "AND", "___", "___", "___", "AND", "___", "___", "___", "AND", "___", "___", "___", "AND", "___", "___",//3
                "___", "EOR", "___", "___", "___", "EOR", "___", "___", "___", "EOR", "___", "___", "___", "EOR", "___", "___",//4
                "___", "EOR", "___", "___", "___", "EOR", "___", "___", "___", "EOR", "___", "___", "___", "EOR", "___", "___",//5
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//6
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//7
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//8
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//9
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//A
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//B
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//C
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//D
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//E
                "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___", "___",//F
};

            _addressModes = new int[256]{
//           0, 1, 2, 3,  4,  5,  6,  7, 8, 9, A, B, C, D, E, F
             6, 7, 6, 7, 11, 11, 11, 11, 6, 5, 4, 5, 1, 1, 1, 1,//0
            10, 9, 6, 9, 12, 12, 12, 12, 6, 3, 6, 3, 2, 2, 2, 2,//1
             1, 7, 6, 7, 11, 11, 11, 11, 6, 5, 4, 5, 1, 1, 1, 1,//2
            10, 9, 6, 9, 12, 12, 12, 12, 6, 3, 6, 3, 2, 2, 2, 2,//3
             6, 7, 6, 7, 11, 11, 11, 11, 6, 5, 4, 5, 1, 1, 1, 1,//4
            10, 9, 6, 9, 12, 12, 12, 12, 6, 3, 6, 3, 2, 2, 2, 2,//5
             6, 7, 6, 7, 11, 11, 11, 11, 6, 5, 4, 5, 8, 1, 1, 1,//6
            10, 9, 6, 9, 12, 12, 12, 12, 6, 3, 6, 3, 2, 2, 2, 2,//7
             5, 7, 5, 7, 11, 11, 11, 11, 6, 5, 6, 5, 1, 1, 1, 1,//8
            10, 9, 6, 9, 12, 12, 13, 13, 6, 3, 6, 3, 2, 2, 3, 3,//9
             5, 7, 5, 7, 11, 11, 11, 11, 6, 5, 6, 5, 1, 1, 1, 1,//A
            10, 9, 6, 9, 12, 12, 13, 13, 6, 3, 6, 3, 2, 2, 3, 3,//B
             5, 7, 5, 7, 11, 11, 11, 11, 6, 5, 6, 5, 1, 1, 1, 1,//C
            10, 9, 6, 9, 12, 12, 12, 12, 6, 3, 6, 3, 2, 2, 2, 2,//D
             5, 7, 5, 7, 11, 11, 11, 11, 6, 5, 6, 5, 1, 1, 1, 1,//E
            10, 9, 6, 9, 12, 12, 12, 12, 6, 3, 6, 3, 2, 2, 2, 2,//F
        };

            _instructionSizes = new int[256]{
//          0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F
            1, 2, 0, 0, 2, 2, 2, 0, 1, 2, 1, 0, 3, 3, 3, 0,//0
            2, 2, 0, 0, 2, 2, 2, 0, 1, 3, 1, 0, 3, 3, 3, 0,//1
            3, 2, 0, 0, 2, 2, 2, 0, 1, 2, 1, 0, 3, 3, 3, 0,//2
            2, 2, 0, 0, 2, 2, 2, 0, 1, 3, 1, 0, 3, 3, 3, 0,//3
            1, 2, 0, 0, 2, 2, 2, 0, 1, 2, 1, 0, 3, 3, 3, 0,//4
            2, 2, 0, 0, 2, 2, 2, 0, 1, 3, 1, 0, 3, 3, 3, 0,//5
            1, 2, 0, 0, 2, 2, 2, 0, 1, 2, 1, 0, 3, 3, 3, 0,//6
            2, 2, 0, 0, 2, 2, 2, 0, 1, 3, 1, 0, 3, 3, 3, 0,//7
            2, 2, 0, 0, 2, 2, 2, 0, 1, 0, 1, 0, 3, 3, 3, 0,//8
            2, 2, 0, 0, 2, 2, 2, 0, 1, 3, 1, 0, 0, 3, 0, 0,//9
            2, 2, 2, 0, 2, 2, 2, 0, 1, 2, 1, 0, 3, 3, 3, 0,//A
            2, 2, 0, 0, 2, 2, 2, 0, 1, 3, 1, 0, 3, 3, 3, 0,//B
            2, 2, 0, 0, 2, 2, 2, 0, 1, 2, 1, 0, 3, 3, 3, 0,//C
            2, 2, 0, 0, 2, 2, 2, 0, 1, 3, 1, 0, 3, 3, 3, 0,//D
            2, 2, 0, 0, 2, 2, 2, 0, 1, 2, 1, 0, 3, 3, 3, 0,//E
            2, 2, 0, 0, 2, 2, 2, 0, 1, 3, 1, 0, 3, 3, 3, 0,//F
        };

            _instructionCycles = new int[256]{
//          0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F
            7, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 4, 4, 6, 6,//0
            2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,//1
            6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 4, 4, 6, 6,//2
            2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,//3
            6, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 3, 4, 6, 6,//4
            2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,//5
            6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 5, 4, 6, 6,//6
            2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,//7
            2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,//8
            2, 6, 2, 6, 4, 4, 4, 4, 2, 5, 2, 5, 5, 5, 5, 5,//9
            2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,//A
            2, 5, 2, 5, 4, 4, 4, 4, 2, 4, 2, 4, 4, 4, 4, 4,//B
            2, 6, 2, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,//C
            2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,//D
            2, 6, 2, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,//E
            2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,//F
        };

            _instructionPageCycles = new int[256]{
//          0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//0
            1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0,//1
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//2
            1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0,//3
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//4
            1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0,//5
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//6
            1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0,//7
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//8
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//9
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//A
            1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1,//B
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//C
            1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0,//D
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,//E
            1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0,//F
        };
        }

        public void BRK(AdressingMode mode, ushort address)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ORA (Or Memory With Accumulator) performs a logical OR on the operand and the accumulator and stores the result in the accumulator. This opcode is similar in function to AND and EOR.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void ORA(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            Operand |= A;
            Set_Negative_and_Zero(Operand);
            A = Operand;
        }

        /// <summary>
        /// EOR (Exclusive OR Memory With Accumulator) performs a logical XOR on the operand and the accumulator and stores the result in the accumulator. This opcode is similar in function to AND and ORA.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void EOR(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            Operand ^= A;
            Set_Negative_and_Zero(Operand);
            A = Operand;
        }

        /// <summary>
        /// Accumulator performs a logical AND on the operand and the accumulator and stores the result in the accumulator. This opcode is similar in function to ORA and EOR.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void AND(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            Operand &= A;
            Set_Negative_and_Zero(Operand);
            A = Operand;
        }

        /// <summary>
        /// Add with carry. Adds Operand to A register, sets Zero, Negative, Carry, Overflow flags
        /// https://stackoverflow.com/questions/29193303/6502-emulation-proper-way-to-implement-adc-and-sbc
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void ADC(AdressingMode mode, ushort address)
        {
            //Decimal mode was cut from NES

            byte Operand = ReadByte(address);
            int carry = SF.Carry ? 1 : 0;

            byte sum = (byte)(A + Operand + carry);
            Set_Negative_and_Zero(sum);

            SF.Carry = (A + Operand + carry) > 0xFF;
            SF.Overflow = (~(A ^ Operand) & (A ^ sum) & 0x80) != 0;

            A = sum;

            //0b1xxx_xxxx       0011
            //0b0xxx_xxxx       0101
            //-----------  XOR  ----
            //0b1xxx_xxxx       0110

            //(A ^ Operand)     (Different)Negative and Positive
            //~(A ^ Operand)    Same

            //(A ^ sum)         Different

            //0x80=0b1000_0000  Extract the important bit
        }

        /// <summary>
        /// Substract with carry. Substracts Operand from A register, sets Zero, Negative, Carry, Overflow flags
        /// https://stackoverflow.com/questions/29193303/6502-emulation-proper-way-to-implement-adc-and-sbc
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void SBC(AdressingMode mode, ushort address)
        {
            //same as ADC with with complement value input

            byte Operand = (byte)~ReadByte(address); //Complement
            int carry = SF.Carry ? 1 : 0;

            byte sum = (byte)(A + Operand + carry);
            Set_Negative_and_Zero(sum);

            SF.Carry = (A + Operand + carry) > 0xFF;
            SF.Overflow = (~(A ^ Operand) & (A ^ sum) & 0x80) != 0;

            A = sum;
        }

        public void ___(AdressingMode mode, ushort address)
        {
            try
            {
                throw new Exception("Illegal OPCODE");
            }
            catch(Exception e)
            {

            }
        }

        #endregion
    }
}
