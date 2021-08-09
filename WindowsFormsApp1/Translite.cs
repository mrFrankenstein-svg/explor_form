﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Translite
    {
        public string Tr2(string s) //транслитезатор имён. На всякий случай.
        {
            string ret = "";
            string[] rus = {"А","Б","В","Г","Д","Е","Ё","Ж","З","И","Й","К","Л","М","Н","О","П","Р","С","Т","У","Ф","Х","Ц","Ч","Ш","Щ","Ъ","Ы","Ь","Э","Ю","Я",
                            "а","б","в","г","д","е","ё","ж","з","и","й","к","л","м","н","о","п","р","с","т","у","ф","х","ц","ч","ш","щ","ъ","ы","ь","э","ю","я"," "};
            string[] eng = {"A","B","V","G","D","E","E","ZH","Z","I","Y","K","L","M","N","O","P","R","S","T","U","F","KH","TS","CH","SH","SH'","''","Y","'","E","YU","YA",
                            "a","b","v","g","d","e","e","zh","z","i","y","k","l","v","n","o","p","r","s","t","u","f","kh","ts","ch","sh","sh'","''","y","'","e","yu","ya"," "};

            for (int j = 0; j < s.Length; j++)
            {
                for (int i = 0; i < rus.Length; i++)
                {
                    if (s.Substring(j, 1) == rus[i])
                    {
                        ret += eng[i];
                        break;
                    }
                }
                if (ret.Length <= j)
                {
                    ret += s.Substring(j, 1);
                }

            }
            return ret;
        }
    }
}