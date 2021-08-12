using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class FileStatus
    { 
        public string FileStatusString(string status,string status2)
        {
             string path= System.Reflection.Assembly.GetEntryAssembly().Location;

            if (status == "status")
            {
                if (File.Exists(path + "/status.txt"))
                {
                    // File.ReadLines("status.txt").Skip(1).First();          //это если нужно что-то пропустить
                    status2 = Convert.ToString(File.ReadLines("status.txt").First());
                }
                else
                {
                    File.Create(path + "/status.txt");
                    status2 = "created";
                }
            }
            else if (status == "add")
            {
                //string[] text = File.ReadAllLines(file, enc);
            }
            return status2;
        }
        public void restart()
        {
            string path = Environment.CurrentDirectory;
            File.Create(path + "/restart.txt");
            Application.Restart();
        }
    }
}
