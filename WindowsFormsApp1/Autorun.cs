using System;
using System.Windows.Forms;     //это только для вывода меседжа при ошибке. Перед компиляцией убрать или заменить
using Microsoft.Win32;      //чтобы залезть в реестр

namespace WindowsFormsApp1
{
    class Autorun
    {
        /*
         есть один важный момент!!!!
        Автозагрузка со стартом виндовс будет происходить только тогда, когда
        автозагружаемая программа будет не от имени админа!!!!!
         */
        public bool SetAutorunValue(bool autorun, string name, string path)
        {
            /*
             * эта строчка вытягивает путь до самой запускающей программы 
             *string ExePath = System.Windows.Forms.Application.ExecutablePath;
            */
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                {
                    reg.SetValue(name, string.Format("\"{0}\"", path));
                }
                else
                {
                    reg.DeleteValue(name);
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
