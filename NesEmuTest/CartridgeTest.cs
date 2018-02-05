using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NesEmu.Core;

namespace NesEmuTest
{
    [TestClass]
    public class CartridgeTest
    {
        Cartridge _cartridge;

        [TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        public void Load_FileNotFound()
        {
            try
            {
                _cartridge = new Cartridge("sampletext");
            }
            catch(System.FormatException exception)
            {
                Assert.AreEqual("File not found", exception.Message);
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        public void Load_FileCorrupt()
        {
            try
            {
                _cartridge = new Cartridge("C:\\Users\\MatePC\\source\\repos\\SMB3Corrupt.nes");
            }
            catch (System.FormatException)
            {
            }
        }


    [TestMethod]
        public void Load_Valid()
        {
            _cartridge = new Cartridge("C:\\Users\\MatePC\\source\\repos\\SMB3.nes");
        }
    }
}
