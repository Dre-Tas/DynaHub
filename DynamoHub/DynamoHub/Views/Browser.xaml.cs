using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


namespace DynamoHub.Views
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : Window
    {
        // Get dict from main Dynamo method
        Dictionary<string, string> allPaths = new Dictionary<string, string>();
        // Create variable to pass to main Dynamo method
        public static string toOpen = null;

        public Browser(Dictionary<string, string> files)
        {
            InitializeComponent();
            // Get dict from main dynamo method
            allPaths = files;
        }

        // General functioning from here: https://www.dotnetperls.com/treeview-wpf
        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            // Sort alphabetically paths of dynamo files
            List<string> keysList = allPaths.Keys.ToList();
            keysList.Sort();

            // Get unique folders to define headers of treeview
            HashSet<string> headers = new HashSet<string>();
            foreach (string p in keysList)
            {
                headers.Add(p.Split('/').First());
            }

            // Loop all files in folders and build treeview
            foreach (string h in headers)
            {
                // Create headers
                TreeViewItem tvItem = new TreeViewItem();
                tvItem.Header = h;

                // List to store all files in each folder
                List<string> filesInFolder = new List<string>();

                // Get files which path is the same of the header
                foreach (var i in keysList.Where(x => x.StartsWith(h)))
                {
                    filesInFolder.Add(i.ToString().Split('/').Last());
                }

                // The itmes at the lower level are names of the files without the folder
                tvItem.ItemsSource = filesInFolder;

                // Add them to the treeview
                var tree = sender as TreeView;
                tree.Items.Add(tvItem);
            }
        }

        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            var tree = sender as TreeView;

            // ... Determine type of SelectedItem.
            if (tree.SelectedItem is TreeViewItem)
            {                
                // ... Handle a TreeViewItem.
                var item = tree.SelectedItem as TreeViewItem;
                MessageBox.Show("You selected this folder: " +
                    item.Header.ToString() +
                    Environment.NewLine +
                    "Please select a file in that folder to open it.");
            }

            // Else if it's the child element
            else if (tree.SelectedItem is string)
            {
                List<string> keysList = allPaths.Keys.ToList();

                string path = null;

                // get path from name of the file selected by the user
                foreach (string k in keysList)
                {
                    if (k.Contains(tree.SelectedItem.ToString()))
                    {
                        path = k;
                    }
                }

                // Instantiate web client to download file
                WebClient wc = new WebClient();

                // Get file's uri from dictionary using path/key
                string uri = allPaths[path];
                // Define where to download gh file
                // TODO: Change location of file - use Dynamo folders to be sure it exists
                string folder = @"C:\temp\DynamoHub\";
                // Assemble download path
                string fName = folder + tree.SelectedItem.ToString();

                // Download file locally
                wc.DownloadFile(uri, fName);
                AutoClosingMessageBox.Show("Downloaded! Opening now...", "Success!", 2000);

                // Pass path to downloaded file to main Dynamo method
                toOpen = fName;
                // And close window
                Close();
            }
        }
    }
}
