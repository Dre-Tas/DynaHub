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
        // Store user inputs
        public Login()
        {
            InitializeComponent();
        }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.user = username.Text;
            GlobalSettings.repo = reponame.Text;
            GlobalSettings.tok = token.Password;

            Close();
        }
    }
}
