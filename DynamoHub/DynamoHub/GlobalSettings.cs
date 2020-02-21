using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DynaHub
{
    class GlobalSettings
    {
        // Aknowledge if the user logged in
        internal static bool logged = false;

        // Temp folder
        public static DirectoryInfo di = null;

        // Get location of assembly / dll file
        private static readonly string assemblyFolder =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        // Navigate up and create tempfolder
        private static readonly string tempFolderPath =
            Path.GetFullPath(Path.Combine(assemblyFolder, @"..\temp\"));
        // Navigate to Dynamo's packages folder
        internal static readonly string packFolderPath =
            Path.GetFullPath(Path.Combine(assemblyFolder, @"..\..\"));

        // Decryption .dll file location
        // TODO: point to DynaHub's location using above assemblyFolder
        internal static readonly string decryptionDll =
            @"C:\Users\sydata\source\repos\DynaHub-Crypto\DynaHub-Crypto\bin\Debug\DynaHub-Crypto.dll";
        //TEMP


        #region URIs
        internal static Uri validationUri = new Uri(
            "pack://application:,,,/DynaHub;component/Resources/verification.png",
            UriKind.RelativeOrAbsolute);

        internal static Uri validatedUri = new Uri(
            "pack://application:,,,/DynaHub;component/Resources/verified.png",
            UriKind.RelativeOrAbsolute);

        internal static Uri downloadingUri = new Uri(
            "pack://application:,,,/DynaHub;component/Resources/Downloading.png",
            UriKind.RelativeOrAbsolute);

        #endregion

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
            if (Directory.Exists(tempFolderPath))
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
