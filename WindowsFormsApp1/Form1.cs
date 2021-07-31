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
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{


    public partial class Form1 : Form
    {
        Process srartProg;
        public Form1()
        {
            InitializeComponent();

            //узнает дату при включерии
            //можно сделать и так, для компактрости
            //
            //label1.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");    
            //Это можно разделять как хочешь. Можно оставить только дату или только время

            label1.Text = DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString();




            //таймер. Просто таймер, который толкает функцию "tmrShow_Tick"

            Timer tmrShow = new Timer();
            tmrShow.Interval = 100;
            tmrShow.Tick += tmrShow_Tick;
            tmrShow.Enabled = true;
        }



        struct LastInputInfo    //переменная для хранения времени бездействия системы
        {
            public uint cbSize;

            public uint dwTime;
        }

        [DllImport("user32.dll")]   // подключение библиотеки для счёта времени бездействия
        static extern bool GetLastInputInfo(out LastInputInfo plii);    //функыия подсчёта времени бездействия системы
        


        private void tmrShow_Tick(object sender, EventArgs e)      //функция счётчика времени
        {
            LastInputInfo lastInputInfo = new LastInputInfo();  //новая переменная для работы с временем бездействия
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo); //присвоение переменной времени бездействия
            GetLastInputInfo(out lastInputInfo);    //вызов функции для обновления переменной времени бездействия
            var idleTime = Environment.TickCount - lastInputInfo.dwTime;    //конвертация времени в удобоваримый вариант подсчёта
            label1.Text = Convert.ToString(idleTime);   //вывод времени на лейбу
        }





        private void button1_Click(object sender, EventArgs e)  //перезапускает
        {
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)  //стартует какой-то процесс. Нужно указать имя exe файла
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

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");    //можно разделять как хочешь. Можно оставить только дату или только время

        }

        private void button6_Click(object sender, EventArgs e)
        {
            LastInputInfo lastInputInfo = new LastInputInfo();  //новая переменная для работы с временем бездействия
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo); //присвоение переменной времени бездействия
            GetLastInputInfo(out lastInputInfo);    //вызов функции для обновления переменной времени бездействия
            var idleTime = Environment.TickCount - lastInputInfo.dwTime;    //конвертация времени в удобоваримый вариант подсчёта
            label1.Text = Convert.ToString(idleTime);   //вывод времени на лейбу
        }


    }
}