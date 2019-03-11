using DynaHub.ViewModels;
using DynaHub.Views;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DynaHub.ViewModels
{
    class BrowserEngine
    {
        internal static async Task<IReadOnlyList<Repository>> GetUserReposAsync()
        {
            // Get users repos from GH
            IReadOnlyList<Repository> userRepos =
                await GitHubConnection.client.Repository.GetAllForCurrent();

            return userRepos;
        }

        #region BAD!!!!
        // This should probably be recursive to go through any folder at any level!

        // contents of the repo highest level
        internal static List<RepositoryContent> repoLevel = new List<RepositoryContent>();

        // List for all folders in repo to be queried
        internal static List<string> repoFolders = new List<string>();

        // Dictionary with both repo path and download_url
        internal static SortedDictionary<string, string> repoFiles =
            new SortedDictionary<string, string>();

        internal static async Task<SortedDictionary<string, string>> GetRepoContentAsync(
            Repository repository)
        {
            long repositoryID = repository.Id;
            // Get everything in repo at higher level of hierarchy
            // (in this case retrieves readme.MD, and folders)
            IReadOnlyList<RepositoryContent> allContent = null;
            allContent =
                    await GitHubConnection.client.Repository.Content.GetAllContents(repositoryID);
            // transform into a normal list that can be cleared
            foreach (var c in allContent)
            {
                repoLevel.Add(c);
            }

            foreach (RepositoryContent r in repoLevel)
            {
                if (r.Name.EndsWith(".dyn"))
                {
                    repoFiles.Add(r.Path, r.DownloadUrl);
                }
                else if (r.Type == "dir")
                {
                    repoFolders.Add(r.Path);
                }
            }

            // Check repo's subfolders
            foreach (string f in repoFolders)
            {
                // Get from specific path in repo
                IReadOnlyList<RepositoryContent> foldersLevel =
                    await GitHubConnection.client.Repository.Content.GetAllContents(
                        repositoryID, f);

                foreach (RepositoryContent s in foldersLevel)
                {
                    if (s.Name.EndsWith(".dyn"))
                    {
                        repoFiles.Add(s.Path, s.DownloadUrl);
                    }
                }
            }

            return repoFiles;
        }
        #endregion BAD!!!!

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
    }
}
