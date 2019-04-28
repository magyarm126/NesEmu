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

        public void OnUIChanged()
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

            _ = _cycle;

            currentOpCode = ReadByte(PC);

            AdressingMode currentAddressMode = GetAdressingMode(currentOpCode);

            ushort currentAddress = 0;
            bool pageCrossed = false;

            //deb
            lastOpCode = currentOpCode;
            OnUIChanged();

            AddressResolver(ref currentAddressMode, ref currentAddress, ref pageCrossed);

            PC += (ushort)_instructionSizes[currentOpCode];
            _cycle += _instructionCycles[currentOpCode];
            if (pageCrossed)
            {
                _cycle += _instructionPageCycles[currentOpCode];
            }

            ExecuteOpCode(currentOpCode, currentAddressMode, currentAddress);

            return _cycle;
        }

        public void Reset()
        {
            //https://wiki.nesdev.com/w/index.php/CPU_power_up_state#cite_note-1

            P = 0x24; //Set flags (Originally P = $34)
            A = X = Y = 0;
            S = 0xFD; //Stack pointer
            //PC = Read16(0xFFFC);
            _cycle = 0;



            //debug
            PC = 0xC000;
            _cycle = 7;
            //C000  4C F5 C5 JMP $C5F5 A:00 X: 00 Y: 00 P: 24 SP: FD PPU:  0,  0 CYC: 7
            //http://www.qmtpro.com/~nes/misc/nestest.log


            //OnUIChanged();
        }
    }
}
