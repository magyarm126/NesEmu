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
            if (_emulator== null || _emulator._cpu==null)
                return;
            _emulator._cpu.Step();
        }

        private void Donkey_Click(object sender, EventArgs e)
        {
            _emulator = new Emulator(eh:_cpu_UIChanged);
           // _emulator._cpu.UIChanged += _cpu_UIChanged;
            if (_emulator != null && _emulator._cpu != null)
            {
           //     _cpu_UIChanged(_emulator._cpu, EventArgs.Empty); // show init
            }
            LoadPRG();
        }

        private void _cpu_UIChanged(object sender, EventArgs e)
        {
            if(sender is Core.CPU)
            {
                Core.CPU tmp = (Core.CPU)sender;
                listView2.BeginUpdate();
                listView2.Items.Insert(0, new ListViewItem(
                    new string[] {
                        tmp.X.ToString("X2"),
                        tmp.Y.ToString("X2"),
                        tmp.A.ToString("X2"),
                        tmp.S.ToString("X2"),
                        tmp.PC.ToString("X2"),
                        Convert.ToString( tmp.P, 2),
                    }));
                listView2.EndUpdate();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadPRG();
        }

        private void LoadPRG()
        {
            if(_emulator != null &&_emulator._cartridge!= null && _emulator._cartridge._PRGROM!= null && _emulator._cpu!=null)
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
            else
            {
                throw new Exception("Not cool");
            }
        }
    }
}
