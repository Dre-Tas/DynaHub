using Octokit;
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
        // Store login credentials
        // if login with token
        public static string user = null;
        public static string repo = null;
        public static string tok = null;
        // if login with GH account
        public static string email = null;
        public static string password = null;
        public static string repoName = null;

        // Temp folder
        public static DirectoryInfo di = null;

        // Define where to download GitHub file inside of Dynamo folders

        // Get location of assembly / dll file
        private static readonly string assemblyFolder =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        // Navigate up and create tempfolder
        private static readonly string tempFolderPath =
            Path.GetFullPath(Path.Combine(assemblyFolder, @"..\temp\"));
        // Navigate to Dynamo's packages folder
        private static readonly string packFolderPath =
            Path.GetFullPath(Path.Combine(assemblyFolder, @"..\..\"));

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

        public static async void DownloadPackagesAsync()
        {
            if (Directory.Exists(packFolderPath))
            {
                // Store package folder
                IReadOnlyList<RepositoryContent> foldersLevel = null;

                try
                {
                    // Check repo's subfolders
                    foreach (string f in Views.Login.repoFolders)
                    {
                        // Take only packages folder
                        if (f == "packages")
                        {
                            // If user logged in through token
                            if (user != null && repo != null)
                            {
                                foldersLevel =
                                    await Views.Login.client.Repository.Content.GetAllContents(
                                        user,
                                        repo,
                                        f);
                            }
                            // If user logged in through credentials
                            else if (repoName != null)
                            {
                                foldersLevel =
                                    await Views.Login.client.Repository.Content.GetAllContents(
                                        repoName.Split('/')[0],
                                        repoName.Split('/')[1],
                                        f);
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Sorry, can't find the packages folder.",
                        "Error");
                }

                try
                {
                    // Instantiate web client to download file
                    WebClient wc = new WebClient();

                    foreach (RepositoryContent s in foldersLevel)
                    {
                        // Take only compressed files
                        if (s.Name.EndsWith(".zip"))
                        {
                            // Download compressed file
                            wc.DownloadFile(s.DownloadUrl, packFolderPath + s.Name);
                            // Extract compressed file
                            ZipFile.ExtractToDirectory(packFolderPath + s.Name, packFolderPath);
                            // Delete original compressed file
                            File.Delete(packFolderPath + s.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error while downloading");
                }
            }
        }

    }
}
