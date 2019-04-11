using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesEmu.Core
{

    public class Mindendes { };

    public delegate void RefreshUI();

    sealed partial class CPU : Mindendes
    {
        private Emulator _emu;

        #region Events



        public event EventHandler UIChanged;

        private void OnUIChanged()
        {
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

        public CPU(Emulator emu)
        {
            SetInstructions();
            _emu = emu;
            Reset();
        }

        #endregion

        public void Step()
        {
            var currentOpCode = ReadByte(PC);
            ExecuteOpCode(currentOpCode);

            OnUIChanged();
        }

        public void Reset()
        {
            //https://wiki.nesdev.com/w/index.php/CPU_power_up_state#cite_note-1

            P = 0x24; //Set flags (Originally P = $34)
            A = X = Y = 0;
            S = 0xFD; //Stack pointer
            PC = Read16(0xFFFC);

            _cycle = 0;
        }
    }
}
