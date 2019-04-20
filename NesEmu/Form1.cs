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

        #region Button Clicks

        private void Open_rom_button_Click(object sender, EventArgs e)
        {
            openRomFileDialog.ShowDialog(); // Show the dialog.
        }

        private void Cpu_Step_button_Click(object sender, EventArgs e)
        {
            if (_emulator== null || _emulator._cpu==null)
                return;
            _emulator._cpu.Step();
        }

        private void Cpu_loop_button_Click(object sender, EventArgs e)
        {
            if (_emulator != null)
            {
                _emulator.CPU_LoopStep();
            }
        }

        private void Ram_refresh_button_Click(object sender, EventArgs e)
        {
            if (_emulator == null || _emulator._cpu == null)
                return;
            LoadListView(Ram_listView, ListType.RAM, _emulator._cpu.GetRam());
        }

        private void Stack_refresh_button_Click(object sender, EventArgs e)
        {
            if (_emulator == null || _emulator._cpu == null)
                return;
            LoadListView(Stack_listView3, ListType.STACK, _emulator._cpu.GetRam());
        }

        #endregion

        #region EventHandlers

        private void _cpu_UIChanged(object sender, EventArgs e)
        {
            if (sender is Core.CPU)
            {
                Core.CPU tmp = (Core.CPU)sender;
                Cpu_Log_listView2.BeginUpdate();
                Cpu_Log_listView2.Items.Insert(0, new ListViewItem(
                    new string[] {
                        tmp.X.ToString("X2"),
                        tmp.Y.ToString("X2"),
                        tmp.A.ToString("X2"),
                        tmp.S.ToString("X2"),
                        tmp.PC.ToString("X2"),
                        Convert.ToString( tmp.P, 2),
                    }));
                Cpu_Log_listView2.EndUpdate();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ListViews


        private enum ListType { PRGROM = 1, RAM, STACK};

        private void LoadListView(ListView listView, ListType listType, byte[] input)
        {
            byte[] InputArray = input;

            int StartIndex = 0;
            int OutputLength = 0;

            if (listType == ListType.RAM)
                OutputLength = 0x800;
            else if (listType == ListType.STACK)
            {
                StartIndex = 0x100;
                OutputLength = 0x100;
            }
            else
                OutputLength = InputArray.Length;

            listView.BeginUpdate();
            listView.Items.Clear();

            for (int i = 0; i < OutputLength; i++)
            {
                var hexvalue = "0x" + InputArray[i+ StartIndex].ToString("X2");
                var hexaddress = "  $" + (i + StartIndex).ToString("X2");

                if(listType==ListType.PRGROM)
                {
                    string opName= "";
                    if (_emulator != null && _emulator._cpu != null)
                        opName = _emulator._cpu.GetOpCodeName(InputArray[i + StartIndex]).ToString();
                    listView.Items.Add(new ListViewItem(new string[] { hexaddress, hexvalue, opName }));
                }
                else if(listType == ListType.RAM)
                {
                    listView.Items.Add(new ListViewItem(new string[] { hexaddress, hexvalue }));
                }
                else if (listType == ListType.STACK)
                {
                    listView.Items.Insert(0, new ListViewItem(new string[] { hexaddress, hexvalue }));
                }
            }
            listView.EndUpdate();
            listView.GridLines = true;
        }

        private void ClearListViews()
        {
            Cpu_Log_listView2.Items.Clear();
            Ram_listView.Items.Clear();
            Stack_listView3.Items.Clear();
            PRG_listView1.Items.Clear();
        }

        private void LoadListViews()
        {
            if (_emulator == null || _emulator._cpu == null || _emulator._cartridge == null || _emulator._cartridge._PRGROM == null)
                return;

            LoadListView(PRG_listView1, ListType.PRGROM, _emulator._cartridge._PRGROM);
            LoadListView(Ram_listView, ListType.RAM, _emulator._cpu.GetRam());
            LoadListView(Stack_listView3, ListType.STACK, _emulator._cpu.GetRam());
        }


        #endregion

        #region Timers

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        #endregion

        #region File Loading

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (sender is FileDialog FD)
            {
                if (FD.CheckFileExists)
                {
                    ClearListViews();

                    _emulator = new Emulator(File_path: FD.FileName, eh: _cpu_UIChanged);

                    LoadListViews();
                }
            }
        }

        #endregion

    }
}
