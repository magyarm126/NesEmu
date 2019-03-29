using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NesEmu.Core;

namespace NesEmu.Mappers
{
    /// <summary>
    /// Mapper 0
    /// The generic designation NROM refers to the Nintendo cartridge boards NES-NROM-128, NES-NROM-256, their HVC counterparts, and clone boards. The iNES format assigns mapper 0 to NROM.
    /// </summary>
    public class NROM : Mapper
    {
        //TBA: VRAM mirroring

        //https://wiki.nesdev.com/w/index.php/NROM
        //https://wiki.nesdev.com/w/index.php/Programming_NROM

        private Cartridge _cartridge;

        public NROM(Emulator emu) : base(emu)
        {
            _cartridge = emu._cartridge;
        }

        public override int MappedAdress(int address)
        {  
            int mappedaddress = address - 0x8000;
            if (_cartridge.PrgRomBanks == 1)
            {
                return (mappedaddress % 0x4000);//16KiB <-- mirror
            }
            else
                return mappedaddress;
        }

        public override byte Read(int address)
        {
            if (address < 0x2000)
            {// CHR rom $0000 - $1FFF
                return _cartridge._CHRROM[address];
            }
            else if (address >= 0x8000)
            {// PRG rom $8000 - 
                return _cartridge._PRGROM[MappedAdress(address)];
            }
            else return 0;
        }

        public override void Write(int address, byte value)
        {
            //todo check for UsesChrRam
            _cartridge._CHRROM[address] = value;
        }
    }
}
