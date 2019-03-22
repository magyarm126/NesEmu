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

        private readonly int _cycle; //cycle counter

        #endregion

        #region Public Members

        public void Test() { }

        #endregion

    }
}
