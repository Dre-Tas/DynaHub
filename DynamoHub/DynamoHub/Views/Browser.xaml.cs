using DynaHub.ViewModels;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DynaHub.Views
{
    public partial class Browser : Window
    {
        // Create variable to pass to main Dynamo method
        internal static string toOpen = null;

        public Browser()
        {
            InitializeComponent();

            selectReposCB.IsEditable = true;
            selectReposCB.IsReadOnly = true;
            selectReposCB.Text = "select a repository";
        }

        private readonly List<Repository> localReposList = new List<Repository>();

        private async void PopulateComboAsync(object sender, EventArgs e)
        {
            if (selectReposCB.Items.Count == 0)
            {
                Task<IReadOnlyList<Repository>> getRepos = GitHubInfo.GetUserReposAsync();

                // Display in-the-meantime text
                selectReposCB.Text = "...retrieving repos";

                IReadOnlyList<Repository> repositories = await getRepos;

                // Populate combobox
                foreach (Repository r in repositories)
                {
                    selectReposCB.Items.Add(r.FullName);
                    // Populate local list (needed below)
                    localReposList.Add(r);
                }
            }
        }

        // Store files with their download url
        SortedDictionary<string, string> repoContent = new SortedDictionary<string, string>();

        private async void OnSelectedAsync(object sender, SelectionChangedEventArgs e)
        {
            string selectionString = null;
            selectionString = selectReposCB.SelectedItem.ToString();

            // Show text in combobox
            selectReposCB.Text = selectionString;

            // Get repository object from user selection
            // It's supposed that the user has only one repo with that path (GH rule)
            GitHubInfo.selectedRepo = localReposList.First(r => r.FullName == selectionString);

            // Initialize process to get repo's content
            Task<SortedDictionary<string, string>> repoContentTask =
                GitHubInfo.GetRepoContentAsync(GitHubInfo.selectedRepo, ".dyn");

            // Get async result
            repoContent = await repoContentTask;

            BrowserEngine.PopulateTree(repoContent, filesTree);
        }

        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            var tree = sender as TreeView;

            if (tree.SelectedItem is string)
            {
                // Convert to actual string
                string selectedItem = tree.SelectedItem as string;

                // Get corresponding file name from dictionary
                string path = BrowserEngine.GetTreeItemNameInDict(selectedItem, repoContent);

                // Get file's uri from dictionary using path/key
                string uri = BrowserEngine.GetUriFromDict(repoContent, path);

                // Assemble download path
                string fName = BrowserEngine.GenerateFileName(selectedItem);

                // Download file from URI at file location just defined
                BrowserEngine.DownlodFileAtLocation(uri, fName);

                // Pass path to downloaded file to main Dynamo method
                toOpen = fName;

                // And close window
                Close();
            }
        }
    }
}
