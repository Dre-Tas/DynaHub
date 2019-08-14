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
        private static TreeView treeAno;

        //private static TreeViewItem previousItem = null;

        internal static void Test(IEnumerable<string> filePaths, TreeViewItem previousItem)
        {
            // Group by parent folder
            IEnumerable<IGrouping<string, string>> groupedPaths =
                filePaths.GroupBy(p => p.Split('/').First());


            foreach (var group in groupedPaths)
            {
                TreeViewItem tvItem = new TreeViewItem();
                tvItem.Header = group.Key;

                List<string> paths = new List<string>();
                List<string> files = new List<string>();

                foreach (var item in group)
                {
                    string trunc = item.Replace(group.Key + '/', "");
                    bool cond = trunc.Contains('/');
                    if (cond)
                        paths.Add(trunc);
                    else if (!cond)
                        tvItem.Items.Add(trunc);
                }

                //tvItem.ItemsSource = files;

                if (previousItem == null)
                {
                    treeAno.Items.Add(tvItem); 
                }
                else
                {
                    previousItem.Items.Add(tvItem);
                }

                if (paths.Count != 0)
                {
                    Test(paths, tvItem);
                }
                //else
                //{
                //    previousItem = null;
                //}
            }
        }

        internal static void PopulateTree(SortedDictionary<string, string> content, TreeView tree)
        {
            // Clear TreeView before re-populating it
            tree.Items.Clear();

            // If there's dynamo stuff in the repo
            if (content.Keys.Count != 0)
            {
                // General functioning from here: https://www.dotnetperls.com/treeview-wpf
                SortedDictionary<string, string>.KeyCollection filePaths = content.Keys;

                treeAno = tree;

                //TEST
                Test(filePaths, null);



                //// Group by parent folder
                //IEnumerable<IGrouping<string, string>> groupedPaths =
                //    filePaths.GroupBy(p => p.Split('/').First());

                //foreach (IGrouping<string, string> group in groupedPaths)
                //{
                //    string header = group.First();

                //    // Create headers
                //    TreeViewItem tvItem = new TreeViewItem();
                //    tvItem.Header = header;

                //    tvItem.ItemsSource = group;

                //    tree.Items.Add(tvItem);
                //}

                // Get unique folders to define headers of treeview
                //HashSet<string> headers = new HashSet<string>();
                foreach (string path in filePaths)
                {



                    //string[] subPaths = path.Split('/');
                    //List<string> subPathsReversed = path.Split('/').Reverse().ToList();

                    //if (subPaths.Length > 0)
                    //{
                    //    for (int i = 0; i < subPathsReversed.Count(); i++)
                    //    {
                    //        TreeViewItem tvItem = new TreeViewItem();

                    //        if ((i + 1) < subPathsReversed.Count())
                    //        {
                    //            tvItem.Header = subPathsReversed[i + 1];

                    //            string item = subPathsReversed[i];

                    //            tvItem.Items.Add(item);

                    //            tree.Items.Add(tvItem);
                    //        }                            
                    //    }


                    //    //foreach (string subPath in subPaths.Reverse())
                    //    //{
                    //    //    TreeViewItem tvItem = new TreeViewItem();



                    //    //}
                    //}

                    //headers.Add(path.Split('/').First());
                }


                //// Loop all files in folders and build treeview
                //foreach (string h in headers)
                //{
                //    // Create headers
                //    TreeViewItem tvItem = new TreeViewItem();
                //    tvItem.Header = h;

                //    // List to store all files in each folder
                //    List<string> filesInFolder = new List<string>();

                //    // Get files which path is the same of the header
                //    foreach (var i in filePaths.Where(x => x.StartsWith(h)))
                //    {
                //        filesInFolder.Add(i.ToString().Split('/').Last());
                //    }

                //    // The itmes at the lower level are names of the files without the folder
                //    tvItem.ItemsSource = filesInFolder;

                //    // Add them to the treeview
                //    tree.Items.Add(tvItem);
                //}
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
