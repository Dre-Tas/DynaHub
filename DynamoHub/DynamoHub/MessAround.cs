using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DynamoHub
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class MessAround : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "DynamoHub View Extension";

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
            pullMenuItem.Click += (sender, args) =>
            {
                MessageBox.Show("Hello " + Environment.UserName);
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
