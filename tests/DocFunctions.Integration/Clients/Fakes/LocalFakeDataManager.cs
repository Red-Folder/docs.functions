using DocFunctions.Lib.Models.Github;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Specialized;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeDataManager
    {
        private Guid NO_PREVIOUS_COMMIT = Guid.NewGuid();

        private OrderedDictionary _commits = new OrderedDictionary();

        public WebhookData GetGithubWebhookData(Models.Commit commit)
        {
            var newCommitSha = Guid.NewGuid();
            var previousCommitSha = _commits.Count == 0 ? NO_PREVIOUS_COMMIT : _commits[_commits.Count - 1];

            _commits.Add(newCommitSha, commit);

            var toBeAdded = commit.ToAdd.Select(x => new Added
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = newCommitSha.ToString()
            }).ToList();
            var toBeModified = commit.ToModify.Select(x => new Modified
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = previousCommitSha.ToString()
            }).ToList();
            var toBeDeleted = commit.ToDelete.Select(x => new Removed
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = previousCommitSha.ToString()
            }).ToList();

            WebhookData data = new WebhookData
            {
                Commits = new List<Commit>
                {
                    new Commit
                    {
                        Added = toBeAdded,
                        Modified = toBeModified,
                        Removed = toBeDeleted
                    }
                }
            };

            return data;
        }
    }
}
