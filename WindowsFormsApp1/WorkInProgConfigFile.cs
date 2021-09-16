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

        string progConfigDir =Form1.thisProgrammDirectory + @"\setings\progConfig.txt";
        //List<string> readableFile;
        List<string> readableFile = new List<string>();
        //string[] readableFile = { };
        char separator = ':';
        int numberOfLine = 0;
        bool match = false;
        string value = "";
        string[] valueMassive;
        //bool lineWitchParametr = false;
        //int numberOfLineWitchParametr = 0;

        public string GetData(string parameter) 
        {
            value = "";
            ReadFile();
            //findMatch(parameter, out match, out numberOfLine);
            findMatch(parameter);
            if (match==true)
            {
                ReadFromFile();
                readableFile.Clear();
                numberOfLine = 0;
                match = false;
                return value;
            }
            else
            {
                value = "not exist";
                readableFile.Clear();
                numberOfLine = 0;
                match = false;
                return value;
            }
        }



        public void SetData(string parameter, string value)
        {
            //Array.Clear(readableFile, 0, readableFile.Length);

            ReadFile();
            //findMatch(parameter, out match, out numberOfLine);
            findMatch(parameter);
            if (match == true)
            {
                writeInFile(parameter,value,false,numberOfLine);
            }
            else
            {
                writeInFile(parameter,value,true,numberOfLine);
            }
            readableFile.Clear();
            numberOfLine = 0;
            match = false;
        }


        private void ReadFromFile()
        {
            //string line = readableFile[numberOfLine];
            valueMassive = readableFile[numberOfLine].Split(separator);
            value=valueMassive[1];            
        }

        private void writeInFile(string parameter, string value, bool newLine, int lineNumber)
        {
            if (newLine == false)          //если в тексте есть такой параметр
            {
                    readableFile[lineNumber] = parameter + separator + value;         //замениям значения параметра
                    // File.WriteAllText(progConfigDir, string.Empty);         //очищаем файл конфига
                    File.Delete(progConfigDir);

                    using (StreamWriter sw = new StreamWriter(progConfigDir, true))     //создаем писателя
                    {
                        for (int i = 0; i < readableFile.Count;)      //для каждой строчки в файле
                        {
                            sw.WriteLine(readableFile[i]);       //пишем строчку в новую строку
                            i++;            //добавляем счётчик строчки
                        }
                        sw.Close();         //закрываем читателя
                    }                
            }
            else            //если в тексте нет такой параметр
            {
                using (StreamWriter sw = new StreamWriter(progConfigDir, true))     //создаем писателя
                {
                    sw.WriteLine(parameter + separator + value);      //дописываем параметр
                }
            }
        }




        private void findMatch(string parameter) //метод ищёт совпадения в прочитанном тексте файла
        {
            //linqList = (List<string>)readableFile.Where(x => x.Contains(parameter));

            for (int i=0; i<readableFile.Count;)
            {
                if(readableFile[i].Contains(parameter))
                {
                    match = true;
                    numberOfLine = i;
                    break;
                }
                i++;
                numberOfLine = i;
            }
            /*
            if (linqList != null)
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
            */
        }

        private void ReadFile()
        {
            if (!Directory.Exists(Form1.thisProgrammDirectory + @"\setings"))
            {
                FolderCreate.PathCreate(Form1.thisProgrammDirectory + @"\setings", false);
            }
            if (!File.Exists(progConfigDir))        //если файла нет
            {
                readableFile.Add("");

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
                    string line="";            //обьявим переменную, в которую будем читать линию
                    for (int i = 0; i >= 0; i++)        //бесконечный цыкл, из которого выведет только конец файла
                    {
                        line = sr.ReadLine();        //читаем одну линию
                        if (line != null)           //если в линии что-то есть
                        {
                            readableFile.Add(line);         //записываем в масив посторочно
                        }
                        else
                        {
                            break;
                        }
                    }
                    sr.Close();         //закрываем читателя
                }
            }
        }
    }
}