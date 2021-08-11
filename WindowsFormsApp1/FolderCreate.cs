using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class FolderCreate
    {
        public void PathCreate(string path)
        {

            if (!Directory.Exists(path)) 
                { 
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden; 
                }
        }
       
    }
}