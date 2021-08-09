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
        public void stringeditor2(string file, string chtoishem, string nachtomenyaem)
        {
            if (File.Exists(file))
            {
                Encoding enc = Encoding.GetEncoding(1251);
                string[] text = File.ReadAllLines(file, enc);
                string tex = "";
                int length = chtoishem.Length;

                for (int j = 0; j < text.Length; j++)
                {
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
                }
                /*    это нужно было для того, чтобы сделать видимым на текстБоксе то, что делает функция
                 *    уже не надо, но пусть будет
                 *    
                 * foreach (string s in text)
                    tex = tex + s;
                */
                File.WriteAllLines(file, text, enc);

            }
        }
    }
}
