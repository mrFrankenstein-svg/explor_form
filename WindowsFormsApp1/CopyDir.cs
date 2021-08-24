using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class CopyDir
    {        
        public void copyDir(string FromDir, string ToDir, bool hide)
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
                    File.Copy(s1, s2);
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
    }
}
