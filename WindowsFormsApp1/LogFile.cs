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
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            if (!Directory.Exists(Form1.thisProgrammDirectory + @"\logs"))      //если папка с логами не существует
            {
                //создаем папку для логов
                FolderCreate.PathCreate(Form1.thisProgrammDirectory + @"\logs", false);

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
            }
        }
    }
}
