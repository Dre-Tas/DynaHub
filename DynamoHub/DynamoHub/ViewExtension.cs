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

        // Token generated in GitHub
        // TODO: DON'T HARDCODE IT! Prompt user to insert
        private readonly string token = "192f2975ee9073ab52d80de574da4840ddab38ce";
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
            var pullMenuItem = new MenuItem { Header = "Pull Graph" };

            var VM = vlp.DynamoWindow.DataContext as DynamoViewModel;

            pullMenuItem.Click += async (sender, args) =>
            {
                // Authenticate through personal access token
                client.Credentials = new Credentials(token);

                //var codeSearch = new SearchCodeRequest()
                //{
                //    Extension = ".dyn",
                //    Repos = new RepositoryCollection
                //    {
                //        "ridleyco/DynamoRepo"
                //    }
                //};

                //var codeResult = await client.Search.SearchCode(codeSearch);

                List<string> apis = new List<string>();
                List<string> names = new List<string>();

                //foreach (var item in codeResult.Items)
                //{
                //    apis.Add(item.Url);
                //    names.Add(item.Name);
                //    //MessageBox.Show(item.GitUrl);
                //    //MessageBox.Show(item.HtmlUrl);
                //    //MessageBox.Show(item.Url);
                //}

                // POC - get template from GitHub
                var temp = await client.Repository.Content.GetAllContents(
                    "ridleyco",
                    "DynamoRepo",
                    "templates/TheMightyDynamoTemplate.dyn");
                // Extract name and uri
                foreach (var item in temp)
                {
                    names.Add(item.Name);
                    apis.Add(item.DownloadUrl);
                }

                // Instantiate web client to download file
                WebClient wc = new WebClient();

                // POC - get first file's name and uri
                string uri = apis[0];
                // TODO: Change location of file - use Dynamo folders to be sure it exists
                string folder = @"C:\temp\DynamoHub\";
                string fName = folder + names[0];

                // Download file locally
                MessageBox.Show($"I'm downloading {uri} to {fName}");
                wc.DownloadFile(uri, fName);
                MessageBox.Show("Downloaded! Opening now...");

                // Open downloaded file
                VM.OpenCommand.Execute(fName);
                MessageBox.Show("tadaaa");
            };
            extensionMenu.Items.Add(pullMenuItem);
            // finally, we need to add our menu to Dynamo
            vlp.dynamoMenu.Items.Add(extensionMenu);
        }


        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown() { }

        public void Dispose() { }

    }
}
