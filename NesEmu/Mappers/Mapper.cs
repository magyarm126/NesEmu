using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NesEmu.Core;

namespace NesEmu.Mappers
{
    public abstract class Mapper
    {
        protected Emulator _emulator;

        public Mapper(Emulator emu)
        {
            _emulator = emu;
        }

        public abstract int MappedAdress(int address);

        public abstract byte Read(int address);

        public abstract void Write(int address, byte value);
    }
}
