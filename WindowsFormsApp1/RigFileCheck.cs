using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{

    class RigFileCheck
    {
        public static bool check()
        {
            //создаём список кужных файлов
            string[] files = { "config.json", "SHA256SUMS", "start.cmd", "WinRing0x64.sys", "xmrig.exe" };
            //создаём переменную для вывода результата
            bool check = false;
            //создаём счётчик количества файлов
            int numOfFiles = 0;
            //берём каждый файл в папке рига
            foreach (string s in Directory.GetFiles( Form1.theSecondDirectoryOfTheExecutingProgram))
            {
                //проганяем его церез цикл сравнения
                for (int i = 0; i <= files.Length;)
                {
                    //если название файла совпадает с названием из списка
                    if (s == Form1.theSecondDirectoryOfTheExecutingProgram + @"\" + files[i])
                    {
                        //плюсуем счётчик
                        numOfFiles++;
                        break;
                    }
                    i++;
                }
            }
            //если зайденных файлов столько же, сколько и в списке
            if (numOfFiles == files.Length)
            {
                //ставим положительный результат
                check = true;
            }
            //выводим результат
            return check;
        }
    }
}
