using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesEmu.Core
{

    public class Mindendes { };


    sealed partial class CPU : Mindendes
    {
        private Emulator _emu;

        #region Events

        public event EventHandler UIChanged;

        private void OnUIChanged()
        {
            if(UIChanged !=null)
                UIChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Constructors

        public CPU()//Test
        {
            //FOR TESTING ONLY
            SetInstructions();
            //Reset();
        }

        public CPU(Emulator emu, EventHandler eh=null)
        {
            if(eh!=null)
            {
                UIChanged += eh;
            }
            SetInstructions();
            _emu = emu;
            Reset();
        }

        #endregion

        public int Step()
        {
            //handle irq interrupt

            //handle nmi interrupt

            int startcycle = _cycle;

            var currentOpCode = ReadByte(PC);

            AdressingMode currentAddressMode = GetAdressingMode(currentOpCode);

            ushort currentAddress = 0;
            bool pageCrossed = false;
            ushort oldAddress = 0;//for page checking

            //http://obelisk.me.uk/6502/addressing.html
            switch (currentAddressMode)
            {
                //No operands
                case AdressingMode.Implied:
                    break;

                //Operand is a number(byte) right after the PC
                //LDA #10 - Load 10 ($0A) into the accumulator
                case AdressingMode.Immediate:
                    currentAddress = (ushort)(PC + 1);
                    break;

                //Operand is a 16bit full address
                //JMP $1234 -Jump to location $1234
                case AdressingMode.Absolute:
                    currentAddress = Read16((ushort)(PC + 1));
                    break;

                //Operand is 16bit + 8bit from X
                //STA $3000,X     ;Store accumulator between $3000 and $30FF
                case AdressingMode.AbsoluteX:
                    oldAddress = Read16((ushort)(PC + 1));
                    pageCrossed = IsPageCross(oldAddress, X);
                    currentAddress = (ushort)(oldAddress + X); 
                    break;

                //Operand is 16bit + 8bit from X
                //AND $4000,Y     ;Perform a logical AND with a $40??
                case AdressingMode.AbsoluteY:
                    oldAddress = Read16((ushort)(PC + 1));
                    pageCrossed = IsPageCross(oldAddress, Y);
                    currentAddress = (ushort)(oldAddress + Y);
                    break;

                //No operands, woks on the Accumulator
                case AdressingMode.Accumulator:
                    break;

                //Adds read byte +2 to PC
                //Next read instruction is relative to PC position
                case AdressingMode.Relative:
                    currentAddress = (ushort)(PC + (sbyte)ReadByte((ushort)(PC + 1)) + 2);
                    break;

                //8bit from 0x00-0xFF
                //LDA $00         ;Load accumulator from $00
                case AdressingMode.ZeroPage:
                    currentAddress = ReadByte((ushort)(PC + 1));
                    break;

                //8bit from PC+1, added to Y
                //LDX $10,Y       ;Load the X register from a location on zero page
                case AdressingMode.ZeroPageY:
                    currentAddress = (ushort)((ReadByte((ushort)(PC + 1)) + Y) & 0xFF);
                    break;

                //8bit from PC+1, added to X
                //STY $10,X       ;Save the Y register at location on zero page
                case AdressingMode.ZeroPageX:
                    currentAddress = (ushort)((ReadByte((ushort)(PC + 1)) + X) & 0xFF);
                    break;

                //16bit points to LO, and reads HI after with wrap-around
                //JMP ($FFFC)     ;Force a power on reset
                case AdressingMode.Indirect:
                    // Must wrap if at the end of a page to emulate a 6502 bug present in the JMP instruction
                    currentAddress = (ushort)Read16WrapPageAround((ushort)Read16((ushort)(PC + 1)));
                    break;

                //Indexed indirect addressing is normally used in conjunction with a table of address held on zero page. The address of the table is taken from the instruction and the X register added to it (with zero page wrap around) to give the location of the least significant byte of the target address.
                case AdressingMode.IndexedIndirect:
                    // Zeropage address of lower nibble of target address (& 0xFF to wrap at 255)
                    ushort loAddress = (ushort)((ReadByte((ushort)(PC + 1)) + X) & 0xFF);

                    // Target address (Must wrap to 0x00 if at 0xFF)
                    currentAddress = (ushort)Read16WrapPageAround((ushort)(loAddress));
                    break;

                //Indirect indirect addressing is the most common indirection mode used on the 6502. In instruction contains the zero page location of the least significant byte of 16 bit address. The Y register is dynamically added to this value to generated the actual target address for operation.
                case AdressingMode.IndirectIndexed:
                    // Zeropage address of the value to add the Y register to to get the target address
                    ushort valueAddress = (ushort)ReadByte((ushort)(PC + 1));

                    // Target address (Must wrap to 0x00 if at 0xFF)
                    oldAddress = Read16WrapPageAround(valueAddress);
                    pageCrossed = IsPageCross((ushort)(oldAddress + Y), Y);
                    currentAddress = (ushort)(oldAddress + Y);
                    break;
            }

            PC += (ushort)_instructionSizes[currentOpCode];
            _cycle += _instructionCycles[currentOpCode];
            if (pageCrossed)
            {
                _cycle += _instructionPageCycles[currentOpCode];
            }
            ExecuteOpCode(currentOpCode, currentAddressMode, currentAddress);

            OnUIChanged();

            return _cycle;
        }

        public void Reset()
        {
            //https://wiki.nesdev.com/w/index.php/CPU_power_up_state#cite_note-1

            P = 0x24; //Set flags (Originally P = $34)
            A = X = Y = 0;
            S = 0xFD; //Stack pointer
            PC = Read16(0xFFFC);

            _cycle = 0;

            OnUIChanged();
        }
    }
}
