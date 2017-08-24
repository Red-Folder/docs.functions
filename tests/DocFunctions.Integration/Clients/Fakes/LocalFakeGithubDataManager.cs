using DocFunctions.Integration.Clients.Wrappers;
using DocFunctions.Integration.Models;
using DocFunctions.Lib.Models.Github;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeGithubDataManager
    {
        private string NO_PREVIOUS_COMMIT = Guid.NewGuid().ToString();

        private string _lastCommit;
        private Dictionary<string, string> _previousCommits = new Dictionary<string, string>();
        private Dictionary<string, ToBeBase> _fileHistory = new Dictionary<string, ToBeBase>();

        private AssetReader _assetReader;

        public LocalFakeGithubDataManager(AssetReader assetReader)
        {
            _lastCommit = NO_PREVIOUS_COMMIT;
            _assetReader = assetReader;
        }

        public WebhookData GetGithubWebhookData(ToBeCommitted commit)
        {
            var newCommitSha = Guid.NewGuid().ToString();
            var previousCommitSha = _lastCommit;

            PopulateFileHistory(commit, newCommitSha);

            var data = CreateWebhookData(commit, newCommitSha, previousCommitSha);

            _previousCommits.Add(newCommitSha.ToString(), _lastCommit);
            _lastCommit = newCommitSha.ToString();

            return data;
        }

        public string GetRawFile(string path, string commitSha)
        {
            var sourceFilename = GetSourceFilename(path, commitSha);

            return _assetReader.GetTextFile(sourceFilename);
        }

        public byte[] GetRawImageFile(string path, string commitSha)
        {
            var sourceFilename = GetSourceFilename(path, commitSha);

            return _assetReader.GetImageFile(sourceFilename);
        }

        private string CreateKey(string filename, string commitSha)
        {
            return $"{filename}:{commitSha}";
        }

        private string GetSourceFilename(string path, string commitSha)
        {
            var key = CreateKey(path, commitSha);
            if (_fileHistory.ContainsKey(key))
            {
                var record = _fileHistory[key];
                if (record != null && record is ToBeBaseWithSource)
                {
                    return (record as ToBeBaseWithSource).SourceFilename;
                }
            }

            var previousCommitSha = _previousCommits[commitSha];
            if (previousCommitSha == NO_PREVIOUS_COMMIT)
            {
                throw new Exception("Unable to find source file");
            }

            return GetSourceFilename(path, previousCommitSha);
        }

        private void PopulateFileHistory(ToBeCommitted commit, string newCommitSha)
        {
            commit.ToAdd.ForEach(x =>
            {
                _fileHistory.Add(CreateKey(x.RepoFilename, newCommitSha), x);
            });
            commit.ToModify.ForEach(x =>
            {
                _fileHistory.Add(CreateKey(x.RepoFilename, newCommitSha), x);
            });
            commit.ToDelete.ForEach(x =>
            {
                _fileHistory.Add(CreateKey(x.RepoFilename, newCommitSha), x);
            });
        }

        private WebhookData CreateWebhookData(ToBeCommitted commit, string newCommitSha, string previousCommitSha)
        {
            return new WebhookData
            {
                Commits = new List<Commit>
                {
                    new Commit
                    {
                        Added = CreateWebhookAddedData(commit, newCommitSha, previousCommitSha),
                        Modified = CreateWebhookModifiedData(commit, newCommitSha, previousCommitSha),
                        Removed = CreateWebhookRemovedData(commit, newCommitSha, previousCommitSha)
                    }
                }
            };
        }

        private List<Added> CreateWebhookAddedData(ToBeCommitted commit, String newCommitSha, String previousCommitSha)
        {
            return commit.ToAdd.Select(x => new Added
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = newCommitSha.ToString()
            }).ToList();
        }

        private List<Modified> CreateWebhookModifiedData(ToBeCommitted commit, String newCommitSha, String previousCommitSha)
        {
            return commit.ToModify.Select(x => new Modified
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = previousCommitSha.ToString()
            }).ToList();
        }

        private List<Removed> CreateWebhookRemovedData(ToBeCommitted commit, String newCommitSha, String previousCommitSha)
        {
            return commit.ToDelete.Select(x => new Removed
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = previousCommitSha.ToString()
            }).ToList();
        }
    }
}
