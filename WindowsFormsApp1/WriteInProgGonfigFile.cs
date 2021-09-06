using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class WriteInProgGonfigFile
    {
        //все переменные писать тут

        string progConfigDir= Environment.CurrentDirectory + @"\setings\progConfig";
        string[] readableFile;
        bool lineWitchParametr = false;
        int numberOfLineWitchParametr = 0;


        public async void WIPGF(string parameter, string value)
        {
            if (!File.Exists(progConfigDir))        //если файла нет
            {
                File.Create(progConfigDir);         //создадим его
            }   

            if (File.Exists(progConfigDir))         //если есть
            {
                using (StreamReader sr = new StreamReader(progConfigDir))       //создадим читателя
                {
                    string line;            //обьявим переменную, в которую будем читать линию
                    for (int i = 0; i >= 0; i++)            
                    {
                        line = await sr.ReadLineAsync();        //читаем одну линию
                        if (line != null)           //если в линии что-то есть
                        {
                            readableFile[i] = line;         //записываем в масив посторочно
                        }
                        else 
                        {
                            break;
                        }
                    }
                    sr.Close();         //закрываем читателя
                }

                for (int i = 0; i < readableFile.Length; )          //для каждой строчки(каждого элемента массива)
                {
                    if (lineWitchParametr == true)          //если совпадение уже не найдёно
                    {
                        for (int j = 0; j < readableFile[i].Length; j++)      //берём символ
                        {
                            if (readableFile[i].Substring(j, parameter.Length) == parameter)        //если такое значение найдено, то 
                            {
                                lineWitchParametr = true;       //ставим идентификатор 
                                numberOfLineWitchParametr = i;      //запоминаем строчку с параметром
                                break;
                            }
                            if (j + parameter.Length == readableFile[i].Length)         //если строччка подходит к концу
                            {
                                break;
                            }
                        }
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }

                ///////////////////////////////////
                ///проверка файла на наличие параметра
                /*
                 
                    for (int i=0; i>=0; i++)
                    {
                        line = await sr.ReadLineAsync();
                        if (line != null || lineWitchParametr == true)
                        {
                            for (int j = 0; j < line.Length; j++)      //берём символ
                            {
                                if (line.Substring(j, parameter.Length) == parameter)        //если такое значение найдено, то 
                                {
                                    lineWitchParametr = true;
                                    numberOfLineWitchParametr = i;
                                    break;
                                }
                            }
                        }
                        else 
                        {
                            break;
                        }
                    }
                 */
                //////////////////////////////////


                if (lineWitchParametr == true)
                {

                }
            }

        }
    }
}
