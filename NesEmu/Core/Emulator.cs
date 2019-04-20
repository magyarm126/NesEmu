using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NesEmu.Core;
using NesEmu.Mappers;

using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace NesEmu.Core
{
    public class Emulator
    {
        public CPU _cpu;
        //public PPU _ppu;
        public Cartridge _cartridge;

        public Mapper _mapper;

        public void CPU_LoopStep(int times = 3000)
        {
            for (int i = 0; i < times; i++)
            {
                _cpu.Step();
            }
        }


    public Emulator(string File_path = "C:\\Users\\MatePC\\source\\repos\\donkey kong.nes", EventHandler eh = null)
        {
            try
            {
                _cartridge = new Cartridge(File_path);
                //MessageBox.Show("Succes!", "ROM loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                switch (_cartridge._MapperNumber)
                {
                    case 0:
                        _mapper = new NROM(this);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                
                if (eh != null)
                {
                    _cpu = new CPU(this,eh:eh);//cpu needs mapper
                }
                else
                {
                    _cpu = new CPU(this);//cpu needs mapper
                }
                    
            }
            catch (FormatException errormsg)
            {
                MessageBox.Show(errormsg.Message, "Error occured while loadin the ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (NotImplementedException)
            {
                MessageBox.Show("Unsupported Mapper number:" + _cartridge._MapperNumber, "Error occured while loadin the ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
