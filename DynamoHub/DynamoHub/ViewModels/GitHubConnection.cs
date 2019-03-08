using Octokit;

namespace DynaHub.ViewModels
{
    class GitHubConnection
    {
        // GitHub client
        internal static readonly GitHubClient client =
            new GitHubClient(new ProductHeaderValue("DynaHub"));

        // Store login credentials
        // if login with token
        internal static string userEmail = null;
        internal static string tokenPassword = null;
        internal static string repo = null;

    }
}
