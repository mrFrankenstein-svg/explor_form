using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class LogFile
    {
        public static void Log(string masage)
        {
            //Form1 mainClass = new Form1();
            string time = DateTime.Now.ToString("HH-mm-ss");
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            if (!Directory.Exists(Form1.thisProgrammDirectory + @"\logs"))      //если папка с логами не существует
            {
                //создаем папку для логов
                FolderCreate.PathCreate(Form1.thisProgrammDirectory + @"\logs", false);

                /*
                // создаем папку для логов на сегодня
                FolderCreate.PathCreate(Form1.thisProgrammDirectory + @"\logs\" + date, false);
                */

                //создаем сам лог на сегодня
                Form1.logFilePath = Form1.thisProgrammDirectory + @"\logs\log_" +date + ".txt";

                // записываем в лог то, что передано
                using (StreamWriter file = new StreamWriter(Form1.logFilePath, true, Encoding.Default))
                {
                    file.WriteLine(masage);
                    file.Close();
                }
            }
            else      //если папка с логами существует
            {
                /*
                if (Directory.Exists(Form1.thisProgrammDirectory + @"\logs\" + date))       //если папка на сегодня есть
                {
                */
                if (File.Exists(Form1.logFilePath))     //если файл существует
                {
                    //записываем в лог
                    using (StreamWriter file = new StreamWriter(Form1.logFilePath, true, Encoding.Default))
                    {
                        file.WriteLine(masage);
                        file.Close();
                    }
                }
                else        //если файл лога не существует
                {
                    //создаем сам лог на сегодня
                    Form1.logFilePath = Form1.thisProgrammDirectory + @"\logs\log_" + date + ".txt";
                    //записываем в лог
                    using (StreamWriter file = new StreamWriter(Form1.logFilePath, true, Encoding.Default))
                    {
                        file.WriteLine(masage);
                        file.Close();
                    }
                }


                /*
                }
                else        //если папки на сегодня нет
                {
                    // создаем папку для логов на сегодня
                    FolderCreate.PathCreate(Form1.thisProgrammDirectory + @"\logs\" + date, false);
                    //создаем сам лог на сегодня
                    Form1.logFilePath = Form1.thisProgrammDirectory + @"\logs\" + date + @"\log_" + time + ".txt";
                    // записываем в лог то, что передано
                    using (StreamWriter file = new StreamWriter(Form1.logFilePath, true, Encoding.Default))
                    {
                        file.WriteLine(masage);
                        file.Close();
                    }
                }
                */
            }
        }
    }
}
