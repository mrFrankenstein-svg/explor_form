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
using System.IO;        //для того, чтобы копаться в директории
using Microsoft.Win32;      //чтобы залезть в реестр

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Process srartProg;
        string progStartName;
        bool prog_started;
        string textTranslit;
        int idleTimeOld;


        public Form1()
        {
            InitializeComponent();

            //узнает дату при включерии
            //можно сделать и так, для компактрости
            //*****.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");    
            //Это можно разделять как хочешь. Можно оставить только дату или только время
            label4.Text = DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString();

            //таймер. Просто таймер, который толкает функцию "tmrShow_Tick"
            Timer tmrShow = new Timer();
            tmrShow.Interval = 1;
            tmrShow.Tick += tmrShow_Tick;
            tmrShow.Enabled = true;

            progStartName = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\" + System.AppDomain.CurrentDomain.FriendlyName;

            if (Directory.Exists(progStartName)==false)
            {
                try
                {
                    File.Copy(System.AppDomain.CurrentDomain.FriendlyName, progStartName, true);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Copy");
                }
            }

            Autorun autoR = new Autorun();
            //autoR.SetAutorunValue(true, System.AppDomain.CurrentDomain.FriendlyName, "\"" + progStartName + "\" -autorun");
            autoR.SetAutorunValue(true, System.AppDomain.CurrentDomain.FriendlyName, progStartName );

        }
        //C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp
        //папка автозагрузки для win7


        struct LastInputInfo    //переменная для хранения времени бездействия системы
        {
            public uint cbSize;

            public uint dwTime;
        }

        [DllImport("user32.dll")]   // подключение библиотеки для счёта времени бездействия
        static extern bool GetLastInputInfo(out LastInputInfo plii);    //функыия подсчёта времени бездействия системы

        LastInputInfo lastInputInfo = new LastInputInfo();  //новая переменная для работы с временем бездействия

        private void tmrShow_Tick(object sender, EventArgs e)      //функция счётчика времени
        {
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo); //присвоение переменной времени бездействия
            GetLastInputInfo(out lastInputInfo);    //вызов функции для обновления переменной времени бездействия

            int idleTime = unchecked(Environment.TickCount - (int)lastInputInfo.dwTime);    //конвертация времени в удобоваримый вариант подсчёта.
                                                                                            //Этот вариант лучше, хотя разницы я не знаю

            label1.Text = Convert.ToString(idleTime);   //вывод времени на лейбу
            label2.Text = DateTime.Now.ToString("HH.mm");

            if (idleTime >= 7200000 && prog_started == false)
            {
                prog_started = true;
                start_prog();
            }
            if (idleTime <= 100 && prog_started == true)
            {
                prog_started = false;
                close_prog();
            }

            if (checkBox1.Checked == true)
            {
                if (idleTime <= 100 && idleTimeOld >= 1000 && prog_started == false)
                {
                    printString(DateTime.Now.ToString("HH.mm.ss") + " Last input.", true);
                }
                idleTimeOld = idleTime;
            }
        }

        private void start_prog()
        { 
            
        }

        private void close_prog()
        { 
        
        }

        private void printString(string masage, bool importmant)
        {
            if (importmant == false)
            {
                textBox1.Text = masage + Environment.NewLine + textBox1.Text;
            }
            else
            {
                textBox1.Text = "¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯" + Environment.NewLine + textBox1.Text;
                textBox1.Text = masage + Environment.NewLine + textBox1.Text;
                textBox1.Text = "_______________" + Environment.NewLine + textBox1.Text;
            }

        }
        private void printInt(string masage, bool importmant)
        {
            if (importmant == false)
            {
                textBox1.Text = masage + Environment.NewLine + textBox1.Text;
            }
            else
            {
                textBox1.Text = "¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯" + Environment.NewLine + textBox1.Text;
                textBox1.Text = Convert.ToString(masage) + Environment.NewLine + textBox1.Text;
                textBox1.Text = "_______________" + Environment.NewLine + textBox1.Text;
            }
        }


        private void button1_Click(object sender, EventArgs e)  //перезапускает
        {
            Application.Restart();

        }

        private void button2_Click(object sender, EventArgs e)  //стартует какой-то процесс. Нужно указать имя exe файла
        {
            if (prog_started == false)
            {
                srartProg = Process.Start("explorer.exe");
                prog_started = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)  //убивает процесс
        {
            if (prog_started == true)
            {
                Process.GetProcessById(srartProg.Id).Kill();
                prog_started = false;
            }
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
            label3.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");    //можно разделять как хочешь. Можно оставить только дату или только время
            label3.Text = progStartName;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo); //присвоение переменной времени бездействия
            GetLastInputInfo(out lastInputInfo);    //вызов функции для обновления переменной времени бездействия
            var idleTime = Environment.TickCount - lastInputInfo.dwTime;    //конвертация времени в удобоваримый вариант подсчёта
            label1.Text = Convert.ToString(idleTime);   //вывод времени на лейбу
        }

       

        private void button7_Click(object sender, EventArgs e)
        {
            Translite tr = new Translite();
            textTranslit = textBox1.Text;
            textBox1.Text = tr.Tr2(textTranslit);
            label3.Text = tr.Tr2(textTranslit);

        }

        private void button9_Click(object sender, EventArgs e)
        {
            //oWerlord для ноутбука, oVerlord для компа
            CreateConfig cc = new CreateConfig();
            cc.stringeditor2(@"C:\Users\Owerlord\Desktop\xmrig\config.json", "rigNameeeeeeeeeee", "1111");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            PowerSetings pow = new PowerSetings();
            pow.SetSetings();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FolderCreate create = new FolderCreate();
            create.PathCreate(@"C:\Users\Owerlord\Desktop\112211");
        }
    }
}