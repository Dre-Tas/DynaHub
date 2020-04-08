using DynaHub.ViewModels;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using Octokit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;


namespace DynaHub
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class ViewExtension : IViewExtension
    {
        public string UniqueId => "7E85F38F-0A19-4F24-9E18-96845764780Q";
        public string Name => "DynaHub View Extension";
        
        /// <summary>
        /// Method that is called when Dynamo starts, but is not yet ready to be used.
        /// </summary>
        /// <param name="vsp">Parameters that provide references to Dynamo settings, version and extension manager.</param>
        public void Startup(ViewStartupParams vsp)
        {
        }

        internal static MenuItem loginMenuItem;

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
            MenuItem extensionMenu = new MenuItem { Header = "DynaHub" };
            // and now we add a new sub-menu item that says hello when clicked
            loginMenuItem = new MenuItem { Header = "Login to GitHub" };
            var browseMenuItem = new MenuItem { Header = "Browse GitHub" };
            var packagesMenuItem = new MenuItem { Header = "Get packages" };
            var logoutMenuItem = new MenuItem { Header = "Logout" };

            var VM = vlp.DynamoWindow.DataContext as DynamoViewModel;

            // Define Login menu option
            loginMenuItem.Click += (sender, args) =>
            {
                if (!GlobalSettings.logged)
                {
                    // Create data tree to represent repo structure
                    Views.Login l = new Views.Login();
                    l.ShowDialog();
                }
                else
                {
                    Helpers.ErrorMessage("You are already logged in.");
                }
            };

            // Define Browse menu option
            browseMenuItem.Click += (sender, args) =>
            {
                if (GlobalSettings.logged)
                {
                    // Create data tree to represent repo structure
                    Views.Browser b = new Views.Browser();
                    b.ShowDialog();

                    // Open downloaded file - path received from Browser
                    VM.OpenCommand.Execute(Views.Browser.toOpen);
                }
                else
                {
                    Helpers.ErrorMessage("You'll need to login before trying to access your files!");
                }
            };

            // Define Get Packages menu option
            packagesMenuItem.Click += (sender, args) =>
            {
                if (GlobalSettings.logged)
                {
                    // Download packages to Dynamo's packages folder
                    Views.Packages p = new Views.Packages(vlp);
                    p.ShowDialog();
                }
                else
                {
                    Helpers.ErrorMessage("You'll need to login before downloading the packages!");
                }
            };

            // Define Get Packages menu option
            logoutMenuItem.Click += (sender, args) =>
            {
                if (GlobalSettings.logged)
                {
                    // Logout of current GitHub login

                    GitHubConnection.Logout();

                    //Views.Packages o = new Views.Packages(vlp);
                    //o.ShowDialog();
                }
                else
                {
                    Helpers.ErrorMessage("You can't logout if you're not logged in");
                }
            };

            // Add main menu to Dynamo
            vlp.dynamoMenu.Items.Add(extensionMenu);
            // Add sub-menus to main menu
            extensionMenu.Items.Add(loginMenuItem);
            extensionMenu.Items.Add(browseMenuItem);
            extensionMenu.Items.Add(packagesMenuItem);
            extensionMenu.Items.Add(logoutMenuItem);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown()
        {
            // When closing Dynamo, delete all the temporary files
            GlobalSettings.DeleteTempFolder();
        }

        public void Dispose() { }

    }
}
