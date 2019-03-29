using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesEmu.Core
{
    public partial class CPU
    {
        //https://wiki.nesdev.com/w/index.php/CPU_memory_map

        private readonly byte[] _ram = new byte[0x800]; //$0000-$07FF	Size:$0800	2KB internal RAM


        private void WriteByte(ushort v, byte value)
        {
            throw new NotImplementedException();
        }

        #region Stack Operations

        public void PushByte(byte value)
        {
            S++;
            WriteByte((ushort)(S + 0b1_0000_0000), value); //Stack pointer is 8bit
        }

        public void Push16(ushort value)
        {//unittest
            byte low  = (byte)(value &= 0b1111_1111);
            byte high = (byte)((value &= 0b1111_1111_0000_0000) >> 8);

            PushByte(high);
            PushByte(low);
        }

        public byte Pop()
        {
            return 0;
        }

        #endregion
    }
}
