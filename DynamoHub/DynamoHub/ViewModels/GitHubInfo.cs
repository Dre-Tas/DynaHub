using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaHub.ViewModels
{
    class GitHubInfo
    {
        internal static bool gotRepos = false;

        // Get all repositories of user [central storage]
        internal static async Task<IReadOnlyList<Repository>> GetUserReposAsync()
        {
            // Get users repos from GH
            IReadOnlyList<Repository> userRepos =
                await GitHubConnection.client.Repository.GetAllForCurrent();

            gotRepos = true;

            return userRepos;
        }

        // Initialise selection variable
        internal static Repository selectedRepo;

        #region ReviewCodeHere
        // This should probably be recursive to go through any folder at any level!

        // contents of the repo highest level
        internal static List<RepositoryContent> repoLevel = new List<RepositoryContent>();

        // List for all folders in repo to be queried
        internal static List<string> repoFolders = new List<string>();

        // Dictionary with both repo path and download_url
        internal static SortedDictionary<string, string> repoFiles =
            new SortedDictionary<string, string>();

        internal static async Task<SortedDictionary<string, string>> GetRepoContentAsync(
            Repository repository, string lookingFor)
        {
            // Clear lists not to repeat if user changes selection
            ClearPrevious();

            long repositoryID = repository.Id;
            // Get everything in repo at higher level of hierarchy
            // (in this case retrieves readme.MD, and folders)
            IReadOnlyList<RepositoryContent> allContent = null;
            allContent =
                    await GitHubConnection.client.Repository.Content.GetAllContents(repositoryID);
            // transform into a normal list that can be cleared
            foreach (var c in allContent)
            {
                repoLevel.Add(c);
            }

            foreach (RepositoryContent r in repoLevel)
            {
                if (r.Name.EndsWith(lookingFor))
                {
                    repoFiles.Add(r.Path, r.DownloadUrl);
                }
                else if (r.Type == "dir")
                {
                    repoFolders.Add(r.Path);
                }
            }

            // Check repo's subfolders
            foreach (string f in repoFolders)
            {
                // Get from specific path in repo
                IReadOnlyList<RepositoryContent> foldersLevel =
                    await GitHubConnection.client.Repository.Content.GetAllContents(
                        repositoryID, f);

                foreach (RepositoryContent s in foldersLevel)
                {
                    if (s.Name.EndsWith(lookingFor))
                    {
                        repoFiles.Add(s.Path, s.DownloadUrl);
                    }
                }
            }

            return repoFiles;
        }
        #endregion

        private static void ClearPrevious()
        {
            repoLevel.Clear();
            repoFolders.Clear();
            repoFiles.Clear();
        }
    }
}
