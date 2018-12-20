using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DynaHub
{
    class GlobalSettings
    {
        // Store credentials
        public static string user = null;
        public static string repo = null;
        public static string tok = null;

        // Temp folder
        public static DirectoryInfo di = null;

        public static string CreateTempFolder(string path)
        {
            try
            {
                di = Directory.CreateDirectory(path);
            }
            catch
            {
                MessageBox.Show("Something went wrong creating the temporary folder to store the graph.",
                    "Error");
            }

            return di.FullName;
        }

        public static void DeleteTempFolder()
        {
            try
            {
                di.Delete(true);
            }
            catch
            {
                MessageBox.Show("Something went wrong deleting the temporary folder storing the graphs.",
                    "Error");
            }
        }
    }
}
