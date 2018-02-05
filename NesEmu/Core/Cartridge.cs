using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesEmu.Core
{
    public class Cartridge
    {
        #region Enum

        /// <summary>
        /// Mirroring Mode
        /// </summary>
        public enum MirroringMode
        {
            Horizontal, Vertical, All, Upper, Lower
        }

        #endregion

        #region Properties

        /// <summary>
        /// Raw Rom Data
        /// </summary>
        public readonly byte[] _raw_ROM;

        /// <summary>
        /// Program Rom Size
        /// </summary>
        public readonly int _PRGROMSize;
        /// <summary>
        /// Character Rom Size
        /// </summary>
        public readonly int _CHRROMSize;
        /// <summary>
        /// Program Ram Size
        /// </summary>
        public readonly int _PRGRAMSize;
        /// <summary>
        /// Program Rom Offset
        /// </summary>
        public readonly int _PRGROMOffset;

        /// <summary>
        /// Program Rom
        /// </summary>
        public readonly byte[] _PRGROM;
        /// <summary>
        /// Character Rom
        /// </summary>
        public readonly byte[] _CHRROM;

        /// <summary>
        /// Mirroring mode of the ROM
        /// </summary>
        public MirroringMode _mirroringMode;

        /// <summary>
        /// Mapping Number of the ROM
        /// </summary>
        public readonly int _MapperNumber;

        #endregion

        #region Constructor

        /// <summary>
        /// Reads a .nes file into a Cartridge
        /// </summary>
        /// <param name="file_path">Path to the .nes file</param>
        public Cartridge(string file_path)
        {
            try
            {
                _raw_ROM = System.IO.File.ReadAllBytes(file_path);
            }
            catch(System.IO.FileNotFoundException)
            {
                throw new FormatException("File not found");
            }

            int rom_header = BitConverter.ToInt32(_raw_ROM, 0);
            if (rom_header != 0x1A53454E) // "NES<EOF>"
            {
                throw new FormatException("Unexpected header value: " + rom_header.ToString("X") + "\nExpected : 4D 41 54 10");
            }

            _PRGROMSize = _raw_ROM[4] * 0x4000; // 16kb units
            _CHRROMSize = _raw_ROM[5] * 0x2000; // 8kb units
            _PRGRAMSize = _raw_ROM[8] * 0x2000; // 8kb units

            _mirroringMode = (_raw_ROM[6] & 0x1) > 0 ? MirroringMode.Vertical : MirroringMode.Horizontal; // Bit 0
            if ((_raw_ROM[6] & 0x8) > 0) _mirroringMode = MirroringMode.All; // Overrides Bit 0

            _MapperNumber = (_raw_ROM[6] >> 4) | (_raw_ROM[7] & 0xF0);

            bool hasTrainer = (_raw_ROM[6] & 0b100) > 0;
            _PRGROMOffset = 16 + (hasTrainer ? 512 : 0);

            _PRGROM = new byte[_PRGROMSize];
            Array.Copy(_raw_ROM, _PRGROMOffset, _PRGROM, 0, _PRGROMSize);

            if (_CHRROMSize == 0)
                _CHRROM = new byte[0x2000];
            else
            {
                _CHRROM = new byte[_CHRROMSize];
                Array.Copy(_raw_ROM, _PRGROMOffset + _PRGROMSize, _CHRROM, 0, _CHRROMSize);
            }
        }

        #endregion
    }
}