using System;

namespace NesEmu
{
    partial class DebugWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Open_rom_button = new System.Windows.Forms.Button();
            this.Cpu_Step_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Timer_start_stop_button = new System.Windows.Forms.Button();
            this.Ram_refresh_button = new System.Windows.Forms.Button();
            this.Stack_refresh_button = new System.Windows.Forms.Button();
            this.PRG_listView1 = new System.Windows.Forms.ListView();
            this.address = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.opcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Cpu_Log_listView2 = new System.Windows.Forms.ListView();
            this.X = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Y = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.A = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.S = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.P = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Stack_listView3 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Ram_listView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openRomFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // Open_rom_button
            // 
            this.Open_rom_button.Location = new System.Drawing.Point(12, 19);
            this.Open_rom_button.Name = "Open_rom_button";
            this.Open_rom_button.Size = new System.Drawing.Size(138, 23);
            this.Open_rom_button.TabIndex = 1;
            this.Open_rom_button.Text = "Open ROM file";
            this.Open_rom_button.UseVisualStyleBackColor = true;
            this.Open_rom_button.Click += new System.EventHandler(this.Open_rom_button_Click);
            // 
            // Cpu_Step_Button
            // 
            this.Cpu_Step_Button.Location = new System.Drawing.Point(6, 19);
            this.Cpu_Step_Button.Name = "Cpu_Step_Button";
            this.Cpu_Step_Button.Size = new System.Drawing.Size(109, 23);
            this.Cpu_Step_Button.TabIndex = 3;
            this.Cpu_Step_Button.Text = "Step";
            this.Cpu_Step_Button.UseVisualStyleBackColor = true;
            this.Cpu_Step_Button.Click += new System.EventHandler(this.Cpu_Step_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Open_rom_button);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 47);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Load cartridge";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Timer_start_stop_button);
            this.groupBox2.Controls.Add(this.Ram_refresh_button);
            this.groupBox2.Controls.Add(this.Stack_refresh_button);
            this.groupBox2.Controls.Add(this.Cpu_Step_Button);
            this.groupBox2.Location = new System.Drawing.Point(6, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 73);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CPU";
            // 
            // Timer_start_stop_button
            // 
            this.Timer_start_stop_button.Location = new System.Drawing.Point(121, 19);
            this.Timer_start_stop_button.Name = "Timer_start_stop_button";
            this.Timer_start_stop_button.Size = new System.Drawing.Size(116, 23);
            this.Timer_start_stop_button.TabIndex = 8;
            this.Timer_start_stop_button.Text = "Timer Start/Stop";
            this.Timer_start_stop_button.UseVisualStyleBackColor = true;
            this.Timer_start_stop_button.Click += new System.EventHandler(this.Timer_start_stop_button_Click);
            // 
            // Ram_refresh_button
            // 
            this.Ram_refresh_button.Location = new System.Drawing.Point(6, 48);
            this.Ram_refresh_button.Name = "Ram_refresh_button";
            this.Ram_refresh_button.Size = new System.Drawing.Size(75, 23);
            this.Ram_refresh_button.TabIndex = 7;
            this.Ram_refresh_button.Text = "Ram";
            this.Ram_refresh_button.UseVisualStyleBackColor = true;
            this.Ram_refresh_button.Click += new System.EventHandler(this.Ram_refresh_button_Click);
            // 
            // Stack_refresh_button
            // 
            this.Stack_refresh_button.Location = new System.Drawing.Point(162, 48);
            this.Stack_refresh_button.Name = "Stack_refresh_button";
            this.Stack_refresh_button.Size = new System.Drawing.Size(75, 23);
            this.Stack_refresh_button.TabIndex = 6;
            this.Stack_refresh_button.Text = "Stack";
            this.Stack_refresh_button.UseVisualStyleBackColor = true;
            this.Stack_refresh_button.Click += new System.EventHandler(this.Stack_refresh_button_Click);
            // 
            // PRG_listView1
            // 
            this.PRG_listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.address,
            this.value,
            this.opcode});
            this.PRG_listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PRG_listView1.Location = new System.Drawing.Point(3, 16);
            this.PRG_listView1.Name = "PRG_listView1";
            this.PRG_listView1.Size = new System.Drawing.Size(211, 549);
            this.PRG_listView1.TabIndex = 9;
            this.PRG_listView1.UseCompatibleStateImageBehavior = false;
            this.PRG_listView1.View = System.Windows.Forms.View.Details;
            // 
            // address
            // 
            this.address.Text = "Address";
            // 
            // value
            // 
            this.value.Text = "Value";
            // 
            // opcode
            // 
            this.opcode.Text = "opCode";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Cpu_Log_listView2);
            this.groupBox3.Location = new System.Drawing.Point(559, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(575, 568);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Registers";
            // 
            // Cpu_Log_listView2
            // 
            this.Cpu_Log_listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.X,
            this.Y,
            this.A,
            this.S,
            this.pc,
            this.P});
            this.Cpu_Log_listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Cpu_Log_listView2.GridLines = true;
            this.Cpu_Log_listView2.Location = new System.Drawing.Point(3, 16);
            this.Cpu_Log_listView2.Name = "Cpu_Log_listView2";
            this.Cpu_Log_listView2.Size = new System.Drawing.Size(569, 549);
            this.Cpu_Log_listView2.TabIndex = 0;
            this.Cpu_Log_listView2.UseCompatibleStateImageBehavior = false;
            this.Cpu_Log_listView2.View = System.Windows.Forms.View.Details;
            // 
            // X
            // 
            this.X.Text = "X";
            this.X.Width = 53;
            // 
            // Y
            // 
            this.Y.Text = "Y";
            this.Y.Width = 55;
            // 
            // A
            // 
            this.A.Text = "A";
            this.A.Width = 68;
            // 
            // S
            // 
            this.S.DisplayIndex = 4;
            this.S.Text = "Stack pointer";
            this.S.Width = 104;
            // 
            // pc
            // 
            this.pc.DisplayIndex = 5;
            this.pc.Text = "Program Counter";
            this.pc.Width = 115;
            // 
            // P
            // 
            this.P.DisplayIndex = 3;
            this.P.Text = "Status Flag";
            this.P.Width = 134;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.PRG_listView1);
            this.groupBox4.Location = new System.Drawing.Point(336, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(217, 568);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PrgROM";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Stack_listView3);
            this.groupBox5.Location = new System.Drawing.Point(168, 192);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(162, 388);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Stack";
            // 
            // Stack_listView3
            // 
            this.Stack_listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.Stack_listView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Stack_listView3.Location = new System.Drawing.Point(3, 16);
            this.Stack_listView3.Name = "Stack_listView3";
            this.Stack_listView3.Size = new System.Drawing.Size(156, 369);
            this.Stack_listView3.TabIndex = 9;
            this.Stack_listView3.UseCompatibleStateImageBehavior = false;
            this.Stack_listView3.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Address";
            this.columnHeader1.Width = 75;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 75;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.Ram_listView);
            this.groupBox6.Location = new System.Drawing.Point(6, 192);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(156, 388);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Ram";
            // 
            // Ram_listView
            // 
            this.Ram_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.Ram_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ram_listView.Location = new System.Drawing.Point(3, 16);
            this.Ram_listView.Name = "Ram_listView";
            this.Ram_listView.Size = new System.Drawing.Size(150, 369);
            this.Ram_listView.TabIndex = 9;
            this.Ram_listView.UseCompatibleStateImageBehavior = false;
            this.Ram_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Address";
            this.columnHeader3.Width = 75;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 75;
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // openRomFileDialog
            // 
            this.openRomFileDialog.FileName = "sample.nes";
            this.openRomFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1_FileOk);
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 605);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DebugWindow";
            this.Text = "NesEmu";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Open_rom_button;
        private System.Windows.Forms.Button Cpu_Step_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView PRG_listView1;
        private System.Windows.Forms.ColumnHeader address;
        private System.Windows.Forms.ColumnHeader value;
        private System.Windows.Forms.ColumnHeader opcode;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView Cpu_Log_listView2;
        private System.Windows.Forms.ColumnHeader X;
        private System.Windows.Forms.ColumnHeader Y;
        private System.Windows.Forms.ColumnHeader A;
        private System.Windows.Forms.ColumnHeader P;
        private System.Windows.Forms.ColumnHeader S;
        private System.Windows.Forms.ColumnHeader pc;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView Stack_listView3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListView Ram_listView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button Ram_refresh_button;
        private System.Windows.Forms.Button Stack_refresh_button;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.OpenFileDialog openRomFileDialog;
        private System.Windows.Forms.Button Timer_start_stop_button;
    }
}

