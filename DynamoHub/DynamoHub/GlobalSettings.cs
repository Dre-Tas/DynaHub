using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        // Define where to download GitHub file inside of Dynamo folders

        // Get location of assembly / dll file
        private static readonly string assemblyFolder =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        // Navigate up and create tempfolder
        private static readonly string tempFolderPath =
            Path.GetFullPath(Path.Combine(assemblyFolder, @"..\temp\"));

        public static string CreateTempFolder()
        {
            try
            {
                di = Directory.CreateDirectory(tempFolderPath);
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
            if (File.Exists(tempFolderPath))
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
}
