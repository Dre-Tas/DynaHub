using DynaHub.ViewModels;
using Octokit;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ProtoCore.AST.ImperativeAST;

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
            // Grab string stored in Windows Credential Manager
            string token = Helpers.GetTokenFromCredManager();

            // Try the config file
            if (File.Exists(IniConfigInfo.configFilePath))
            {
                // If it exists, get the token stored there
                string configToken = IniConfigInfo.GetToken();

                // If the token in config is different from the one in cred manager, user needs to resolve conflict
                if (token != null && token != configToken)
                {
                    foundCreds.Text = "There are two conflicting tokens in the config file and on Credential Manager. You may want to fix that first.";
                    return;
                }

                token = configToken;
            }

            // If it's still null after checking the config file
            if (token == null)
            {
                foundCreds.Text = "You don't have a token saved in your config file or Credential Manager.";
                return;
            }

            // Keep track if user has config file and/or decrypting dll
            bool configExists = File.Exists(IniConfigInfo.configFilePath);
            bool dllExists = false;
            if (configExists) dllExists = File.Exists(IniConfigInfo.GetDllPath());

            // Check if the user created a decryption dll and stored / right info in config file
            if (configExists && dllExists)
            {
                token = Helpers.DecryptToken(token);
            }

            if (token != null)
            {
                // Populate with decrypted token
                this.token.Password = token;
                foundCreds.Text = "I found the token in your config file or Credential Manager";

                if (!configExists)
                {
                    foundCreds.Text += ", but there was no config file containing decryption info. You'll be fine if you didn't encrypt the token.";
                }
                else if (!dllExists)
                {
                    foundCreds.Text += ", but there was no dll file containing decryption methods. You'll be fine if you didn't encrypt the token.";
                }
                // Add period to sentence if none of the conditions are satisfied
                else
                {
                    foundCreds.Text += ".";
                }
            }
            else
            {
                foundCreds.Text = "The config file is there but the info is wrong.";
            }
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

                GitHubConnection.ChangeLoginGreet();
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
