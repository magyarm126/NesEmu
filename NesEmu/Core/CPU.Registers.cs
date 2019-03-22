using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesEmu.Core
{
    public partial class CPU
    {

        //byte	    System.Byte	    1 byte	    0 to 255
        //short     System.Int16    2 bytes     -32,768 to 32,767

        /// <summary>
        /// Accumulator
        /// A is byte-wide and along with the arithmetic logic unit (ALU), supports using the status register for carrying, overflow detection, and so on.
        /// </summary>
        private byte _A;

        /// <summary>
        /// Indexes
        /// X and Y are byte-wide and used for several addressing modes. They can be used as loop counters easily, using INC/DEC and branch instructions. Not being the accumulator, they have limited addressing modes themselves when loading and saving.
        /// </summary>
        private byte _X, _Y;

        /// <summary>
        /// Program Counter
        /// The 2-byte program counter PC supports 65536 direct (unbanked) memory locations, however not all values are sent to the cartridge. It can be accessed either by allowing CPU's internal fetch logic increment the address bus, an interrupt (NMI, Reset, IRQ/BRQ), and using the RTS/JMP/JSR/Branch instructions.
        /// </summary>
        private ushort _PC; //might have to limit set function

        /// <summary>
        /// Stack Pointer
        /// S is byte-wide and can be accessed using interrupts, pulls, pushes, and transfers
        /// </summary>
        private byte _S;

        private StatusFlag _SF = new StatusFlag();
        public StatusFlag SF { get => _SF; set => _SF = value; }

        /// <summary>
        /// Status Register
        /// P has 6 bits used by the ALU but is byte-wide. PHP, PLP, arithmetic, testing, and branch instructions can access this register.
        /// </summary>
        public byte P { get => _SF.P; set => _SF.P = value; }
        public byte S { get => _S; set => _S = value; }
        public ushort PC { get => _PC; set => _PC = value; }
        public byte X { get => _X; set => _X = value; }
        public byte Y { get => _Y; set => _Y = value; }
        public byte A { get => _A; set => _A = value; }

        /// <summary>
        /// The flags register, also called processor status or just P, is one of the six architectural registers on the 6502 family CPU. It is composed of six one-bit registers; instructions modify one or more bits and leave others unchanged.
        /// </summary>
        public class StatusFlag
        {
            public byte P { get; set; }

            //0
            public bool Carry {
                get => (P & 0b1) > 0;
                set {
                    if (value)
                        P |= 0b0000_0001;
                    else
                        P &= 0b1111_1110;
                }
            }

            //1
            public bool Zero
            {
                get => (P & 0b10) > 0;
                set
                {
                    if (value)
                        P |= 0b0000_0010;
                    else
                        P &= 0b1111_1101;
                }
            }

            //2
            public bool Interrupt_Disable
            {
                get => (P & 0b100) > 0;
                set
                {
                    if (value)
                        P |= 0b0000_0100;
                    else
                        P &= 0b1111_1011;
                }
            }

            //3
            public bool Decimal
            {
                get => (P & 0b1000) > 0;
                set
                {
                    if (value)
                        P |= 0b0000_1000;
                    else
                        P &= 0b1111_0111;
                }
            }

            //4
            public bool Bit4
            {
                get => (P & 0b1000_0) > 0;
                set
                {
                    if (value)
                        P |= 0b0001_0000;
                    else
                        P &= 0b1110_1111;
                }
            }

            //5
            public bool Bit5
            {
                get => (P & 0b1000_00) > 0;
                set
                {
                    if (value)
                        P |= 0b0010_0000;
                    else
                        P &= 0b1101_1111;
                }
            }

            //6
            public bool Overflow
            {
                get => (P & 0b1000_000) > 0;
                set
                {
                    if (value)
                        P |= 0b0100_0000;
                    else
                        P &= 0b1011_1111;
                }
            }

            //7
            public bool Negative
            {
                get => (P & 0b1000_0000) > 0;
                set
                {
                    if (value)
                        P |= 0b1000_0000;
                    else
                        P &= 0b0111_1111;
                }
            }
        }


    }
}
