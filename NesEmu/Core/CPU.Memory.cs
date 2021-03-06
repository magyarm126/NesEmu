﻿using System;
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
                address %= 0x800;
                ret = _ram[address];
            }
            else if(address >= 0x2000 && address<=0x2007)
            {
                //PPU register
                int ppuregister = (address - 0x2000);
                //UNIT TEST
                //throw new NotImplementedException();
            }
            else if (address >= 0x2008 && address <= 0x3FFF)
            {
                int ppuregister = (address - 0x2000) % 8;
                //UNIT TEST
                //throw new NotImplementedException();
            }
            else if (address >= 0x4000 && address <= 0x4017)
            {
                //UNIT TEST
                //NES APU, IO registers
                //throw new NotImplementedException();
            }
            else if (address >= 0x4018 && address <= 0x401F)
            {
                //UNIT TEST
                //NES APU, IO registers
                //CPUTEST?
               // throw new NotImplementedException();
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
                address %= 0x800;
                _ram[address]=value;
            }
            else if (address >= 0x2000 && address <= 0x2007)
            {
                //PPU register
                int ppuregister = (address - 0x2000);
                //UNIT TEST
                //throw new NotImplementedException();
            }
            else if (address >= 0x2008 && address <= 0x3FFF)
            {
                int ppuregister = (address - 0x2000) % 8;
                //UNIT TEST
                //throw new NotImplementedException();
            }
            else if (address >= 0x4000 && address <= 0x4017)
            {
                //UNIT TEST
                //NES APU, IO registers
                //throw new NotImplementedException();
            }
            else if (address >= 0x4018 && address <= 0x401F)
            {
                //UNIT TEST
                //NES APU, IO registers
                //CPUTEST?
                //throw new NotImplementedException();
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

        public bool IsPageCross(ushort oldadr, ushort/*byte register*/newadr)
        {
           /* ushort newadr = (ushort)(oldadr + (ushort)register);
            return ((newadr & 0xFF00) != (oldadr & 0xFF00));*/

            return ((newadr & 0xFF00) != (oldadr & 0xFF00));
        }

        public byte[] GetRam()//This might be slow
        {
            byte[] copy = new byte[0x800];

            Array.Copy(_ram, copy, _ram.Length);

            return copy;
        }

        #endregion

        #region Stack Operations

        //Descending empty stack
        //Reset -> SP = $FD

        //  |----| $1FF
        //  |----|
        //  |----|  <--(Pop)
        //  |    |<-Stack pointer   <<(Push)
        //  |    |
        //  |    |
        //  |    |
        //  |    | $100

        /// <summary>
        /// Push 8 bits to stack
        /// </summary>
        /// <param name="value"></param>
        public void PushByte(byte value)
        {
            WriteByte((ushort)(S + 0b1_0000_0000), value); //Stack pointer is 8bit
            S--;
        }

        /// <summary>
        /// Pops 8bits from stack
        /// </summary>
        /// <returns></returns>
        public byte PopByte()
        {
            S++;
            byte ret = ReadByte((ushort)(S + 0b1_0000_0000)); //Stack pointer is 8bit

            return ret;
        }

        /// <summary>
        /// Push 16bits to stack
        /// </summary>
        /// <param name="value"></param>
        public void Push16(ushort value)
        {
            byte low  = (byte)(value & 0b1111_1111);
            byte high = (byte)((value & 0b1111_1111_0000_0000) >> 8);

            PushByte(high);
            PushByte(low);
        }

        /// <summary>
        /// Pop 16bits from stack
        /// </summary>
        /// <returns></returns>
        public ushort Pop16()
        {
            ushort ret = 0;

            byte lo = PopByte();
            byte hi = PopByte();

            ret = (ushort)((hi << 8) | lo);

            return ret;
        }

        

        #endregion
    }
}
