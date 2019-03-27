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

        #region Private Members

        private readonly byte[] _ram = new byte[0x800]; //$0000-$07FF	Size:$0800	2KB internal RAM

        private int _cycle; //cycle counter

        private Emulator _emu;

       

        #endregion

        #region Public Members

        public void Test() { }

        #endregion

        #region Constructors

        public CPU(Emulator emu)
        {
            _emu = emu;
        }

        public CPU(byte A, byte X, byte Y, ushort PC, byte S, StatusFlag SF, StatusFlag sF, byte p, byte s, ushort pC, byte x, byte y, byte a, byte[] ram, int cycle)
        {
            _A = A;
            _X = X;
            _Y = Y;
            _PC = PC;
            _S = S;
            _SF = SF ?? throw new ArgumentNullException(nameof(SF));
            this.SF = sF ?? throw new ArgumentNullException(nameof(sF));
            P = p;
            this.S = s;
            this.PC = pC;
            this.X = x;
            this.Y = y;
            this.A = a;
            _ram = ram ?? throw new ArgumentNullException(nameof(ram));
            _cycle = cycle;
        }

        #endregion
    }
}
