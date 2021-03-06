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
        char separator = ' ';
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
            //читаем файл
            ReadFile();
            //findMatch(parameter, out match, out numberOfLine);
            //ищем параметр из прочитанного файла
            findMatch(parameter);
            //если нашли
            if (match == true)
            {
                //приписываем значение к параметру
                writeInFile(parameter,value,false,numberOfLine);
            }
            //если не нашли
            else
            {
                writeInFile(parameter,value,true,numberOfLine);
            }
            readableFile.Clear();
            numberOfLine = 0;
            match = false;
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
                    string line = "";            //обьявим переменную, в которую будем читать линию
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

        private void findMatch(string parameter) //метод ищёт совпадения в прочитанном тексте файла
        {
            //linqList = (List<string>)readableFile.Where(x => x.Contains(parameter));

            //для каждой СТРОЧКИ из ЛИСТА, в который мы считали файл
            for (int i = 0; i < readableFile.Count;)
            {
                //делим выбранную строчку в массив по разделителю, который назначен сверху в этом методе
                string[] line = readableFile[i].Split(separator);
                //если получилось разделить и после разделения есть 2 кусочка массива
                if (line.Length > 1)
                {
                    //берём пкервый кусочек массива и сравнимаем его с искомым параметром

                    if (line[0] == parameter)       //если нашли
                    {
                        //ставим флаг, что нашли
                        match = true;
                        //запоминаем номер строчки, в которой нашли
                        numberOfLine = i;
                        //полностью заканчиваем поиск
                        break;
                    }
                }
                //еслине нашли
                //прибовляем значение и продолжаем поиск
                i++;
                //вроде как не нужное запоминание строчки каждый раз, но пусть будет пока что
                //в таком виде
                //numberOfLine = i;
            }
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

                    //ну... получается удаляем старый файл и создаем новый. Но чуть позже
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
            else            //если в тексте нет такого параметра
            {
                using (StreamWriter sw = new StreamWriter(progConfigDir, true))     //создаем писателя
                {
                    sw.WriteLine(parameter + separator + value);      //дописываем параметр
                }
            }
        }
    }
}