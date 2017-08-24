using DocFunctions.Integration.Clients.Wrappers;
using DocFunctions.Integration.Models;
using DocFunctions.Lib.Models.Github;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var newCommitSha = Guid.NewGuid();
            var previousCommitSha = _lastCommit;

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

        private string CreateKey(string filename, Guid commitSha)
        {
            return CreateKey(filename, commitSha.ToString());
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

    }
}
