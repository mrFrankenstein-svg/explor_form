using System;
using Microsoft.Win32;      //чтобы залезть в реестр

namespace WindowsFormsApp1
{
    class Autorun
    {
        public bool SetAutorunValue(bool autorun, string name)
        {
            string ExePath = System.Windows.Forms.Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                    reg.SetValue(name, ExePath);
                else
                    reg.DeleteValue(name);

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
