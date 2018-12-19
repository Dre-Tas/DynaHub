using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using Octokit;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace DynaHub
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class ViewExtension : IViewExtension
    {
        public string UniqueId => "7E85F38F-0A19-4F24-9E18-96845764780Q";
        public string Name => "DynaHub View Extension";

        // create client
        readonly GitHubClient client = new GitHubClient(new ProductHeaderValue("DynaHub"));

        /// <summary>
        /// Method that is called when Dynamo starts, but is not yet ready to be used.
        /// </summary>
        /// <param name="vsp">Parameters that provide references to Dynamo settings, version and extension manager.</param>
        public void Startup(ViewStartupParams vsp) { }

        /// <summary>
        /// Method that is called when Dynamo has finished loading and the UI is ready to be interacted with.
        /// </summary>
        /// <param name="vlp">
        /// Parameters that provide references to Dynamo commands, settings, events and
        /// Dynamo UI items like menus or the background preview. This object is supplied by Dynamo itself.
        /// </param>
        public void Loaded(ViewLoadedParams vlp)
        {
            // let's now create a completely top-level new menu item
            var extensionMenu = new MenuItem { Header = "DynaHub" };
            // and now we add a new sub-menu item that says hello when clicked
            var loginMenuItem = new MenuItem { Header = "Login to GitHub" };
            var browseMenuItem = new MenuItem { Header = "Browse GitHub" };

            var VM = vlp.DynamoWindow.DataContext as DynamoViewModel;

            loginMenuItem.Click += (sender, args) =>
            {
                // Create data tree to represent repo structure
                Views.Login l = new Views.Login();
                l.ShowDialog();

            };

            browseMenuItem.Click += async (sender, args) =>
            {
                if (GlobalSettings.user != "" && GlobalSettings.user != "username" && GlobalSettings.user != null &&
                GlobalSettings.repo != "" && GlobalSettings.repo != "reponame" && GlobalSettings.repo != null &&
                GlobalSettings.tok != "" && GlobalSettings.tok != "token" && GlobalSettings.tok != null)
                {
                    // Try to authenticate through personal access token
                    try
                    {
                        client.Credentials = new Credentials(GlobalSettings.tok);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("It seems like you've input the wrong token");
                        return;
                    }

                    // It only works with a simple repo structure (for now): repo > folders [NO SUBFOLDERS]
                    // Dictionary with both repo path and download_url
                    Dictionary<string, string> repoFiles = new Dictionary<string, string>();
                    // List for all folders in repo to be queried
                    List<string> repoFolders = new List<string>();

                    IReadOnlyList<RepositoryContent> repoLevel = null;

                    // Get content from GitHub at highest/repo level
                    try
                    {
                        repoLevel =
                            await client.Repository.Content.GetAllContents(
                                GlobalSettings.user,
                                GlobalSettings.repo);
                    }
                    catch
                    {
                        MessageBox.Show("I couldn't retrieve the contents of the repo",
                            "Error");
                        return;
                    }

                    // Check if there are .dyn file in outer level of repo
                    // And store all folders
                    try
                    {
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
                    }
                    catch (NullReferenceException)
                    {
                        // Do nothing. Already managed in catch above
                    }

                    // Check repo's subfolders
                    foreach (string f in repoFolders)
                    {
                        IReadOnlyList<RepositoryContent> foldersLevel =
                            await client.Repository.Content.GetAllContents(
                                GlobalSettings.user,
                                GlobalSettings.repo,
                                f);

                        foreach (RepositoryContent s in foldersLevel)
                        {
                            if (s.Name.EndsWith(".dyn"))
                            {
                                repoFiles.Add(s.Path, s.DownloadUrl);
                            }
                        }
                    }

                    // Create data tree to represent repo structure
                    Views.Browser b = new Views.Browser(repoFiles);
                    b.ShowDialog();

                    // Open downloaded file - path received from Browser
                    VM.OpenCommand.Execute(Views.Browser.toOpen);

                }
                else
                {
                    MessageBox.Show("You'll need to login before trying to access your files!");
                }
            };


            // Add main menu to Dynamo
            vlp.dynamoMenu.Items.Add(extensionMenu);
            // Add sub-menus to main menu
            extensionMenu.Items.Add(loginMenuItem);
            extensionMenu.Items.Add(browseMenuItem);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown()
        {
            GlobalSettings.DeleteTempFolder();
        }

        public void Dispose() { }

    }
}
