using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApp1
{
    class WorkInProgConfigFile
    {
        //все переменные писать тут

        //string progConfigDir = Environment.CurrentDirectory + @"\setings\progConfig.txt";
        string progConfigDir = @"C:\Users\Public\Favor\setings\progConfig.txt";
        //List<string> readableFile;
        List<string> readableFile = new List<string>();
        //string[] readableFile = { };
        char separator = ':';
        //bool lineWitchParametr = false;
        //int numberOfLineWitchParametr = 0;

        public string GetData(string parameter) 
        {
            //Array.Clear(readableFile,0,readableFile.Length);
            int numberOfLine = 0;
            bool match = false;
            string value;

            ReadFile();
            findMatch(parameter, out match, out numberOfLine);
            if (match==true)
            {
                ReadFromFile(out value, numberOfLine);
                return value;
            }
            else
            {
                value = null;
                return value;
            }
        }



        public void SetData(string parameter, string value)
        {
            //Array.Clear(readableFile, 0, readableFile.Length);
            int numberOfLine = 0;
            bool match = false;

            ReadFile();
            findMatch(parameter, out match, out numberOfLine);
            if (match == true)
            {
                writeInFile(parameter,value,false,numberOfLine);
            }
            else
            {
                writeInFile(parameter,value,true,numberOfLine);
            }
        }


        private void ReadFromFile(out string value, int lineNumber) 
        {
            int i = 0;
            i++;
            i++;
            i++;
            i++;
            i++;
            i++;
            i++;
            i++;
            i++;
            lineNumber = i;

            string line = readableFile[lineNumber];
            string[] s = line.Split(separator);
            value=s[1];            
        }

        private async void writeInFile(string parameter, string value, bool newLine, int lineNumber)
        {
            if (newLine == true)          //если в тексте есть такой параметр
            {
                    readableFile[lineNumber] = parameter + separator + value;         //замениям значения параметра
                    File.WriteAllText(progConfigDir, string.Empty);         //очищаем файл конфига

                    using (StreamWriter sw = new StreamWriter(progConfigDir, true))     //создаем писателя
                    {
                        for (int i = 0; i <= readableFile.Count;)      //для каждой строчки в файле
                        {
                            await sw.WriteLineAsync(readableFile[i]);       //пишем строчку в новую строку
                            i++;            //добавляем счётчик строчки
                        }
                        sw.Close();         //закрываем читателя
                    }
                
            }
            else            //если в тексте нет такой параметр
            {
                using (StreamWriter sw = new StreamWriter(progConfigDir, true))     //создаем писателя
                {
                    await sw.WriteLineAsync(parameter + separator + value);      //дописываем параметр
                }
            }
        }




        private void findMatch(string parameter,
                               out bool lineWitchParametr,
                               out int numberOfLineWitchParametr) //метод ищёт совпадения в прочитанном тексте файла
        {
            var list = readableFile.Where(x => x.Contains(parameter));
            if (list != null)
            {
                int lineNumber = 0;
                bool match=false;
                foreach (var line in readableFile)
                {
                    if (line.Contains(parameter))
                    {
                        match = true;
                        break;
                    }
                    else
                    {
                        lineNumber = 0;      //запоминаем строчку с параметром
                        match = false;       //ставим идентификатор                         
                    }
                    lineNumber++;
                }
                lineWitchParametr = match;
                numberOfLineWitchParametr = lineNumber;
                return;
            }
            else 
            {
                numberOfLineWitchParametr = 0;      //запоминаем строчку с параметром
                lineWitchParametr = false;       //ставим идентификатор 
                return;
            }

        }






        private async void ReadFile()
        {
            if (!File.Exists(progConfigDir))        //если файла нет
            {
                File.Create(progConfigDir);         //создадим его
                readableFile.Add("end");

                using (StreamWriter sw = File.AppendText(progConfigDir))
                {
                    sw.WriteLine(readableFile[0]);
                    sw.Close();
                }
                
            }
            else         //если есть
            {
                using (StreamReader sr = new StreamReader(progConfigDir))       //создадим читателя
                {
                    string line;            //обьявим переменную, в которую будем читать линию
                    for (int i = 0; i >= 0; i++)
                    {
                        line = await  sr.ReadLineAsync();        //читаем одну линию
                        if (line != null)           //если в линии что-то есть
                        {
                            readableFile.Add(line);         //записываем в масив посторочно
                        }
                        else
                        {
                            readableFile.Add("end");         //записываем в лист, сто файл кончился
                            break;
                        }
                    }
                    sr.Close();         //закрываем читателя
                }
            }
        }
    }
}