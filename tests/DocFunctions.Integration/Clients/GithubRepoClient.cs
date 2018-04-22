using DocFunctions.Integration.Clients.Wrappers;
using Octokit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Clients
{
    public class GithubRepoClient : IRepoClient
    {
        private string _username;
        private string _key;
        private string _repo;

        private AssetReader _assetReader;

        private DocFunctions.Integration.Models.ToBeCommitted _toBeCommitted = null;

        public GithubRepoClient(string username, string key, string repo, AssetReader assetReader)
        {
            _username = username;
            _key = key;
            _repo = repo;

            _assetReader = assetReader;

            StartCommit();
        }

        private void StartCommitIfNotAlreadyInProgress()
        {
            if (_toBeCommitted == null)
            {
                StartCommit();
            }
        }

        private void StartCommit()
        {
            _toBeCommitted = new DocFunctions.Integration.Models.ToBeCommitted();
        }

        private void EndCommit()
        {
            _toBeCommitted = null;
        }

        public void AddFileToCommit(string repoFilename, string sourceFilename)
        {
            StartCommitIfNotAlreadyInProgress();
            _toBeCommitted.ToAdd.Add(new Models.ToBeAdded(repoFilename, sourceFilename));
        }

        public void DeleteFileFromCommit(string repoFilename)
        {
            StartCommitIfNotAlreadyInProgress();
            _toBeCommitted.ToDelete.Add(new Models.ToBeDeleted(repoFilename));
        }

        public void ModifyFileInCommit(string repoFilename, string sourceFilename)
        {
            StartCommitIfNotAlreadyInProgress();
            _toBeCommitted.ToModify.Add(new Models.ToBeModified(repoFilename, sourceFilename));
        }

        public void PushCommit(string commitMessage)
        {
            var github = CreateClient();

            var parent = GetParent(github).Result;
            var latestCommit = GetLatestCommit(github, parent.Object.Sha).Result;
            var latestTree = GetFullTree(github, latestCommit.Tree.Sha).Result;
            var newTree = CreateCommitTree(github, latestTree).Result;
            var commit = CreateCommit(github, commitMessage, newTree.Sha, parent.Object.Sha).Result;

            AttachCommitToParent(github, commit.Sha).Wait();

            EndCommit();
        }

        private Octokit.GitHubClient CreateClient()
        {
            var credentials = new Octokit.Credentials(_username, _key);
            var connection = new Octokit.Connection(new Octokit.ProductHeaderValue("Red-Folder.DocFunctions.IntegrationTests"))
            {
                Credentials = credentials
            };
            return new Octokit.GitHubClient(connection);
        }

        private Task<Reference> GetParent(Octokit.GitHubClient github)
        {
            return github.Git.Reference.Get(_username, _repo, "heads/master");
        }

        private Task<Commit> GetLatestCommit(Octokit.GitHubClient github, string parentSha)
        {
            return github.Git.Commit.Get(_username, _repo, parentSha);
        }
        private Task<TreeResponse> GetFullTree(Octokit.GitHubClient github, string treeSha)
        {
            return github.Git.Tree.GetRecursive(_username, _repo, treeSha);
        }

        private Task<BlobReference> GetBlobReference(Octokit.GitHubClient github, string sourceFilename)
        {
            var blob = IsImage(sourceFilename) ? GetImageBlob(sourceFilename) : GetTextBlob(sourceFilename);
            return github.Git.Blob.Create(_username, _repo, blob);
        }

        private bool IsImage(string sourceFilename)
        {
            return (sourceFilename.ToLower().EndsWith(".png") || sourceFilename.ToLower().EndsWith(".jpg"));
        }

        private NewBlob GetImageBlob(string sourceFilename)
        {
            var imgBase64 = Convert.ToBase64String(_assetReader.GetImageFile(sourceFilename));
            return new NewBlob { Encoding = EncodingType.Base64, Content = (imgBase64) };
        }

        private NewBlob GetTextBlob(string sourceFilename)
        {
            var textContents = _assetReader.GetTextFile(sourceFilename);
            return new NewBlob { Encoding = EncodingType.Utf8, Content = textContents };
        }

        private void RemoveFromTree(NewTree tree, string filename)
        {
            var toRemove = tree.Tree.Where(x => x.Path.Equals(filename)).First();
            tree.Tree.Remove(toRemove);
        }

        private Task<TreeResponse> CreateCommitTree(Octokit.GitHubClient github, TreeResponse currentTree)
        {

            var newTree = CloneTree(currentTree);

            foreach (var toAdd in _toBeCommitted.ToAdd)
            {
                var blob = GetBlobReference(github, toAdd.SourceFilename).Result;
                newTree.Tree.Add(
                    new NewTreeItem
                    {
                        Path = toAdd.RepoFilename,
                        Mode = "100644",
                        Type = TreeType.Blob,
                        Sha = blob.Sha
                    });
            }

            foreach (var toModify in _toBeCommitted.ToModify)
            {
                var blob = GetBlobReference(github, toModify.SourceFilename).Result;
                newTree.Tree.Add(
                    new NewTreeItem
                    {
                        Path = toModify.RepoFilename,
                        Mode = "100644",
                        Type = TreeType.Blob,
                        Sha = blob.Sha
                    });
            }

            foreach (var toDelete in _toBeCommitted.ToDelete)
            {
                RemoveFromTree(newTree, toDelete.RepoFilename);
            }

            return CreateCommitTree(github, newTree);
        }

        private Task<TreeResponse> CreateCommitTree(Octokit.GitHubClient github, NewTree newTree)
        {
            return github.Git.Tree.Create(_username, _repo, newTree);
        }

        private Task<Commit> CreateCommit(Octokit.GitHubClient github, string message, string newTreeSha, string parentSha)
        {
            var newCommit = new NewCommit(message, newTreeSha, parentSha);
            return github.Git.Commit.Create(_username, _repo, newCommit);
        }

        private Task<Reference> AttachCommitToParent(Octokit.GitHubClient github, string commitSha)
        {
            return github.Git.Reference.Update(_username, _repo, "heads/master", new ReferenceUpdate(commitSha));
        }

        private NewTree CloneTree(TreeResponse original)
        {
            var newTree = new NewTree();
            original.Tree
                        .Where(x => x.Type != TreeType.Tree)
                        .Select(x => new NewTreeItem
                        {
                            Path = x.Path,
                            Mode = x.Mode,
                            Type = x.Type.Value,
                            Sha = x.Sha
                        })
                        .ToList()
                        .ForEach(x => newTree.Tree.Add(x));
            return newTree;
        }
    }
}
