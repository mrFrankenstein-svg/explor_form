using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApp1
{
    class PowerSetings
    {
        //сюда вписать всй, что нужно будет выключить.
        string[] jobs = { "disk-timeout-ac", "disk-timeout-dc" , "standby-timeout-ac", "standby-timeout-dc",
            "hibernate-timeout-ac", "hibernate-timeout-dc" };

        public void SetSetings(string path)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.CreateNoWindow = true;      //это чтоб не видно было ее. 

            
            for (int i = 0; i < jobs.Length;)
            {
                p.StartInfo.Arguments = "/c powercfg /change " + jobs[i] + " 0";
                p.Start();
                i++;
            }

            File.Create(path + @"\setings\power.txt");
        }
    }
}
