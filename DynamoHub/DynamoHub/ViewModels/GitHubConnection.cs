using DynaHub.Views;
using Octokit;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DynaHub.ViewModels
{
    class GitHubConnection
    {
        // GitHub client
        internal static readonly GitHubClient client =
            new GitHubClient(new ProductHeaderValue("DynaHub"));

        // GitHub user
        internal static User user;

        internal static async Task<User> LoginAsync(string GHtoken)
        {
            // Authenticate through personal access token
            client.Credentials = new Credentials(GHtoken);

            try
            {
                user = await client.User.Current();
            }
            catch
            {
                user = null;
            }

            return user;
        }

        internal static async Task<User> LoginAsync(string GHemail, string GHpassword)
        {
            // Authenticate through personal access token
            client.Credentials = new Credentials(GHemail, GHpassword);

            try
            {
                user = await client.User.Current();
            }
            catch
            {
                user = null;
            }

            return user;
        }

        internal static void GreetUser(User user)
        {
            string baseGreet = "You logged in successfully";
            // Get user and greet user to check if input token is correct
            if (user.Name != null)
            {
                Helpers.SuccessMessage($"{baseGreet}, {user.Name}!"); 
            }
            else
            {
                Helpers.SuccessMessage($"{baseGreet}, {user.Type}!");
            }
        }

        internal static void ChangeLoginGreet()
        {
            string baseLoginGreet = "Hello";
            if (user.Name != null && GlobalSettings.logged)
            {
                ViewExtension.loginMenuItem.Header = $"{baseLoginGreet}, {user.Name}!";
            }
            else
            {
                ViewExtension.loginMenuItem.Header = $"{baseLoginGreet}, {user.Type}";
            }
        }

        internal static void Logout()
        {
            // Allow user to login again by changing logged value (no real login method method)
            GlobalSettings.logged = false;
            ViewExtension.loginMenuItem.Header = "Login to GitHub";

            // Notify user
            Helpers.SuccessMessage($"You logged out of {user.Login}");
            
            // Set user back to null as sanity check
            user = null;
        }
    }
}
