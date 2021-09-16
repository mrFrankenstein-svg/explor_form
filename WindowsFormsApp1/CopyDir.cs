using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class CopyDir //: IDisposable
    {
        //private bool _disposed = false; //флаг, что наш объект был Disposed

        public static void copyDir(string FromDir, string ToDir, bool hide)
        {
            
            if (!Directory.Exists(ToDir))
            {
                DirectoryInfo di = Directory.CreateDirectory(ToDir);
                if (hide)
                {
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
            }
            try
            {
                foreach (string s1 in Directory.GetFiles(FromDir))
                {
                    string s2 = ToDir + "\\" + Path.GetFileName(s1);
                    if (!File.Exists(s2))
                    {
                        File.Copy(s1, s2);
                    }
                }
                foreach (string s in Directory.GetDirectories(FromDir))
                {
                    copyDir(s, ToDir + "\\" + Path.GetFileName(s),false);
                }
               
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message + " " + FromDir+ " " + ToDir, "Copy");
            }
        }
        
        /*
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                //нельзя вызвать метод Dispose для объекта дважды
                return;
            }
            if (disposing)
            {
                //тут освобождаем все ресурсы. В нашем случае он только один.
                Close(); //по сути делаем _sw.Dispose();
            }
            _disposed = true; //помечаем флаг что метод Dispose уже был вызван
        }
        */
    }
}
