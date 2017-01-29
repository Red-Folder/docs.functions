using Octokit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Helpers
{
    public class GitHub
    {
        private string _username;
        private string _key;
        private string _repo;

        public GitHub(string username, string key, string repo)
        {
            _username = username;
            _key = key;
            _repo = repo;
        }

        public void CreateTestBog()
        {
            CreateTestBogAsync().Wait();
        }

        public async Task CreateTestBogAsync()
        {
            var blogname = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            var credentials = new Octokit.Credentials(_username, _key);
            var connection = new Octokit.Connection(new Octokit.ProductHeaderValue("Red-Folder.DocFunctions.IntegrationTests"))
            {
                Credentials = credentials
            };
            var github = new Octokit.GitHubClient(connection);

            // Below adapted from http://laedit.net/2016/11/12/GitHub-commit-with-Octokit-net.html

            //-------------------------------------------------------------------------------------------
            // Get the latest commit
            //-------------------------------------------------------------------------------------------
            // Get reference of master branch
            var masterReference = await github.Git.Reference.Get(_username, _repo, "heads/master");
            // Get the laster commit of this branch
            var latestCommit = await github.Git.Commit.Get(_username, _repo, masterReference.Object.Sha);

            //-------------------------------------------------------------------------------------------
            // Create an image
            //-------------------------------------------------------------------------------------------
            // For image, get image content and convert it to base64
            var imgBase64 = Convert.ToBase64String(File.ReadAllBytes(@"Assets\Image.png"));
            // Create image blob
            var imgBlob = new NewBlob { Encoding = EncodingType.Base64, Content = (imgBase64) };
            var imgBlobRef = await github.Git.Blob.Create(_username, _repo, imgBlob);

            //-------------------------------------------------------------------------------------------
            // Create a meta file
            //-------------------------------------------------------------------------------------------
            // Create text blob
            var metaContents = File.ReadAllText(@"Assets\blog.json").Replace("[BLOGNAME]", blogname);
            var metaBlob = new NewBlob { Encoding = EncodingType.Utf8, Content = metaContents };
            var metaBlobRef = await github.Git.Blob.Create(_username, _repo, metaBlob);

            //-------------------------------------------------------------------------------------------
            // Create a markdown file
            //-------------------------------------------------------------------------------------------
            // Create text blob
            var mdContents = File.ReadAllText(@"Assets\blog.md").Replace("[BLOGNAME]", blogname);
            var mdBlob = new NewBlob { Encoding = EncodingType.Utf8, Content = mdContents };
            var mdBlobRef = await github.Git.Blob.Create(_username, _repo, mdBlob);

            //-------------------------------------------------------------------------------------------
            // Create a tree
            //-------------------------------------------------------------------------------------------
            // Create new Tree
            var nt = new NewTree { BaseTree = latestCommit.Tree.Sha };
            // Add items based on blobs
            nt.Tree.Add(new NewTreeItem { Path = $"{blogname}/Image.jpg", Mode = "100644", Type = TreeType.Blob, Sha = imgBlobRef.Sha });
            nt.Tree.Add(new NewTreeItem { Path = $"{blogname}/blog.json", Mode = "100644", Type = TreeType.Blob, Sha = metaBlobRef.Sha });
            nt.Tree.Add(new NewTreeItem { Path = $"{blogname}/blog.md", Mode = "100644", Type = TreeType.Blob, Sha = mdBlobRef.Sha });

            var newTree = await github.Git.Tree.Create(_username, _repo, nt);

            //-------------------------------------------------------------------------------------------
            // Create commit
            //-------------------------------------------------------------------------------------------
            // Create Commit
            var newCommit = new NewCommit($"Created test blog {blogname}", newTree.Sha, masterReference.Object.Sha);
            var commit = await github.Git.Commit.Create(_username, _repo, newCommit);

            //-------------------------------------------------------------------------------------------
            // And push it
            //-------------------------------------------------------------------------------------------
            // Update HEAD with the commit
            await github.Git.Reference.Update(_username, _repo, "heads/master", new ReferenceUpdate(commit.Sha));
        }
    }
}
