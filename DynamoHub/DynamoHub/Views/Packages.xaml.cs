using DynaHub.ViewModels;
using Dynamo.Wpf.Extensions;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DynaHub.Views
{
    /// <summary>
    /// Interaction logic for Packages.xaml
    /// </summary>
    public partial class Packages : Window
    {
        internal static ViewLoadedParams loadedParams;

        public Packages(ViewLoadedParams vlp)
        {
            InitializeComponent();
            loadedParams = vlp;

            // Add an initial item that just tells the user to select a repo
            ListBoxItem defaultItem = new ListBoxItem();
            defaultItem.Content = "Select a repository";

            packagesList.Items.Add(defaultItem);
        }

        private List<Repository> localReposList = new List<Repository>();

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
            // Let user know DynaHub is retrieving stuff
            packagesList.Items.Clear();

            ListBoxItem asyncItem = new ListBoxItem();
            asyncItem.Content = "...retrieving repos";

            packagesList.Items.Add(asyncItem);

            // Show name of selected repo in combobox
            string selectionString = null;
            selectionString = selectReposCB.SelectedItem.ToString();

            selectReposCB.Text = selectionString;

            // Get repository object from user selection
            // It's supposed that the user has only one repo with that path (GH rule)
            GitHubInfo.selectedRepo = localReposList.Where(r => r.FullName == selectionString).First();

            // Initialize process to get repo's content
            Task<SortedDictionary<string, string>> repoContentTask =
                GitHubInfo.GetRepoContentAsync(GitHubInfo.selectedRepo, ".zip");
            //                  Packages are uploaded as zipped folders  ^

            // Get async result
            repoContent = await repoContentTask;

            // Populate ListBox
            //GetPackages.PopulateListBox(repoContent, packagesList);
        }

        private void GetPacks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if there are packages to be uninstalled before installing all the right packages
                List<string> uninstallFirst = GetPackages.CheckUninstall(repoContent);

                // If there are, uninstall them
                if (uninstallFirst.Count != 0)
                {
                    GetPackages.UninstallPackages(uninstallFirst);
                }
                else
                {
                    // Pop up splash screen in the meantime
                    SplashWindow downloadingSplash =
                        new SplashWindow(GlobalSettings.downloadingUri);

                    // Once all the duplicates have been uninstalled > install ALL packages
                    GetPackages.InstallPackages(repoContent);

                    // Close splash screen
                    downloadingSplash.CloseSplash();

                    // Notify user
                    MessageBox.Show("You now have all the right packages.", "Success");
                }

                // Close window
                Close();

                // Close Dynamo to get changes to packages
                GetPackages.CloseDynamo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while downloading");
            }
        }
    }
}
