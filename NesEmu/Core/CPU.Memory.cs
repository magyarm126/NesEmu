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

        #region Memory operations

        /// <summary>
        /// Reads byte from internal memory, mappers, ppu, etc based on address location
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private byte ReadByte(ushort address)
        {
            //https://wiki.nesdev.com/w/index.php/CPU_memory_map

            byte ret =0;
            
            if (address >= 0x0 && address <= 0x2000)
            {   
                //0000-2000 internal ram
                address &= 0x800;
                ret = _ram[address];
            }
            else if(address >= 0x2000 && address<=0x2007)
            {
                //PPU register
                int ppuregister = (address - 0x2000);
                //UNIT TEST
                throw new NotImplementedException();
            }
            else if (address >= 0x2008 && address <= 0x3FFF)
            {
                int ppuregister = (address - 0x2000) % 8;
                //UNIT TEST
                throw new NotImplementedException();
            }
            else if (address >= 0x4000 && address <= 0x4017)
            {
                //UNIT TEST
                //NES APU, IO registers
                throw new NotImplementedException();
            }
            else if (address >= 0x4018 && address <= 0x401F)
            {
                //UNIT TEST
                //NES APU, IO registers
                //CPUTEST?
                throw new NotImplementedException();
            }
            else if (address >= 0x4020 && address <= 0xFFFF)
            {
                //Mapper
                ret = _emu._mapper.Read(address);
            }
            else
            {
                throw new NotImplementedException();
            }

            return ret;
        }

        /// <summary>
        /// Writes byte to internal memory, mappers, ppu, etc based on address location
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        private void WriteByte(ushort address, byte value)
        {
            //https://wiki.nesdev.com/w/index.php/CPU_memory_map

            if (address >= 0x0 && address <= 0x2000)
            {
                //0000-2000 internal ram
                address &= 0x800;
                _ram[address]=value;
            }
            else if (address >= 0x2000 && address <= 0x2007)
            {
                //PPU register
                int ppuregister = (address - 0x2000);
                //UNIT TEST
                throw new NotImplementedException();
            }
            else if (address >= 0x2008 && address <= 0x3FFF)
            {
                int ppuregister = (address - 0x2000) % 8;
                //UNIT TEST
                throw new NotImplementedException();
            }
            else if (address >= 0x4000 && address <= 0x4017)
            {
                //UNIT TEST
                //NES APU, IO registers
                throw new NotImplementedException();
            }
            else if (address >= 0x4018 && address <= 0x401F)
            {
                //UNIT TEST
                //NES APU, IO registers
                //CPUTEST?
                throw new NotImplementedException();
            }
            else if (address >= 0x4020 && address <= 0xFFFF)
            {
                //Mapper
                _emu._mapper.Write(address, value);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Reads 2bytes from address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private ushort Read16(ushort address)
        {
            ushort ret = 0;

            byte lo = ReadByte(address);
            byte hi = ReadByte((ushort)(address + 1));
            ret = (ushort)((hi << 8) | lo);

            return ret;
        }

        /// <summary>
        /// Read 16 bits for indirect modes
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private ushort Read16WrapPageAround(ushort address)//Reading the last byte of a page as low byte, means high byte is read from the start of the page instead of reading the first byte of the next page
        {
            ushort ret = 0;
            if((address & 0xFF) == 0xFF) //ends with xx_FF;
            {
                byte lo = ReadByte(address);
                byte hi = ReadByte((ushort)(address & (~0xFF))); //0xFFF..FF_00

                ret = (ushort)((hi << 8) | lo);
            }
            else
                ret = Read16(address);

            return ret;
        }

        public bool IsPageCross(ushort oldadr, byte register)
        {
            ushort newadr = (ushort)(oldadr + (ushort)register);
            return ((newadr & 0xFF00) != (oldadr & 0xFF00));
        }

        #endregion

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
