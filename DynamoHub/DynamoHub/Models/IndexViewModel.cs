using System.Collections.Generic;
using Octokit;

namespace DynamoHub
{
    public class IndexViewModel
    {
        public IndexViewModel(IEnumerable<Repository> repositories)
        {
            Repositories = repositories;
        }

        public IEnumerable<Repository> Repositories { get; private set; }
    }
}
