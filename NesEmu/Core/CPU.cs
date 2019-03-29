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

        #region Constructors

        public CPU()//Test
        {
            SetInstructions();
        }

        public CPU(Emulator emu)
        {
            SetInstructions();
            _emu = emu;
        }

        #endregion
    }
}
