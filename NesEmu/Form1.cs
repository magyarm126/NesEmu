using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NesEmu.Core;

namespace NesEmu
{
    public partial class DebugWindow : Form
    {
        public Emulator _emulator;
        public DebugWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _emulator = new Emulator("error");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(); // Show the dialog.
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

        private void PushPopTestButton(object sender, EventArgs e)
        {
            if(_emulator != null && _emulator._cpu!= null)
            {
                _emulator._cpu.PushByte(0x8F);
                _emulator._cpu.Push16(0xC2DD);
                _emulator._cpu.Pop16();
                _emulator._cpu.PopByte();
            }
        }

        private void LoadPRG()
        {
            if(_emulator != null &&_emulator._cartridge!= null && _emulator._cartridge._PRGROM!= null && _emulator._cpu!=null)
            {

                var prCode = _emulator._cartridge._PRGROM;
                var prLength = prCode.Length;
                PRG_listView1.BeginUpdate();

                string[] tmp = new string[prLength];
                for( int i= 0; i < prLength; i++)
                {
                    var hexvalue = "0x" + prCode[i].ToString("X2");
                    var hexaddress = "  $" + i.ToString("X2");
                    var opName = _emulator._cpu.GetOpCodeName(prCode[i]).ToString();
                    tmp[i] = hexvalue + hexaddress;
                    
                    PRG_listView1.Items.Add(new ListViewItem(new string[] { hexaddress, hexvalue, opName }));
                }
                PRG_listView1.EndUpdate();
                PRG_listView1.GridLines = true;
            }
            else
            {
                throw new Exception("Not cool");
            }
        }

        private void LoadStack(CPU cpu)
        {
            if (cpu != null)
            {

                byte[] prCode = cpu.GetRam();
                var prLength = 0x100;
                Stack_listView3.BeginUpdate();

                Stack_listView3.Items.Clear();
                string[] tmp = new string[prLength];
                for (int i = 0; i < prLength; i++)
                {
                    var hexvalue = "0x" + prCode[i+0x100].ToString("X2");
                    var hexaddress = "  $" + (i + 0x100).ToString("X2");
                    tmp[i] = hexvalue + hexaddress;

                    Stack_listView3.Items.Insert(0, new ListViewItem(new string[] { hexaddress, hexvalue }));
                }
                Stack_listView3.EndUpdate();
                Stack_listView3.GridLines = true;
            }
            else
            {
                throw new Exception("Not cool");
            }
        }

        private void LoadRam(CPU cpu)
        {
            if (cpu != null)
            {

                byte[] prCode = cpu.GetRam();
                var prLength = 0x800;
                Ram_listView.BeginUpdate();

                Ram_listView.Items.Clear();
                string[] tmp = new string[prLength];
                for (int i = 0; i < prLength; i++)
                {
                    var hexvalue = "0x" + prCode[i].ToString("X2");
                    var hexaddress = "  $" + (i).ToString("X2");
                    tmp[i] = hexvalue + hexaddress;

                    Ram_listView.Items.Add(new ListViewItem(new string[] { hexaddress, hexvalue }));
                }
                Ram_listView.EndUpdate();
                Ram_listView.GridLines = true;
            }
            else
            {
                throw new Exception("Not cool");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(_emulator!=null)
            {
                _emulator.CPU_LoopStep();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (_emulator == null || _emulator._cpu == null)
                return;
            LoadStack(_emulator._cpu);
        }

        private void Ram_refresh_button_Click(object sender, EventArgs e)
        {
            if (_emulator == null || _emulator._cpu == null)
                return;
            LoadRam(_emulator._cpu);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (sender is FileDialog FD)
            {
                if (FD.CheckFileExists)
                {
                    _emulator = new Emulator(File_path: FD.FileName, eh: _cpu_UIChanged);
                    LoadPRG();
                }
                    
            }
        }
    }
}
