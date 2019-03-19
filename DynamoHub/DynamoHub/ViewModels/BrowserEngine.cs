using DynaHub.ViewModels;
using DynaHub.Views;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DynaHub.ViewModels
{
    class BrowserEngine
    {

        internal static void PopulateTree(SortedDictionary<string, string> content, TreeView tree)
        {
            // If there's dynamo stuff in the repo
            if (content.Keys.Count != 0)
            {
                // Get unique folders to define headers of treeview
                // General functioning from here: https://www.dotnetperls.com/treeview-wpf
                SortedDictionary<string, string>.KeyCollection filePaths = content.Keys;

                // Get name of the unique folders
                HashSet<string> headers = new HashSet<string>();
                foreach (string p in filePaths)
                {
                    headers.Add(p.Split('/').First());
                }

                // Clear TreeView before re-populating it
                tree.Items.Clear();

                // Loop all files in folders and build treeview
                foreach (string h in headers)
                {
                    // Create headers
                    TreeViewItem tvItem = new TreeViewItem();
                    tvItem.Header = h;

                    // List to store all files in each folder
                    List<string> filesInFolder = new List<string>();

                    // Get files which path is the same of the header
                    foreach (var i in filePaths.Where(x => x.StartsWith(h)))
                    {
                        filesInFolder.Add(i.ToString().Split('/').Last());
                    }

                    // The itmes at the lower level are names of the files without the folder
                    tvItem.ItemsSource = filesInFolder;

                    // Add them to the treeview
                    tree.Items.Add(tvItem);
                }
            }
            // If there's no dynamo
            else
            {
                // Clear TreeView before re-populating it
                tree.Items.Clear();

                // If there's no dynamo
                // Create headers
                TreeViewItem tvItem = new TreeViewItem();
                tvItem.Header = "There's no Dynamo (.dyn) file here!";

                // Add them to the treeview
                tree.Items.Add(tvItem);
            }
        }

        internal static string GetTreeItemNameInDict(string treeItem,
                                SortedDictionary<string, string> dict)
        {
            // Get keys collection in dict
            SortedDictionary<string, string>.KeyCollection keys = dict.Keys;

            // Get corresponding
            string path = keys.Where(k => k.Contains(treeItem)).First();

            return path;
        }

        internal static string GetUriFromDict(SortedDictionary<string, string> dict, string key)
        {
            return dict[key];
        }

        internal static string GenerateFileName(string name)
        {
            string tempFold;

            // Create temp directory
            try
            {
                tempFold = GlobalSettings.CreateTempFolder();
            }
            catch
            {
                MessageBox.Show("Something went wrong creating the temporary folder to store the graph.",
                    "Error");
                return null;
            }

            // Assemble download path
            return tempFold + name;
        }

        internal static void DownlodFileAtLocation(string uri, string location)
        {
            // Instantiate web client to download file
            WebClient wc = new WebClient();

            // Download file locally
            try
            {
                wc.DownloadFile(uri, location);
            }
            catch (WebException)
            {
                Helpers.ErrorMessage("Sorry, I couldn't find that file online.");
                return;
            }
            catch
            {
                Helpers.ErrorMessage("Ooops, something went wrong.");
                return;
            }

            // Notify user but don't block process in case user doesn't close window
            AutoClosingMessageBox.Show("Downloaded (temporarily)! Opening now...", "Success!", 2000);
        }
    }
}
