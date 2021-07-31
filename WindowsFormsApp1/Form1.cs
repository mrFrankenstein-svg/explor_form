using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{


    public partial class Form1 : Form
    {
        Process srartProg;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //перезапускает
        {
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)  //стартует процесс
        {
            srartProg= Process.Start("explorer.exe");
        }

        private void button3_Click(object sender, EventArgs e)  //убивает процесс
        {
            Process.GetProcessById(srartProg.Id).Kill();
        }

        private void button4_Click(object sender, EventArgs e)  //прячет программу
        {
            Hide();
        }
        protected override void WndProc(ref Message m)  //события при подключении и отключении усб
        {
            base.WndProc(ref m);
            if (m.WParam.ToInt32() == 0x8000)
                label1.Text = "USB connected!";
            if (m.WParam.ToInt32() == 0x8004)
                label1.Text = "USB  disconnected!";
        }

    }
}