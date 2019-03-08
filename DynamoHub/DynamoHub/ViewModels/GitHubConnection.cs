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


        internal static async Task LoginAsync(string GHtoken)
        {
            // Authenticate through personal access token
            client.Credentials = new Credentials(GHtoken);

            try
            {
                User user = await client.User.Current();

                // Get user and greet user to check if input token is correct
                Helpers.SuccessMessage($"You logged in successfully {user.Name}!");
            }
            catch (Exception)
            {
                Helpers.ErrorMessage("It seems like you've input the wrong token");
            }
        }

        internal static async Task LoginAsync(string GHemail, string GHpassword)
        {
            // Authenticate through personal access token
            client.Credentials = new Credentials(GHemail, GHpassword);

            try
            {
                User user = await client.User.Current();

                // Get user and greet user to check if input token is correct
                Helpers.SuccessMessage($"You logged in successfully {user.Name}!");
            }
            catch (Exception)
            {
                Helpers.ErrorMessage("It seems like you've input the wrong token");
            }
        }
    }
}
