using DynaHub.Views;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using Microsoft.VisualBasic;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DynaHub.ViewModels
{
    class GetPackages
    {
        internal static void PopulateListBox(
            SortedDictionary<string, string> content,
            System.Windows.Controls.ListBox listBox)
        {
            listBox.Items.Clear();

            if (content.Keys.Count != 0)
            {
                foreach (string key in content.Keys)
                {
                    // Add an initial item that just tells the user to select a repo
                    ListBoxItem item = new ListBoxItem();
                    item.Content = key.Split('/').Last();

                    listBox.Items.Add(item);
                }
            }
            else
                listBox.Items.Add("No zipped packages found here.");
        }

        internal static List<string> CheckUninstall(SortedDictionary<string, string> repoContent)
        {
            List<string> toUninstall = new List<string>();

            foreach (KeyValuePair<string, string> pair in repoContent)
            {
                string zipName = pair.Key.Split('/').Last();
                string downloadPath = GlobalSettings.packFolderPath + zipName;

                string packagePath = downloadPath.Replace(".zip", "");

                // Check if this folder already exists
                if (Directory.Exists(packagePath))
                    toUninstall.Add(packagePath);
            }

            return toUninstall;
        }

        // This method gets called only if method above returns more than 0 elements
        internal static void UninstallPackages(List<string> uninstall)
        {
            // Check if there's anything to uninstall
            int countUninstall = uninstall.Count;

            // Communicate to user
            Helpers.InfoMessage("...Now don't freak out, ok?" +
                Environment.NewLine +
                $"you already had {countUninstall} of the packages installed, so I have to " +
                "uninstall them/it in order to to install the right version." +
                Environment.NewLine +
                "To do so I'll have to close Dynamo now. You'll have to restart it and then" +
                "re-run the Get Packages command so that I can download the right ones." +
                Environment.NewLine + "Alright, I'll see you in a bit!");

            foreach (string path in uninstall)
            {
                // Delete package by adding to list of packages to uninstall
                Packages.loadedParams.ViewStartupParams
                    .Preferences.PackageDirectoriesToUninstall.Add(path);
            }
        }

        // This doesn't come from the check because I want to install ALL the packages from gh
        internal static void InstallPackages(SortedDictionary<string, string> repoContent)
        {
            // Instantiate web client to download file
            WebClient wc = new WebClient();

            foreach (KeyValuePair<string, string> pair in repoContent)
            {
                string zipName = pair.Key.Split('/').Last();
                string downloadPath = GlobalSettings.packFolderPath + zipName;

                // Download compressed file
                wc.DownloadFile(pair.Value, downloadPath);
                // Extract compressed file
                ZipFile.ExtractToDirectory(downloadPath, GlobalSettings.packFolderPath);
                // Delete original compressed file
                File.Delete(downloadPath);
            }
        }

        internal static void CloseDynamo()
        {
            // Notify user before closing
            Helpers.InfoMessage("I'm closing Dynamo now as it needs to restart to get the right info."
                + Environment.NewLine +
                "You'll have to restart it once it's closed.");

            // Close Dynamo
            Packages.loadedParams.DynamoWindow.Close();
        }
    }
}
