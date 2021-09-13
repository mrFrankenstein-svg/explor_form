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
        public static void PathCreate(string path, bool hide)
        {

            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                if (hide)
                {
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
            }
        }
       
    }
}