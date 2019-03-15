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

        private Uri validationUri = new Uri(
            "pack://application:,,,/DynaHub;component/Resources/verification.png",
            UriKind.RelativeOrAbsolute);

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            // Get user inputs
            tok = token.Password;

            // Start async to get user
            Task<User> getUser = GitHubConnection.LoginAsync(tok);

            // Pop up splash screen in the meantime
            SplashWindow splash = new SplashWindow(validationUri);

            // Await for user
            User user = await getUser;

            // Close splash screen
            splash.CloseSplash();

            // Greet user
            GitHubConnection.GreetUser(user);

            // If you go to this point, it was successful
            GlobalSettings.logged = true;

            // And close the log in form
            Close();
        }

        private async void EmailPassB_ClickAsync(object sender, RoutedEventArgs e)
        {
            GHemail = email.Text;
            GHpassword = password.Password;

            // Start async to get user
            Task<User> getUser = GitHubConnection.LoginAsync(GHemail, GHpassword);

            // Pop up splash screen in the meantime
            SplashWindow splash = new SplashWindow(validationUri);

            // Await for user
            User user = await getUser;

            // Close splash screen
            splash.CloseSplash();

            // Greet user
            GitHubConnection.GreetUser(user);
         
            // If you go to this point, it was successful
            GlobalSettings.logged = true;

            // And close the log in form
            Close();
        }
    }
}
