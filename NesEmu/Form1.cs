using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NesEmu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                NesEmu.Core.Cartridge cartridge = new Core.Cartridge("asd");
            }
            catch(System.FormatException errormsg)
            {
                MessageBox.Show(errormsg.Message, "Error occured while loadin the ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                NesEmu.Core.Cartridge cartridge = new Core.Cartridge("C:\\Users\\MatePC\\source\\repos\\SMB3Corrupt.nes");
            }
            catch (System.FormatException errormsg)
            {
                MessageBox.Show(errormsg.Message, "Error occured while loadin the ROM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
