﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesEmu.Core
{
    public partial class CPU
    {
        private Instruction[] _instructions;
        private string[] _instruction_names;
        private int[] _addressModes, _instructionSizes, _instructionCycles, _instructionPageCycles;
        public int _cycle { get; set; } //cycle counter

        public byte currentOpCode { get; set; }
        public byte lastOpCode { get; set; }

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
        /// Sets/Clears overflow flag based on bit 6
        /// </summary>
        /// <param name="Operand"></param>
        private void SetOverflow(byte value)
        {
            SF.Negative = ((value >> 6) & 1) == 1;
            //0bx1xx_xxxx true
            //0bx0xx_xxxx false
        }

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

        #region Init Instructions

        private void SetInstructions()
        {
            _instructions = new Instruction[256]
            {
                //0,   1,   2,   3,   4,   5,   6,   7,   8,   9,   A,   B,   C,   D,   E,   F
                BRK, ORA, ___, ___, ___, ORA, ASL, ___, PHP, ORA, ASL, ___, ___, ORA, ASL, ___,//0
                BPL, ORA, ___, ___, ___, ORA, ASL, ___, CLC, ORA, ___, ___, ___, ORA, ASL, ___,//1
                JSR, AND, ___, ___, BIT, AND, ROL, ___, PLP, AND, ROL, ___, BIT, AND, ROL, ___,//2
                BMI, AND, ___, ___, ___, AND, ROL, ___, SEC, AND, ___, ___, ___, AND, ROL, ___,//3
                RTI, EOR, ___, ___, ___, EOR, LSR, ___, PHA, EOR, LSR, ___, JMP, EOR, LSR, ___,//4
                BVC, EOR, ___, ___, ___, EOR, LSR, ___, CLI, EOR, ___, ___, ___, EOR, LSR, ___,//5
                RTS, ADC, ___, ___, ___, ADC, ROR, ___, PLA, ADC, ROR, ___, JMP, ADC, ROR, ___,//6
                BVS, ADC, ___, ___, ___, ADC, ROR, ___, SEI, ADC, ___, ___, ___, ADC, ROR, ___,//7
                ___, STA, ___, ___, STY, STA, STX, ___, DEY, STA, TXA, ___, STY, STA, STX, ___,//8
                BCC, STA, ___, ___, STY, STA, STX, ___, TYA, STA, TXS, ___, ___, STA, ___, ___,//9
                LDY, LDA, LDX, ___, LDY, LDA, LDX, ___, TAY, LDA, TAX, ___, LDY, LDA, LDX, ___,//A
                BCS, LDA, ___, ___, LDY, LDA, LDX, ___, CLV, LDA, TSX, ___, LDY, LDA, LDX, ___,//B
                CPY, CMP, ___, ___, CPY, CMP, DEC, ___, INY, CMP, DEX, ___, CPY, CMP, DEC, ___,//C
                BNE, CMP, ___, ___, ___, CMP, DEC, ___, CLD, CMP, ___, ___, ___, CMP, DEC, ___,//D
                CPX, SBC, ___, ___, CPX, SBC, INC, ___, INX, SBC, NOP, ___, CPX, SBC, INC, ___,//E
                BEQ, SBC, ___, ___, ___, SBC, INC, ___, SED, SBC, ___, ___, ___, SBC, INC, ___,//F
            };

            _instruction_names = new String[256]
            {
                //0,     1,     2,     3,     4,     5,     6,     7,     8,     9,     A,     B,     C,     D,     E,     F
                "BRK", "ORA", "___", "___", "___", "ORA", "ASL", "___", "PHP", "ORA", "ASL", "___", "___", "ORA", "ASL", "___",//0
                "BPL", "ORA", "___", "___", "___", "ORA", "ASL", "___", "CLC", "ORA", "___", "___", "___", "ORA", "ASL", "___",//1
                "JSR", "AND", "___", "___", "BIT", "AND", "ROL", "___", "PLP", "AND", "ROL", "___", "BIT", "AND", "ROL", "___",//2
                "BMI", "AND", "___", "___", "___", "AND", "ROL", "___", "SEC", "AND", "___", "___", "___", "AND", "ROL", "___",//3
                "RTI", "EOR", "___", "___", "___", "EOR", "LSR", "___", "PHA", "EOR", "LSR", "___", "JMP", "EOR", "LSR", "___",//4
                "BVC", "EOR", "___", "___", "___", "EOR", "LSR", "___", "CLI", "EOR", "___", "___", "___", "EOR", "LSR", "___",//5
                "RTS", "ADC", "___", "___", "___", "ADC", "ROR", "___", "PLA", "ADC", "ROR", "___", "JMP", "ADC", "ROR", "___",//6
                "BVS", "ADC", "___", "___", "___", "ADC", "ROR", "___", "SEI", "ADC", "___", "___", "___", "ADC", "ROR", "___",//7
                "___", "STA", "___", "___", "STY", "STA", "STX", "___", "DEY", "STA", "TXA", "___", "STY", "STA", "STX", "___",//8
                "BCC", "STA", "___", "___", "STY", "STA", "STX", "___", "TYA", "STA", "TXS", "___", "___", "STA", "___", "___",//9
                "LDY", "LDA", "LDX", "___", "LDY", "LDA", "LDX", "___", "TAY", "LDA", "TAX", "___", "LDY", "LDA", "LDX", "___",//A
                "BCS", "LDA", "___", "___", "LDY", "LDA", "LDX", "___", "CLV", "LDA", "TSX", "___", "LDY", "LDA", "LDX", "___",//B
                "CPY", "CMP", "___", "___", "CPY", "CMP", "DEC", "___", "INY", "CMP", "DEX", "___", "CPY", "CMP", "DEC", "___",//C
                "BNE", "CMP", "___", "___", "___", "CMP", "DEC", "___", "CLD", "CMP", "___", "___", "___", "CMP", "DEC", "___",//D
                "CPX", "SBC", "___", "___", "CPX", "SBC", "INC", "___", "INX", "SBC", "NOP", "___", "CPX", "SBC", "INC", "___",//E
                "BEQ", "SBC", "___", "___", "___", "SBC", "INC", "___", "SED", "SBC", "___", "___", "___", "SBC", "INC", "___",//F
            };

            _addressModes = new int[256]
            {
            //   0, 1, 2, 3,  4,  5,  6,  7, 8, 9, A, B, C, D, E, F
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

            _instructionSizes = new int[256]
            {
            //  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F
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

            _instructionCycles = new int[256]
            {
            //  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F
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

            _instructionPageCycles = new int[256]
            {
            //  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F
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

        #endregion

        #region Jump

        /// <summary>
        /// JMP - Jump
        /// Sets the program counter to the address specified by the operand.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void JMP(AdressingMode mode, ushort address)
        {
            PC = address;
        }

        /// <summary>
        /// JSR - Jump to Subroutine
        /// The JSR instruction pushes the address (minus one) of the return point on to the stack and then sets the program counter to the target memory address.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void JSR(AdressingMode mode, ushort address)
        {
            Push16((ushort)(PC - 1));
            PC = address;
        }

        /// <summary>
        /// RTI - Return from Interrupt
        /// The RTI instruction is used at the end of an interrupt processing routine.It pulls the processor flags from the stack followed by the program counter.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void RTI(AdressingMode mode, ushort address)
        {
            SF.P= PopByte();
            PC = Pop16();
        }

        /// <summary>
        /// RTS - Return from Subroutine
        /// The RTS instruction is used at the end of a subroutine to return to the calling routine. It pulls the program counter (minus one) from the stack.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void RTS(AdressingMode mode, ushort address)
        {
            PC = (ushort)(Pop16() + 1);
        }

        #endregion

        #region Branches

        /// <summary>
        /// Branch on PLus
        /// Branches are dependant on the status of the flag bits when the op code is encountered. A branch not taken requires two machine cycles. Add one if the branch is taken and add one more if the branch crosses a page boundary.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BPL(AdressingMode mode, ushort address)
        {
            if (!SF.Negative)
                Branch(address);
        }

        /// <summary>
        /// Branch on MInus
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BMI(AdressingMode mode, ushort address)
        {
            if (SF.Negative)
                Branch(address);
        }

        /// <summary>
        /// Branch on oVerflow Clear
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BVC(AdressingMode mode, ushort address)
        {
            if (!SF.Overflow)
                Branch(address);
        }

        /// <summary>
        /// Branch on oVerflow Set
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BVS(AdressingMode mode, ushort address)
        {
            if (SF.Overflow)
                Branch(address);
        }

        /// <summary>
        /// Branch on Carry Clear
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BCC(AdressingMode mode, ushort address)
        {
            if (!SF.Carry)
                Branch(address);
        }

        /// <summary>
        /// Branch on Carry Set
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BCS(AdressingMode mode, ushort address)
        {
            if (SF.Carry)
                Branch(address);
        }

        /// <summary>
        /// Branch on Not Equal
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BNE(AdressingMode mode, ushort address)
        {
            if (!SF.Zero)
                Branch(address);
        }

        /// <summary>
        /// Branch on EQual
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BEQ(AdressingMode mode, ushort address)
        {
            if (SF.Zero)
                Branch(address);
        }

        /// <summary>
        /// Branches are dependant on the status of the flag bits when the op code is encountered. A branch not taken requires two machine cycles. Add one if the branch is taken and add one more if the branch crosses a page boundary.
        /// </summary>
        /// <param name="address"></param>
        private void Branch(ushort address)
        {
            _cycle += IsPageCross(PC, address) ? 2 : 1;
            PC = address;
        }

        #endregion

        #region System

        /// <summary>
        /// When the MOS 6502 processor was modified into the Ricoh 2A03 chip for the NES, the BRK (Force Break) opcode was preserved. As on other 6502 family CPUs, the BRK instruction advances the program counter by 2, pushes the Program Counter Register and processor status register to the stack, sets the Interrupt Flag to temporarily prevent other IRQs from being executed, and reloads the Program Counter from the vector at $FFFE-$FFFF.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BRK(AdressingMode mode, ushort address)
        {
            Push16(PC);
            Push16(SF.P);
            SF.Bit5 = true;
            SF.Bit4 = true;
            PC = Read16(0xFFFE);
        }

        /// <summary>
        /// NOP - No Operation
        /// The NOP instruction causes no changes to the processor other than the normal incrementing of the program counter to the next instruction.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void NOP(AdressingMode mode, ushort address)
        {
            //Do nothing
        }

        #endregion

        #region Stack

        /// <summary>
        /// PLP - Pull Processor Status
        /// Pulls an 8 bit value from the stack and into the processor flags. The flags will take on new states as determined by the value pulled.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void PLP(AdressingMode mode, ushort address)
        {
            SF.P = PopByte(); //Maybe wrong
            //SF.P = (byte)(status & 0b1110_1111); //Set all except bit 4
        }

        /// <summary>
        /// PLA - Pull Accumulator
        /// Pulls an 8 bit value from the stack and into the accumulator. The zero and negative flags are set as appropriate.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void PLA(AdressingMode mode, ushort address)
        {
            byte Operand = PopByte();
            Set_Negative_and_Zero(Operand);
            A = Operand;
            
        }

        /// <summary>
        /// PHP - Push Processor Status
        /// Pushes a copy of the status flags on to the stack.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void PHP(AdressingMode mode, ushort address)
        {
            PushByte(SF.P); //maybe | 0x10;
        }

        /// <summary>
        /// PHA - Push Accumulator
        /// Pushes a copy of the accumulator on to the stack.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void PHA(AdressingMode mode, ushort address)
        {
            PushByte(A);
        }

        #endregion

        #region Bitwise

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
        /// ASL (Arithmetic Shift Left)
        /// ASL shifts all bits left one position. 0 is shifted into bit 0 and the original bit 7 is shifted into the Carry.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void ASL(AdressingMode mode, ushort address)
        {
            if(mode == AdressingMode.Accumulator)
            {
                SF.Carry = (A & 0b1000_0000) != 0; //overflow goes to carry from bit 7 instead of wrap around
                A <<= 1;
                Set_Negative_and_Zero(A);
            }
            else
            {
                byte value = ReadByte(address);
                SF.Carry = (value & 0b1000_0000) != 0; //overflow goes bit 7 instead of wrap around
                value <<= 1;
                Set_Negative_and_Zero(value);
                WriteByte(address, value);
            }

        }

        /// <summary>
        /// BIT sets the Z flag as though the value in the address tested were ANDed with the accumulator. The S and V flags are set to match bits 7 and 6 respectively in the value stored at the tested address.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void BIT(AdressingMode mode, ushort address)
        {
            byte value = ReadByte(address);
            SetNegative(value);
            SetOverflow(value);
            SetZero((byte)(value & A));
        }

        /// <summary>
        /// LSR - Logical Shift Right
        /// A,C,Z,N = A/2 or M,C,Z,N = M/2
        /// Each of the bits in A or M is shift one place to the right.The bit that was in bit 0 is shifted into the carry flag.Bit 7 is set to zero.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void LSR(AdressingMode mode, ushort address)
        {
            if (mode == AdressingMode.Accumulator)
            {
                SF.Carry = (A & 1) == 1; //overflow goes carry instead of wrap around
                A >>= 1;
                Set_Negative_and_Zero(A);
            }
            else
            {
                byte value = ReadByte(address);
                SF.Carry = (value & 1) == 1;
                value >>= 1;
                Set_Negative_and_Zero(value);
                WriteByte(address, value);
            }
        }

        /// <summary>
        /// ROR - Rotate Right
        /// Move each of the bits in either A or M one place to the right. Bit 7 is filled with the current value of the carry flag whilst the old bit 0 becomes the new carry flag value.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void ROR(AdressingMode mode, ushort address)
        {
            if (mode == AdressingMode.Accumulator)
            {
                bool orignalCarry = SF.Carry;

                SF.Carry = (A & 1) == 1;
                A >>= 1;
                A |= (byte)(orignalCarry ? 0x80 : 0);

                Set_Negative_and_Zero(A);
            }
            else
            {
                bool orignalCarry = SF.Carry;
                byte value = ReadByte(address);

                SF.Carry = (value & 1) == 1;
                value >>= 1;
                value |= (byte)(orignalCarry ? 0x80 : 0);

                WriteByte(address, value);
                Set_Negative_and_Zero(value);
            }
        }

        /// <summary>
        /// ROL - Rotate Left
        /// Move each of the bits in either A or M one place to the left. Bit 0 is filled with the current value of the carry flag whilst the old bit 7 becomes the new carry flag value.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void ROL(AdressingMode mode, ushort address)
        {
            if (mode == AdressingMode.Accumulator)
            {
                bool orignalCarry = SF.Carry;

                SF.Carry = (A & 0b1000_0000) != 0;
                A <<= 1;
                A |= (byte)(orignalCarry ? 1 : 0);

                Set_Negative_and_Zero(A);
            }
            else
            {
                bool orignalCarry = SF.Carry;
                byte value = ReadByte(address);

                SF.Carry = (value & 0b1000_0000) != 0;
                value <<= 1;
                value |= (byte)(orignalCarry ? 1 : 0);

                WriteByte(address, value);
                Set_Negative_and_Zero(value);
            }
        }

        #endregion

        #region Registers

        /// <summary>
        /// Compare sets flags as if a subtraction had been carried out. If the value in the accumulator is equal or greater than the compared value, the Carry will be set. The equal (Z) and negative (N) flags will be set based on equality or lack thereof and the sign (i.e. A>=$80) of the accumulator.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void CMP(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            SF.Carry = A >= Operand;
            Set_Negative_and_Zero((byte)(A - Operand));
        }

        /// <summary>
        /// CPY - Compare Y Register
        /// Z,C,N = Y-M
        /// This instruction compares the contents of the Y register with another memory held value and sets the zero and carry flags as appropriate.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void CPY(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            SF.Carry = Y >= Operand;
            Set_Negative_and_Zero((byte)(Y - Operand));
        }

        /// <summary>
        /// CPX - Compare X Register
        /// Z,C,N = X-M
        /// This instruction compares the contents of the X register with another memory held value and sets the zero and carry flags as appropriate.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void CPX(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            SF.Carry = X >= Operand;
            Set_Negative_and_Zero((byte)(X - Operand));
        }

        /// <summary>
        /// CLC (Clear Carry Flag) clears the Carry Flag in the Processor Status Register by setting the 0th bit 0. To set the carry flag, use SEC. Clearing the carry flag should be done prior to any instruction that might set it where you might need to read the carry flag's value after the instruction.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void CLC(AdressingMode mode, ushort address)
        {
            SF.Carry = false;
        }

        /// <summary>
        /// SEC (Set Carry Flag) sets the Carry Flag in the Processor Status Register by setting the 0th bit 1. To clear the carry flag, use CLC.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void SEC(AdressingMode mode, ushort address)
        {
            SF.Carry = true;
        }

        /// <summary>
        /// CLI (Clear Interrupt Disable Flag) clears the Interrupt Flag in the Processor Status Register by setting the 2nd bit 0. To set the interrupt disable flag, use SEI.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void CLI(AdressingMode mode, ushort address)
        {
            SF.Interrupt_Disable = false;
        }

        /// <summary>
        /// SEI (Set Interrupt Disable Flag) sets the Interrupt Flag in the Processor Status Register by setting the 2nd bit 1. To clear the interrupt disable flag, use CLI.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void SEI(AdressingMode mode, ushort address)
        {
            SF.Interrupt_Disable = true;
        }

        /// <summary>
        /// CLV (Clear Overflow Flag) clears the Overflow Flag in the Processor Status Register by setting the 6th bit 0. There is no opcode for setting the overflow flag.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void CLV(AdressingMode mode, ushort address)
        {
            SF.Overflow = false;
        }

        /// <summary>
        /// CLD (Clear Decimal Flag) clears the Decimal Flag in the Processor Status Register by setting the 3rd bit 0. To set the decimal flag, use SED. Even though the NES doesn't use decimal mode, the opcodes to clear and set the flag do work, so if you need to store a bit, this acts as a free space.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void CLD(AdressingMode mode, ushort address)
        {
            SF.Decimal = false;
        }

        /// <summary>
        /// SED (Set Decimal Flag) set the Decimal Flag in the Processor Status Register by setting the 3rd bit 1. To clear the decimal flag, use CLD. Even though the NES doesn't use decimal mode, the opcodes to clear and set the flag do work, so if you need to store a bit, this acts as a free space.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void SED(AdressingMode mode, ushort address)
        {
            SF.Decimal = true;
        }

        #endregion

        #region Storage
      
        /// <summary>
        /// STA (Store Accumulator In Memory) stores the accumulator into a specified memory address. It is probably the second most-used opcode in 6502 assembly as it stores the most-used register. It is similar in function to STX and STY.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void STA(AdressingMode mode, ushort address)
        {
            WriteByte(address, A);
        }

        /// <summary>
        /// STY (Store Y Index In Memory) stores the Y index into a specified memory address. It is similar in function to STA and STY.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void STY(AdressingMode mode, ushort address)
        {
            WriteByte(address, Y);
        }

        /// <summary>
        /// STX (Store X Index In Memory) stores the X index into a specified memory address. It is similar in function to STA and STY.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void STX(AdressingMode mode, ushort address)
        {
            WriteByte(address, X);
        }

        /// <summary>
        /// LDA (Load Accumulator With Memory) loads the accumulator with specified memory. It is probably the most-used opcode in 6502 assembly as it loads the most-used register. It is similar in function to LDX and LDY.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void LDA(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            Set_Negative_and_Zero(Operand);
            A = Operand;
        }

        /// <summary>
        /// LDY (Load Y Index With Memory) loads the Y Index Register with the specified memory. It is similar in function to LDA and LDX opcodes. Unlike LDA, there aren't as many addressing modes available for LDY.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void LDY(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            Set_Negative_and_Zero(Operand);
            Y = Operand;
        }

        /// <summary>
        /// LDX (Load X Index With Memory) loads the X Index Register with the specified memory. It is similar in function to LDA and LDY opcodes. Unlike LDA, there aren't as many addressing modes available for LDX.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void LDX(AdressingMode mode, ushort address)
        {
            byte Operand = ReadByte(address);
            Set_Negative_and_Zero(Operand);
            X = Operand;
        }

        /// <summary>
        /// TYA (Transfer Y Index to Accumulator) transfers the value in the Y index to the accumulator. The reverse of this opcode is TAY.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void TYA(AdressingMode mode, ushort address)
        {
            A = Y;
            Set_Negative_and_Zero(A);
        }

        /// <summary>
        /// TAY (Transfer Accumulator to Y Index) transfers the value in the accumulator to the Y index. The reverse of this opcode is TYA.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void TAY(AdressingMode mode, ushort address)
        {
            Y = A;
            Set_Negative_and_Zero(Y);
        }

        /// <summary>
        /// TXA (Transfer X Index to Accumulator) transfers the value in the X index to the accumulator. The reverse of this opcode is TAX.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void TXA(AdressingMode mode, ushort address)
        {
            A = X;
            Set_Negative_and_Zero(A);
        }

        /// <summary>
        /// TAX (Transfer Accumulator to X Index) transfers the value in the accumulator to the X index. The reverse of this opcode is TXA.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void TAX(AdressingMode mode, ushort address)
        {
            X = A;
            Set_Negative_and_Zero(X);
        }

        /// <summary>
        /// TSX (Transfer Stack Pointer to X Index) transfers the value in the stack pointer to the X index. The reverse of this opcode is TXS.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void TSX(AdressingMode mode, ushort address)
        {
            X = S;
            Set_Negative_and_Zero(X);
        }

        /// <summary>
        /// TXS (Transfer X Index to Stack Pointer) transfers the value in the X index to the stack pointer. The reverse of this opcode is TSX.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void TXS(AdressingMode mode, ushort address)
        {
            S = X;
            Set_Negative_and_Zero(S);
        }

        #endregion

        #region Math

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

        /// <summary>
        /// DEX - Decrement X Register
        /// X,Z,N = X-1
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void DEX(AdressingMode mode, ushort address)
        {
            X--;
            Set_Negative_and_Zero(X);
        }

        /// <summary>
        /// DEY - Decrement Y Register
        /// Y,Z,N = Y-1
        /// Subtracts one from the Y register setting the zero and negative flags as appropriate.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void DEY(AdressingMode mode, ushort address)
        {
            Y--;
            Set_Negative_and_Zero(Y);
        }

        /// <summary>
        /// DEC - Decrement Memory
        /// M,Z,N = M-1
        /// Subtracts one from the value held at a specified memory location setting the zero and negative flags as appropriate.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void DEC(AdressingMode mode, ushort address)
        {
            var tmp = ReadByte(address);
            tmp--;
            Set_Negative_and_Zero(tmp);
            WriteByte(address, tmp);
        }

        /// <summary>
        /// INY - Increment Y Register
        /// Y,Z,N = Y+1
        /// Adds one to the Y register setting the zero and negative flags as appropriate.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void INY(AdressingMode mode, ushort address)
        {
            Y++;
            Set_Negative_and_Zero(Y);
        }

        /// <summary>
        /// INX - Increment X Register
        /// X,Z,N = X+1
        /// Adds one to the X register setting the zero and negative flags as appropriate.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void INX(AdressingMode mode, ushort address)
        {
            X++;
            Set_Negative_and_Zero(X);
        }

        /// <summary>
        /// INC - Increment Memory
        /// M,Z,N = M+1
        /// Adds one to the value held at a specified memory location setting the zero and negative flags as appropriate.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="address"></param>
        public void INC(AdressingMode mode, ushort address)
        {
            var tmp = ReadByte(address);
            tmp++;
            Set_Negative_and_Zero(tmp);
            WriteByte(address, tmp);
        }

        #endregion

        #region Illegal OPCodes

        public void ___(AdressingMode mode, ushort address)
        {
            return;
        }

        #endregion

        #endregion
    }
}
