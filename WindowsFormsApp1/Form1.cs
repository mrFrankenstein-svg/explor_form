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
using System.IO; //добавить!

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Process srartProg;
        string progStartName;
        bool prog_started;
        string textTranslit;

        public Form1()
        {
            InitializeComponent();

            //узнает дату при включерии
            //можно сделать и так, для компактрости
            //
            //label1.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");    
            //Это можно разделять как хочешь. Можно оставить только дату или только время

            label4.Text = DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString();
            
            //таймер. Просто таймер, который толкает функцию "tmrShow_Tick"

            Timer tmrShow = new Timer();
            tmrShow.Interval = 1;
            tmrShow.Tick += tmrShow_Tick;
            tmrShow.Enabled = true;


            progStartName= @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\" + System.AppDomain.CurrentDomain.FriendlyName;

            if (Directory.Exists(progStartName)==false)
            {
                try
                {
                    File.Copy(System.AppDomain.CurrentDomain.FriendlyName, progStartName, true);
                }

                catch (Exception ex)
                {
                   // MessageBox.Show(ex.Message);
                }
            }         

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

            // int idleTime = Environment.TickCount - (int)lastInputInfo.dwTime;    //конвертация времени в удобоваримый вариант подсчёта

            int idleTime = unchecked(Environment.TickCount - (int)lastInputInfo.dwTime);    //конвертация времени в удобоваримый вариант подсчёта.
                                                                                            //Этот вариант лучше, хотя разницы я не знаю
            idleTime = idleTime / 1000;
            
            label1.Text = Convert.ToString(idleTime);   //вывод времени на лейбу
            label2.Text = DateTime.Now.ToString("HH.mm");

            if (idleTime >= 7200000)
            {
                prog_started = true;
                start_prog();
            }
        }

        private void start_prog()
        { 
            
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
            //LastInputInfo lastInputInfo = new LastInputInfo();  //новая переменная для работы с временем бездействия
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo); //присвоение переменной времени бездействия
            GetLastInputInfo(out lastInputInfo);    //вызов функции для обновления переменной времени бездействия
            var idleTime = Environment.TickCount - lastInputInfo.dwTime;    //конвертация времени в удобоваримый вариант подсчёта
            label1.Text = Convert.ToString(idleTime);   //вывод времени на лейбу
        }

        private static string Tr2(string s) //транслитезатор имён. На всякий случай.
        {
            string ret = "";
            string[] rus = {"А","Б","В","Г","Д","Е","Ё","Ж","З","И","Й","К","Л","М","Н","О","П","Р","С","Т","У","Ф","Х","Ц","Ч","Ш","Щ","Ъ","Ы","Ь","Э","Ю","Я",
                            "а","б","в","г","д","е","ё","ж","з","и","й","к","л","м","н","о","п","р","с","т","у","ф","х","ц","ч","ш","щ","ъ","ы","ь","э","ю","я"," "};
            string[] eng = {"A","B","V","G","D","E","E","ZH","Z","I","Y","K","L","M","N","O","P","R","S","T","U","F","KH","TS","CH","SH","SH'","''","Y","'","E","YU","YA",
                            "a","b","v","g","d","e","e","zh","z","i","y","k","l","v","n","o","p","r","s","t","u","f","kh","ts","ch","sh","sh'","''","y","'","e","yu","ya"," "};

            for (int j = 0; j < s.Length; j++)
            {
                for (int i = 0; i < rus.Length; i++)
                {
                    if (s.Substring(j, 1) == rus[i])
                    { 
                        ret += eng[i];
                        break;
                    }
                }
                if (ret.Length<=j) 
                {
                    ret += s.Substring(j, 1);
                }
             
            }
            return ret;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textTranslit = textBox1.Text;
            textBox1.Text = Tr2(textTranslit);
            label3.Text = Tr2(textTranslit);
        }
    }
}