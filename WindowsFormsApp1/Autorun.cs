using System;
using System.IO;
using System.Windows.Forms;     //это только для вывода меседжа при ошибке. Перед компиляцией убрать или заменить
using Microsoft.Win32;      //чтобы залезть в реестр

namespace WindowsFormsApp1
{
    class Autorun
    {
        public static string valueFromConfig = "";

        /*
         есть один важный момент!!!!
        Автозагрузка со стартом виндовс будет происходить только тогда, когда
        автозагружаемая программа будет не от имени админа!!!!!
         */
        public static bool SetAutorunValue(bool autorun, string name, string path)
        {
            /*
             *  эта строчка вытягивает путь до самой запускающей программы
             *  string ExePath = System.Windows.Forms.Application.ExecutablePath;
             */
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            WorkInProgConfigFile config = new WorkInProgConfigFile();
            valueFromConfig = config.GetData("autorun");

            try
            {
                //если функцию нужно добавить в регистр
                if (autorun)        
                {
                    //if (!File.Exists(path + @"\setings\autorn.txt"))

                    //если в файле конфига программы НЕТ значения авторана
                    if (valueFromConfig != "not exist")     
                    {
                        //устанавливаем записть в регистр
                        reg.SetValue(name, string.Format("\"{0}\"", path + @"\" + name));
                        //записываем в файл конфига о прогрессе
                        config.SetData("autorun", "1");
                    }
                    //если в файле конфига программы ЕСТЬ значения авторана
                    else
                    { 
                        //если значение =1 (true)
                        if(valueFromConfig=="1")
                        {

                        }
                        //если значение 
                        else 
                        {
                            
                        }
                    }
                }
                else
                {
                    reg.DeleteValue(name);
                    if (File.Exists(path + @"\setings\autorn.txt"))
                    {
                        File.Delete(path + @"\setings\autorn.txt");
                    }
                }
                reg.Close();
            }
            catch 
            {
                return false;
            }
            return true;
        }
    }
}