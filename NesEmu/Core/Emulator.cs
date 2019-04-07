using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NesEmu.Core;
using NesEmu.Mappers;

namespace NesEmu.Core
{
    public class Emulator
    {
        public CPU _cpu;
        //public PPU _ppu;
        public Cartridge _cartridge;

        public Mapper _mapper;

        public Emulator(string File_path = "C:\\Users\\MatePC\\source\\repos\\donkey kong.nes")
        {
            try
            {
                _cartridge = new Core.Cartridge(File_path);
                MessageBox.Show("Succes!", "ROM loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                switch (_cartridge._MapperNumber)
                {
                    case 0:
                        _mapper = new NROM(this);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                _cpu = new CPU(this);//cpu needs mapper
            }
            catch (System.FormatException errormsg)
            {
                MessageBox.Show(errormsg.Message, "Error occured while loadin the ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
 
        }
    }
}
