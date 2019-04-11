using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NesEmu.Core;

namespace NesEmu
{
    public partial class Form1 : Form
    {
        public Emulator _emulator;
        public Form1()
        {
            InitializeComponent();

            //_emulator = new Emulator();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _emulator = new Emulator("error");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _emulator = new Emulator("C:\\Users\\MatePC\\source\\repos\\SMB3Corrupt.nes");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _emulator = new Emulator("C:\\Users\\MatePC\\source\\repos\\SMB3.nes");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _emulator._cpu.ExecuteOpCode(0);
            _emulator._cpu.ExecuteOpCode(1);
            _emulator._cpu.ExecuteOpCode(2);
        }

        private void Donkey_Click(object sender, EventArgs e)
        {
            _emulator = new Emulator();
            button5_Click(sender,e);
        }

        /*private void listBox1_Format(object sender, ListControlConvertEventArgs e)
        {
            int i = 0;
            if (int.TryParse(e.Value.ToString(), out i))
                e.Value = string.Format("0x{0}", i.ToString("X2"));
        }*/

        private void button5_Click(object sender, EventArgs e)
        {
            if(_emulator != null)
            {

                var prCode = _emulator._cartridge._PRGROM;
                var prLength = prCode.Length;
                listView1.BeginUpdate();

                string[] tmp = new string[prLength];
                for( int i= 0; i < prLength; i++)
                {
                    var hexvalue = "0x" + prCode[i].ToString("X2");
                    var hexaddress = "  $" + i.ToString("X2");
                    var opName = _emulator._cpu.GetOpCodeName(prCode[i]).ToString();
                    tmp[i] = hexvalue + hexaddress;
                    
                    listView1.Items.Add(new ListViewItem(new string[] { hexaddress, hexvalue, opName }));
                }
                listView1.EndUpdate();
                listView1.GridLines = true;

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_Format(object sender, ListControlConvertEventArgs e)
        {

        }
    }
}
