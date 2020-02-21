using DynaHub.ViewModels;
using Octokit;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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

        private void token_GotFocus(object sender, RoutedEventArgs e)
        {
            // Check if user stored token in Windows credential Manager
            string GHToken = Helpers.GetTokenFromCredManager();

            if (GHToken == null)
            {
                foundCreds.Text = "You don't have a token saved in your Credential Manager";
                return;
            }

            // If decrypting dll exists then the user encrypted the token in Credential Manager
            if (System.IO.File.Exists(GlobalSettings.decryptionDll))
                GHToken = Helpers.DecryptToken(GHToken);

            // Populate with decrypted token
            token.Password = GHToken;
            foundCreds.Text = "I found the token in your Credential Manager";
        }
        #endregion

        // Store login credentials
        private static string GHtok = null;
        internal static string GHemail = null;
        internal static string GHpassword = null;

        private async void Login_ClickAsync(object sender, RoutedEventArgs e)
        {
            // Get user inputs
            GHtok = token.Password;
            GHemail = email.Text;
            GHpassword = Password.Password;

            User user = null;

            // Check it's different from default values
            if (GHtok != "")
            {
                user = await GitHubConnection.LoginAsync(GHtok);
            }
            else if (GHemail != "email address" && GHpassword != "password")
            {
                user = await GitHubConnection.LoginAsync(GHemail, GHpassword);
            }

            // Pop up splash screen in the meantime
            SplashWindow verificationSplash = new SplashWindow(GlobalSettings.validationUri);

            // Close splash screen
            verificationSplash.CloseSplash();

            if (user != null)
            {
                // Pop up splash screen in the meantime
                SplashWindow verifiedSplash = new SplashWindow(GlobalSettings.validatedUri);

                // Wait x seconds showing message
                System.Threading.Thread.Sleep(2000);

                // Close splash screen
                verifiedSplash.CloseSplash();

                // Greet user
                GitHubConnection.GreetUser(user);

                // If you go to this point, it was successful
                GlobalSettings.logged = true;
            }
            else
            {
                Helpers.ErrorMessage("It seems like you've input the wrong credentials/token");
                GlobalSettings.logged = false;
            }

            // And close the log in form
            Close();
        }
    }
}
