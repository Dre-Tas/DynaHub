using DynaHub.ViewModels;
using DynaHub.Views;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DynaHub.ViewModels
{
    class BrowserEngine
    {
        private static TreeView tree;

        internal static List<string> CreateRoot(IEnumerable<string> filePaths)
        {
            // Group by parent folder
            IEnumerable<IGrouping<string, string>> groupedPaths =
                filePaths.GroupBy(p => p.Split('/').First());

            // Report which files are part of the root so you don't use them again
            List<string> used = new List<string>();

            // Add single files of first hierarchical level to root of tree
            foreach (var single in groupedPaths.Where(g => g.Count() == 1))
            {
                string inUse = single.SingleOrDefault();
                tree.Items.Add(inUse);
                used.Add(inUse);
            }

            return used;
        }

        internal static void CreateTree(IEnumerable<string> filePaths, TreeViewItem previousItem)
        {
            // Group by parent folder (recursively)
            IEnumerable<IGrouping<string, string>> groupedPaths =
                filePaths.GroupBy(p => p.Split('/').First());

            // Loop through grouped strings = paths in same folder
            foreach (var group in groupedPaths)
            {
                // Create tree node
                TreeViewItem nodeItem = new TreeViewItem();
                // Define header as parent folder
                nodeItem.Header = group.Key;

                List<string> paths = new List<string>();
                List<string> files = new List<string>();

                foreach (var item in group)
                {
                    // the name of the node is without parent folder
                    string nodeName = item.Replace(group.Key + '/', "");
                    // Check if it's still a folder path of just a file name
                    bool cond = nodeName.Contains('/');

                    // If it's a path add to paths
                    if (cond)
                        paths.Add(nodeName);
                    // Otherwise it is a file name (add to files)
                    else
                        nodeItem.Items.Add(nodeName);
                }

                if (previousItem == null)
                {
                    tree.Items.Add(nodeItem); 
                }
                else
                {
                    previousItem.Items.Add(nodeItem);
                }
                // Recursion to reproduce full path structure
                if (paths.Count != 0)
                {
                    CreateTree(paths, nodeItem);
                }
            }
        }

        internal static void PopulateTree(SortedDictionary<string, string> content, TreeView tree)
        {
            // Clear TreeView before re-populating it
            tree.Items.Clear();

            // If there's dynamo stuff in the repo
            if (content.Keys.Count != 0)
            {
                SortedDictionary<string, string>.KeyCollection filePaths = content.Keys;

                // Reference tree outside mathod so it can be used by everyone
                BrowserEngine.tree = tree;

                // Create and populate the root
                List<string> rootFiles = CreateRoot(filePaths);

                // Get rid of files in root folder from list of paths
                List<string> remaining = new List<string>();
                foreach (string path in filePaths)
                {
                    if (!rootFiles.Contains(path))
                    {
                        remaining.Add(path);
                    }
                }

                // Create and populate the tree
                CreateTree(remaining, null);
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
