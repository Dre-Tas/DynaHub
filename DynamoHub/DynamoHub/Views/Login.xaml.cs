using DynaHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        // Initialize
        public Login()
        {
            InitializeComponent();
        }

        #region UIFunct
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void field_GotFocus(object sender, RoutedEventArgs e)
        {
            // Get calling element
            TextBox s = sender as TextBox;

            // Set calling element to empty
            s.Text = "";
        }

        private void field_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox s = sender as TextBox;

            // Set to initial value only if user didn't input any value
            if (s.Text == "")
            {
                s.Text = s.Name;
            }
        }

        private void pass_GotFocus(object sender, RoutedEventArgs e)
        {
            // Get calling element
            PasswordBox p = sender as PasswordBox;

            // Set calling element to empty
            p.Password = "";
        }

        private void pass_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox p = sender as PasswordBox;

            // Set to initial value only if user didn't input any value
            if (p.Password == "")
            {
                p.Password = p.Name;
            }
        }

        private void button_MouseUp(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        // Store login credentials
        // if login with token
        private static string tok = null;
        // if login with GH account
        internal static string GHemail = null;
        internal static string GHpassword = null;

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            // Get user inputs
            tok = token.Password;

            VerificationMessage();

            // Connect to GH
            await GitHubConnection.LoginAsync(tok);

            #region ThisShouldntBeHere
            //// Get content from GitHub at highest/repo level
            //try
            //{
            //    repoLevel =
            //        await client.Repository.Content.GetAllContents(
            //            GlobalSettings.user,
            //            GlobalSettings.repo);
            //}
            //catch
            //{
            //    MessageBox.Show("I couldn't find anything with those credentials.",
            //        "Error",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    return;
            //}

            //try
            //{
            //    foreach (RepositoryContent r in repoLevel)
            //    {
            //        if (r.Name.EndsWith(".dyn"))
            //        {
            //            repoFiles.Add(r.Path, r.DownloadUrl);
            //        }
            //        else if (r.Type == "dir")
            //        {
            //            repoFolders.Add(r.Path);
            //        }
            //    }
            //}
            //catch (NullReferenceException)
            //{
            //    // Do nothing. Already managed in catch above
            //}

            //try
            //{
            //    // Check repo's subfolders
            //    foreach (string f in repoFolders)
            //    {
            //        IReadOnlyList<RepositoryContent> foldersLevel =
            //            await client.Repository.Content.GetAllContents(
            //                GlobalSettings.user,
            //                GlobalSettings.repo,
            //                f);

            //        foreach (RepositoryContent s in foldersLevel)
            //        {
            //            if (s.Name.EndsWith(".dyn"))
            //            {
            //                repoFiles.Add(s.Path, s.DownloadUrl);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(),
            //        "Error",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //}
            //// Notify user
            //AutoClosingMessageBox.Show("The login was successful.", "Success", 3000);
            #endregion ThisShouldntBeHere

            // If you go to this point, it was successful
            GlobalSettings.logged = true;
            // And close the log in form
            Close();
        }

        //private async void GetRepos_ClickAsync(object sender, RoutedEventArgs e)
        //{
        //    GlobalSettings.email = email.Text;
        //    GlobalSettings.password = password.Password;

        //    // Try to authenticate through personal access token
        //    try
        //    {
        //        client.Credentials = new Credentials(
        //            GlobalSettings.email,
        //            GlobalSettings.password);
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("It seems like you've input the wrong email or password",
        //            "Error",
        //            MessageBoxButton.OK,
        //            MessageBoxImage.Error);
        //        return;
        //    }

        //    repoName.Text = "...retrieving repos";

        //    try
        //    {
        //        User user = await client.User.Current();

        //        var repos = await client.Repository.GetAllForCurrent();
        //        foreach (var r in repos.ToList())
        //        {
        //            repoName.Items.Add(r.FullName);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        repoName.Text = "wrong credentials?";
        //        return;
        //    }
        //    repoName.Text = "pick repo";
        //}

        private async void EmailPassB_ClickAsync(object sender, RoutedEventArgs e)
        {
            GHemail = email.Text;
            GHpassword = password.Password;
            //GlobalSettings.repoName = repoName.Text;

            VerificationMessage();

            await GitHubConnection.LoginAsync(GHemail, GHpassword);

            #region ThisShouldntBeHere
            //// Get content from GitHub at highest/repo level
            //try
            //{
            //    repoLevel = await client.Repository.Content.GetAllContents(
            //                GlobalSettings.repoName.Split('/')[0],
            //                GlobalSettings.repoName.Split('/')[1]);
            //}
            //catch
            //{
            //    MessageBox.Show("I couldn't find anything with those credentials.",
            //        "Error",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    return;
            //}

            //try
            //{
            //    foreach (RepositoryContent r in repoLevel)
            //    {
            //        if (r.Name.EndsWith(".dyn"))
            //        {
            //            repoFiles.Add(r.Path, r.DownloadUrl);
            //        }
            //        else if (r.Type == "dir")
            //        {
            //            repoFolders.Add(r.Path);
            //        }
            //    }
            //}
            //catch (NullReferenceException)
            //{
            //    // Do nothing. Already managed in catch above
            //}

            //try
            //{
            //    // Check repo's subfolders
            //    foreach (string f in repoFolders)
            //    {
            //        IReadOnlyList<RepositoryContent> foldersLevel =
            //            await client.Repository.Content.GetAllContents(
            //                GlobalSettings.repoName.Split('/')[0],
            //                GlobalSettings.repoName.Split('/')[1],
            //                f);

            //        foreach (RepositoryContent s in foldersLevel)
            //        {
            //            if (s.Name.EndsWith(".dyn"))
            //            {
            //                repoFiles.Add(s.Path, s.DownloadUrl);
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Something went wrong",
            //        "Error",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    return;
            //}

            //// And close the log in form
            //Close();
            //// Notify user
            //AutoClosingMessageBox.Show("The login was successful.", "Success", 3000);
            #endregion

            // If you go to this point, it was successful
            GlobalSettings.logged = true;

            // And close the log in form
            Close();
        }

        private void VerificationMessage()
        {
            AutoClosingMessageBox.Show("Verifying credentials...",
                "Logging in",
                3000);
        }
    }
}
