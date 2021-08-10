using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    class PowerSetings
    {
        //сюда вписать всй, что нужно будет выключить с аргументами.
        string[] jobs = { "disk-timeout-ac 0", "disk-timeout-dc 0" };

        public void SetSetings()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.CreateNoWindow = true;      //это чтоб не видно было ее. 

            for (int i = 0; i < jobs.Length;)
            {
                p.StartInfo.Arguments = "/c powercfg /change " + jobs[i];
                p.Start();
                i++;
            }
        }

    }
}
