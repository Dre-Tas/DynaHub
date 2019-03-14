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
            if (user == null)
            {
                Helpers.ErrorMessage("It seems like you've input the wrong token");
            }
            else
            {
                // Get user and greet user to check if input token is correct
                Helpers.SuccessMessage($"You logged in successfully {user.Name}!");
            }
        }
    }
}
