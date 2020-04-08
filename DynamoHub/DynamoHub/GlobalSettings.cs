using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DynaHub.ViewModels;

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

        public static string CreateTempFolder(string subfolder)
        {
            try
            {
                // Accommodate different logins
                di = Directory.CreateDirectory(tempFolderPath + $@"{subfolder}\");
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
                    // Delete whole temp folder including login-specific subfolders
                    DirectoryInfo diTemp = new DirectoryInfo(tempFolderPath);
                    diTemp.Delete(true);
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
