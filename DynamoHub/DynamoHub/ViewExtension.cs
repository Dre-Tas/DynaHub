using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using Octokit;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace DynamoHub
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class ViewExtension : IViewExtension
    {
        public string UniqueId => "7E85F38F-0A19-4F24-9E18-96845764780Q";
        public string Name => "DynamoHub View Extension";

        // create client
        readonly GitHubClient client = new GitHubClient(new ProductHeaderValue("DynamoHub"));

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
            var extensionMenu = new MenuItem { Header = "DynamoHub" };
            // and now we add a new sub-menu item that says hello when clicked
            var pullMenuItem = new MenuItem { Header = "Browse GitHub" };

            var VM = vlp.DynamoWindow.DataContext as DynamoViewModel;

            pullMenuItem.Click += async (sender, args) =>
            {
                // Authenticate through personal access token
                client.Credentials = new Credentials(GitHubConnection.token);

                // Name of the repo
                // TODO: put as user input
                string repoName = "DynamoRepo";

                // It only works with a simple repo structure (for now): repo > folders [NO SUBFOLDERS]
                // Dictionary with both repo path and download_url
                Dictionary<string, string> repoFiles = new Dictionary<string, string>();
                // List for all folders in repo to be queried
                List<string> repoFolders = new List<string>();

                // Get content from GitHub at highest/repo level
                IReadOnlyList<RepositoryContent> repoLevel =
                    await client.Repository.Content.GetAllContents(
                        "ridleyco",
                        repoName);

                // Check if there are .dyn file in outer level of repo
                // And store all folders
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
                    IReadOnlyList<RepositoryContent> foldersLevel =
                        await client.Repository.Content.GetAllContents(
                            "ridleyco",
                            repoName,
                            f);

                    foreach (RepositoryContent s in foldersLevel)
                    {
                        if (s.Name.EndsWith(".dyn"))
                        {
                            repoFiles.Add(s.Path, s.DownloadUrl);
                        }
                    }
                }

                // TODO - exceptions + all files

                // Create data tree to represent repo structure
                Views.Browser b = new Views.Browser(repoFiles);
                b.ShowDialog();

                // Open downloaded file - path received from Browser
                VM.OpenCommand.Execute(Views.Browser.toOpen);
            };
            extensionMenu.Items.Add(pullMenuItem);
            // Add menu to Dynamo
            vlp.dynamoMenu.Items.Add(extensionMenu);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown() { }

        public void Dispose() { }

    }
}
