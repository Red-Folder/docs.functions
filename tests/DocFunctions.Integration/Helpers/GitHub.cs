﻿using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Helpers
{
    public class GitHub
    {
        private string _username;
        private string _key;
        private string _repo;
        private string _blogname;

        public GitHub(string username, string key, string repo, string blogname)
        {
            _username = username;
            _key = key;
            _repo = repo;
            _blogname = blogname;
        }

        public void CreateTestBog()
        {
            CreateTestBogAsync().Wait();
        }

        public async Task CreateTestBogAsync()
        {
            var github = CreateClient();

            var parent = await GetParent(github);
            var latestCommit = await GetLatestCommit(github, parent.Object.Sha);
            var latestTree = await GetFullTree(github, latestCommit.Tree.Sha);

            var imgBlogRef = await GetImageBlogReference(github, @"Assets\Image.png");
            var metaBlogRef = await GetMetaBlogReference(github, @"Assets\blog.json");
            var mdBlogRef = await GetMarkdownBlogReference(github, @"Assets\blog.md");

            var newTree = await CreateCommitTree(github, latestTree, imgBlogRef, metaBlogRef, mdBlogRef);
            var commit = await CreateCommit(github, "Created test blog", newTree.Sha, parent.Object.Sha);

            await AttachCommitToParent(github, commit.Sha);
        }

        public void DeleteTestBog()
        {
            DeleteTestBogAsync().Wait();
        }

        public async Task DeleteTestBogAsync()
        {
            var github = CreateClient();

            var parent = await GetParent(github);
            var latestCommit = await GetLatestCommit(github, parent.Object.Sha);
            var latestTree = await GetFullTree(github, latestCommit.Tree.Sha);

            var workingTree = CloneTree(latestTree);
            RemoveFromTree(workingTree, $"{_blogname}/Image.png");
            RemoveFromTree(workingTree, $"{_blogname}/blog.json");
            RemoveFromTree(workingTree, $"{_blogname}/blog.md");

            var newTree = await CreateCommitTree(github, workingTree);
            var commit = await CreateCommit(github, "Deleted test blog", newTree.Sha, parent.Object.Sha);

            await AttachCommitToParent(github, commit.Sha);
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

        private Task<BlobReference> GetImageBlogReference(Octokit.GitHubClient github, string filename)
        {
            var imgBase64 = Convert.ToBase64String(File.ReadAllBytes(filename));
            // Create image blob
            var imgBlob = new NewBlob { Encoding = EncodingType.Base64, Content = (imgBase64) };
            return github.Git.Blob.Create(_username, _repo, imgBlob);
        }

        private Task<BlobReference> GetMetaBlogReference(Octokit.GitHubClient github, string filename)
        {

            var metaContents = File.ReadAllText(filename).Replace("[BLOGNAME]", _blogname);
            var metaBlob = new NewBlob { Encoding = EncodingType.Utf8, Content = metaContents };
            return github.Git.Blob.Create(_username, _repo, metaBlob);
        }

        private Task<BlobReference> GetMarkdownBlogReference(Octokit.GitHubClient github, string filename)
        {
            var mdContents = File.ReadAllText(filename).Replace("[BLOGNAME]", _blogname);
            var mdBlob = new NewBlob { Encoding = EncodingType.Utf8, Content = mdContents };
            return github.Git.Blob.Create(_username, _repo, mdBlob);
        }

        private void RemoveFromTree(NewTree tree, string filename)
        {
            var toRemove = tree.Tree.Where(x => x.Path.Equals(filename)).First();
            tree.Tree.Remove(toRemove);
        }

        private Task<TreeResponse> CreateCommitTree(Octokit.GitHubClient github, TreeResponse currentTree, BlobReference imgBlobRef, BlobReference metaBlobRef, BlobReference mdBlobRef)
        {

            var newTree = CloneTree(currentTree);
            newTree.Tree.Add(new NewTreeItem { Path = $"{_blogname}/Image.png", Mode = "100644", Type = TreeType.Blob, Sha = imgBlobRef.Sha });
            newTree.Tree.Add(new NewTreeItem { Path = $"{_blogname}/blog.json", Mode = "100644", Type = TreeType.Blob, Sha = metaBlobRef.Sha });
            newTree.Tree.Add(new NewTreeItem { Path = $"{_blogname}/blog.md", Mode = "100644", Type = TreeType.Blob, Sha = mdBlobRef.Sha });

            return CreateCommitTree(github, newTree);
        }

        private Task<TreeResponse> CreateCommitTree(Octokit.GitHubClient github, NewTree newTree)
        {
            return github.Git.Tree.Create(_username, _repo, newTree);
        }

        private Task<Commit> CreateCommit(Octokit.GitHubClient github, string message, string newTreeSha, string parentSha)
        {
            var newCommit = new NewCommit($"{message} {_blogname}", newTreeSha, parentSha);
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
                            Type = x.Type,
                            Sha = x.Sha
                        })
                        .ToList()
                        .ForEach(x => newTree.Tree.Add(x));
            return newTree;
        }
    }
}
