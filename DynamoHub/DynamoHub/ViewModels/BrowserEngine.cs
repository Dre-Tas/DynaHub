using DynaHub.ViewModels;
using DynaHub.Views;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynaHub.ViewModels
{
    class BrowserEngine
    {
        internal static async Task<IReadOnlyList<Repository>> GetUserReposAsync()
        {
            // Get users repos from GH
            IReadOnlyList<Repository> userRepos =
                await GitHubConnection.client.Repository.GetAllForCurrent();
            
            return userRepos;
        }

        // contents of the repo highest level
        public static IReadOnlyList<RepositoryContent> repoLevel = null;

        internal static async Task<IReadOnlyList<RepositoryContent>> GetRepoContentAsync()
        {
            // Get everything in repo at higher level of hierarchy
            // (in this case retrieves readme.MD, and folders)
            repoLevel =
                await GitHubConnection.client.Repository.Content.GetAllContents(
                    Browser.selectedRepo.Id);

            return repoLevel;
        }

        // Dictionary with both repo path and download_url
        internal static Dictionary<string, string> repoFiles = new Dictionary<string, string>();

        // List for all folders in repo to be queried
        internal static List<string> repoFolders = new List<string>();

        //internal static async Task PopulateTreeViewAsync()
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

        //    // Check repo's subfolders
        //    foreach (string f in repoFolders)
        //    {
        //        IReadOnlyList<RepositoryContent> foldersLevel =
        //            await GitHubConnection.client.Repository.Content.GetAllContents(repo.Id, f);

        //        foreach (RepositoryContent s in foldersLevel)
        //        {
        //            if (s.Name.EndsWith(".dyn"))
        //            {
        //                repoFiles.Add(s.Path, s.DownloadUrl);
        //            }
        //        }
        //    }

        //}


    }
}
