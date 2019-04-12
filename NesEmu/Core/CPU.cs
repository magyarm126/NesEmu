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

            switch(currentAddressMode)
            {
                case AdressingMode.Implied:
                    break;
                case AdressingMode.Immediate:
                    currentAddress = (ushort)(PC + 1);
                    break;
                case AdressingMode.Absolute:
                    currentAddress = Read16((ushort)(PC + 1));
                    break;
                case AdressingMode.AbsoluteX:
                    currentAddress = (ushort)(Read16((ushort)(PC + 1)) + X);
                    pageCrossed = IsPageCross((ushort)(currentAddress - X), (ushort)X);
                    break;
                case AdressingMode.AbsoluteY:
                    currentAddress = (ushort)(Read16((ushort)(PC + 1)) + Y);
                    pageCrossed = IsPageCross((ushort)(currentAddress - Y), (ushort)Y);
                    break;
                case AdressingMode.Accumulator:
                    break;
                case AdressingMode.Relative:
                    currentAddress = (ushort)(PC + (sbyte)ReadByte((ushort)(PC + 1)) + 2);
                    break;
                case AdressingMode.ZeroPage:
                    currentAddress = ReadByte((ushort)(PC + 1));
                    break;
                case AdressingMode.ZeroPageY:
                    currentAddress = (ushort)((ReadByte((ushort)(PC + 1)) + Y) & 0xFF);
                    break;
                case AdressingMode.ZeroPageX:
                    currentAddress = (ushort)((ReadByte((ushort)(PC + 1)) + X) & 0xFF);
                    break;
                case AdressingMode.Indirect:
                    // Must wrap if at the end of a page to emulate a 6502 bug present in the JMP instruction
                    currentAddress = (ushort)_memory.Read16WrapPage((ushort)Read16((ushort)(PC + 1)));
                    break;
                case AdressingMode.IndexedIndirect:
                    // Zeropage address of lower nibble of target address (& 0xFF to wrap at 255)
                    ushort lowerNibbleAddress = (ushort)((ReadByte((ushort)(PC + 1)) + X) & 0xFF);

                    // Target address (Must wrap to 0x00 if at 0xFF)
                    currentAddress = (ushort)_memory.Read16WrapPage((ushort)(lowerNibbleAddress));
                    break;
                case AdressingMode.IndirectIndexed:
                    // Zeropage address of the value to add the Y register to to get the target address
                    ushort valueAddress = (ushort)ReadByte((ushort)(PC + 1));

                    // Target address (Must wrap to 0x00 if at 0xFF)
                    currentAddress = (ushort)(_memory.Read16WrapPage(valueAddress) + Y);
                    pageCrossed = IsPageCross((ushort)(currentAddress - Y), currentAddress);
                    break;

                    /*
                     *http://forums.nesdev.com/viewtopic.php?f=3&t=7267
                     *
                     * There's the error on wrap for the JMP Indirect instruction, you ask to do an indirect jmp at ($1FF) and it reads the 16-bit jump address from $1FF and $100 instead of $1FF and $200. 

                        Then there's the extra cycle for when some instructions cross pages when adding X or Y. 

                        Then there's the zeropage instructions which wrap back to the zeropage when you add a X to them instead of advancing to the $100 page.
                    */

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
