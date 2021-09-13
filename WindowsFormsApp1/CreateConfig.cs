using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class CreateConfig
    {
        public static void stringeditor2(string file, string fileDirectory, string chtoishem)
        {
            string name = Environment.UserName;
            Translite trans = new Translite();
            trans.Tr2(name);

            if (File.Exists(file))
            {
                Encoding enc = Encoding.GetEncoding(1251);      //Это выбор формата для тхт. Не знаю, но может так лучше будет
                string[] text =  File.ReadAllLines(file, enc);       //А тут читаем файл, от которого нам передали имя, в массив,
                                                                    //который разделяет файл на кусочки с правильным форматом.
                                                                    //Не знаю почему так, но он просто дробит файл на куски
                                                                    //по несколько символов.
                string tex = "";

                for (int j = 0; j < text.Length; j++)
                {
                    for (int n = 0; n < text[j].Length;)
                    {
                        if (text[j].Length - n >= chtoishem.Length)     //если длинна кусочка больше чем нужный для замены, то
                        {
                            if (text[j].Substring(n, chtoishem.Length) == chtoishem)
                            {
                                tex += name;
                                n += name.Length;
                            }
                            else
                            {
                                tex += text[j].Substring(n, 1); //Эта конструкция берёт из стринга кусочек,
                                                                //начинающийся с n и длящийся "1" символ.
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
                }
                /*    это нужно было для того, чтобы сделать видимым на текстБоксе то, что делает функция
                 *    уже не надо, но пусть будет
                 *    
                 * foreach (string s in text)
                    tex = tex + s;
                */
               
                /*
                if (!File.Exists(fileDirectory))
                {
                    File.Create(fileDirectory);
                }
                */
                using (StreamWriter fileWrite = new StreamWriter(fileDirectory, true))
                {
                    foreach (string s in text)
                        fileWrite.WriteLine(s);
                    fileWrite.Close();
                }
                //File.WriteAllLines(fileDirectory, text);        //записываем файл, идя по предоставленному имени, в нужной кодировке.
                
            }
        }
    }
}