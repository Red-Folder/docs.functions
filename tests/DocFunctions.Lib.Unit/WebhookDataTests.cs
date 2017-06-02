using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using DocFunctions.Lib.Models.Github;

namespace DocFunctions.Lib.Unit
{
    public class WebhookDataTests
    {
        [Fact]
        public void CanDeSerialiseToModel()
        {
            var model = JsonConvert.DeserializeObject<Models.Github.Raw.WebhookData>(_rawJson);

            Assert.Equal(2, model.Commits.Length);
        }

        [Fact]
        public void CanDeSerialiseToModelAndGetAdds()
        {
            var model = JsonConvert.DeserializeObject<Models.Github.Raw.WebhookData>(_rawJson);

            Assert.Equal(3, model.Commits[0].Added.Length);
        }

        [Fact]
        public void CanDeSerialiseToModelAndGetRemoves()
        {
            var model = JsonConvert.DeserializeObject<Models.Github.Raw.WebhookData>(_rawJson);

            Assert.Equal(2, model.Commits[0].Removed.Length);
        }

        [Fact]
        public void CanGetPathForFirstCommitFirstAdd()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("2017-04-10-20-27-54/", model.Commits[0].Added[0].Path);
        }

        [Fact]
        public void CanGetFullFilenameForFirstCommitFirstAdd()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("2017-04-10-20-27-54/Image.jpg", model.Commits[0].Added[0].FullFilename);
        }

        [Fact]
        public void CanGetFilenameForFirstCommitFirstAdd()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("Image.jpg", model.Commits[0].Added[0].Filename);
        }

        [Fact]
        public void CanGetShaForReadForFirstCommitFirstAdd()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("e74f8255d4c8bc010101ec978exxxxxxxxx", model.Commits[0].Added[0].CommitShaForRead);
        }

        [Fact]
        public void CanGetShaForReadForFirstCommitFirstRemove()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("276d28cc847fcf5b2df23af24b757670284fb38e", model.Commits[0].Removed[0].CommitShaForRead);
        }

        [Fact]
        public void CanGetShaForReadForSecondCommitSecondAdd()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("e74f8255d4c8bc010101ec978efb6ee8d6007b44", model.Commits[1].Added[1].CommitShaForRead);
        }

        [Fact]
        public void CanGetShaForReadForSecondCommitSecondRemove()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("e74f8255d4c8bc010101ec978exxxxxxxxx", model.Commits[1].Removed[1].CommitShaForRead);
        }

        [Fact]
        public void CanGetShaForFirstCommitFirstAdd()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("e74f8255d4c8bc010101ec978exxxxxxxxx", model.Commits[0].Added[0].CommitSha);
        }

        [Fact]
        public void CanGetShaForFirstCommitFirstRemove()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("e74f8255d4c8bc010101ec978exxxxxxxxx", model.Commits[0].Removed[0].CommitSha);
        }

        [Fact]
        public void CanGetShaForSecondCommitSecondAdd()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("e74f8255d4c8bc010101ec978efb6ee8d6007b44", model.Commits[1].Added[1].CommitSha);
        }

        [Fact]
        public void CanGetShaForSecondCommitSecondRemove()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.Equal("e74f8255d4c8bc010101ec978efb6ee8d6007b44", model.Commits[1].Removed[1].CommitSha);
        }

        [Fact]
        public void FirstCommitFirstAddedIsImage()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.True(model.Commits[0].Added[0].IsImageFile);
        }

        [Fact]
        public void FirstCommitFirstAddedIsNotBlog()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.False(model.Commits[0].Added[0].IsBlogFile);
        }

        [Fact]
        public void SecondCommitSecondRemovedIsNotImage()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.False(model.Commits[1].Removed[1].IsImageFile);
        }

        [Fact]
        public void SecondCommitSecondRemoveIsBlog()
        {
            var model = WebhookData.Deserialize(_rawJson);

            Assert.True(model.Commits[1].Removed[1].IsBlogFile);
        }

        private const string _rawJson = @"
            {
	            ""ref"": ""refs/heads/master"",
	            ""before"": ""276d28cc847fcf5b2df23af24b757670284fb38e"",
	            ""after"": ""e74f8255d4c8bc010101ec978efb6ee8d6007b44"",
	            ""created"": false,
	            ""deleted"": false,
	            ""forced"": false,
	            ""base_ref"": null,
	            ""compare"": ""https://github.com/Red-Folder/red-folder.docs.staging/compare/276d28cc847f...e74f8255d4c8"",
	            ""commits"": [{
			            ""id"": ""e74f8255d4c8bc010101ec978exxxxxxxxx"",
			            ""tree_id"": ""04ed15e90623b327c996117bb05cb06f8223a55e"",
			            ""distinct"": true,
			            ""message"": ""Created test blog 2017-04-10-20-27-54"",
			            ""timestamp"": ""2017-04-10T19:27:58+00:00"",
			            ""url"": ""https://github.com/Red-Folder/red-folder.docs.staging/commit/e74f8255d4c8bc010101ec978efb6ee8d6007b44"",
			            ""author"": {
				            ""name"": ""Mark Taylor"",
				            ""email"": ""markbryantaylor@gmail.com"",
				            ""username"": ""Red-Folder""

                        },
			            ""committer"": {
				            ""name"": ""Mark Taylor"",
				            ""email"": ""markbryantaylor@gmail.com"",
				            ""username"": ""Red-Folder""
			            },
			            ""added"": [""2017-04-10-20-27-54/Image.jpg"", ""2017-04-10-20-27-54/blog.json"", ""2017-04-10-20-27-54/blog.md""],
			            ""removed"": [""2017-04-10-20-27-54/Image.jpg"", ""2017-04-10-20-27-54/blog.json""],
			            ""modified"": []
		            },
                    {
			            ""id"": ""e74f8255d4c8bc010101ec978efb6ee8d6007b44"",
			            ""tree_id"": ""04ed15e90623b327c996117bb05cb06f8223a55e"",
			            ""distinct"": true,
			            ""message"": ""Created test blog 2017-04-11-20-27-54"",
			            ""timestamp"": ""2017-04-11T19:27:58+00:00"",
			            ""url"": ""https://github.com/Red-Folder/red-folder.docs.staging/commit/e74f8255d4c8bc010101ec978efb6ee8d6007b44"",
			            ""author"": {
				            ""name"": ""Mark Taylor"",
				            ""email"": ""markbryantaylor@gmail.com"",
				            ""username"": ""Red-Folder""

                        },
			            ""committer"": {
				            ""name"": ""Mark Taylor"",
				            ""email"": ""markbryantaylor@gmail.com"",
				            ""username"": ""Red-Folder""
			            },
			            ""added"": [""2017-04-11-20-27-54/Image.jpg"", ""2017-04-11-20-27-54/blog.json"", ""2017-04-11-20-27-54/blog.md""],
			            ""removed"": [""2017-04-11-20-27-54/Image.jpg"", ""2017-04-11-20-27-54/blog.json""],
			            ""modified"": []
		            }
	            ],
	            ""head_commit"": {
		            ""id"": ""e74f8255d4c8bc010101ec978efb6ee8d6007b44"",
		            ""tree_id"": ""04ed15e90623b327c996117bb05cb06f8223a55e"",
		            ""distinct"": true,
		            ""message"": ""Created test blog 2017-04-10-20-27-54"",
		            ""timestamp"": ""2017-04-10T19:27:58+00:00"",
		            ""url"": ""https://github.com/Red-Folder/red-folder.docs.staging/commit/e74f8255d4c8bc010101ec978efb6ee8d6007b44"",
		            ""author"": {
			            ""name"": ""Mark Taylor"",
			            ""email"": ""markbryantaylor@gmail.com"",
			            ""username"": ""Red-Folder""
		            },
		            ""committer"": {
			            ""name"": ""Mark Taylor"",
			            ""email"": ""markbryantaylor@gmail.com"",
			            ""username"": ""Red-Folder""
		            },
		            ""added"": [""2017-04-10-20-27-54/Image.jpg"", ""2017-04-10-20-27-54/blog.json"", ""2017-04-10-20-27-54/blog.md""],
		            ""removed"": [],
		            ""modified"": []
	            },
	            ""repository"": {
		            ""id"": 80350516,
		            ""name"": ""red-folder.docs.staging"",
		            ""full_name"": ""Red-Folder/red-folder.docs.staging"",
		            ""owner"": {
			            ""name"": ""Red-Folder"",
			            ""email"": ""markbryantaylor@gmail.com"",
			            ""login"": ""Red-Folder"",
			            ""id"": 2298738,
			            ""avatar_url"": ""https://avatars3.githubusercontent.com/u/2298738?v=3"",
			            ""gravatar_id"": """",
			            ""url"": ""https://api.github.com/users/Red-Folder"",
			            ""html_url"": ""https://github.com/Red-Folder"",
			            ""followers_url"": ""https://api.github.com/users/Red-Folder/followers"",
			            ""following_url"": ""https://api.github.com/users/Red-Folder/following{/other_user}"",
			            ""gists_url"": ""https://api.github.com/users/Red-Folder/gists{/gist_id}"",
			            ""starred_url"": ""https://api.github.com/users/Red-Folder/starred{/owner}{/repo}"",
			            ""subscriptions_url"": ""https://api.github.com/users/Red-Folder/subscriptions"",
			            ""organizations_url"": ""https://api.github.com/users/Red-Folder/orgs"",
			            ""repos_url"": ""https://api.github.com/users/Red-Folder/repos"",
			            ""events_url"": ""https://api.github.com/users/Red-Folder/events{/privacy}"",
			            ""received_events_url"": ""https://api.github.com/users/Red-Folder/received_events"",
			            ""type"": ""User"",
			            ""site_admin"": false
		            },
		            ""private"": false,
		            ""html_url"": ""https://github.com/Red-Folder/red-folder.docs.staging"",
		            ""description"": ""A test version of my red-folder.docs repo - used for Staging testing"",
		            ""fork"": false,
		            ""url"": ""https://github.com/Red-Folder/red-folder.docs.staging"",
		            ""forks_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/forks"",
		            ""keys_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/keys{/key_id}"",
		            ""collaborators_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/collaborators{/collaborator}"",
		            ""teams_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/teams"",
		            ""hooks_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/hooks"",
		            ""issue_events_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/issues/events{/number}"",
		            ""events_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/events"",
		            ""assignees_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/assignees{/user}"",
		            ""branches_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/branches{/branch}"",
		            ""tags_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/tags"",
		            ""blobs_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/git/blobs{/sha}"",
		            ""git_tags_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/git/tags{/sha}"",
		            ""git_refs_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/git/refs{/sha}"",
		            ""trees_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/git/trees{/sha}"",
		            ""statuses_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/statuses/{sha}"",
		            ""languages_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/languages"",
		            ""stargazers_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/stargazers"",
		            ""contributors_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/contributors"",
		            ""subscribers_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/subscribers"",
		            ""subscription_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/subscription"",
		            ""commits_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/commits{/sha}"",
		            ""git_commits_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/git/commits{/sha}"",
		            ""comments_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/comments{/number}"",
		            ""issue_comment_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/issues/comments{/number}"",
		            ""contents_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/contents/{+path}"",
		            ""compare_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/compare/{base}...{head}"",
		            ""merges_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/merges"",
		            ""archive_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/{archive_format}{/ref}"",
		            ""downloads_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/downloads"",
		            ""issues_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/issues{/number}"",
		            ""pulls_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/pulls{/number}"",
		            ""milestones_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/milestones{/number}"",
		            ""notifications_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/notifications{?since,all,participating}"",
		            ""labels_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/labels{/name}"",
		            ""releases_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/releases{/id}"",
		            ""deployments_url"": ""https://api.github.com/repos/Red-Folder/red-folder.docs.staging/deployments"",
		            ""created_at"": 1485699994,
		            ""updated_at"": ""2017-04-10T18:44:48Z"",
		            ""pushed_at"": 1491852479,
		            ""git_url"": ""git://github.com/Red-Folder/red-folder.docs.staging.git"",
		            ""ssh_url"": ""git@github.com:Red-Folder/red-folder.docs.staging.git"",
		            ""clone_url"": ""https://github.com/Red-Folder/red-folder.docs.staging.git"",
		            ""svn_url"": ""https://github.com/Red-Folder/red-folder.docs.staging"",
		            ""homepage"": null,
		            ""size"": 10,
		            ""stargazers_count"": 0,
		            ""watchers_count"": 0,
		            ""language"": null,
		            ""has_issues"": true,
		            ""has_projects"": true,
		            ""has_downloads"": true,
		            ""has_wiki"": true,
		            ""has_pages"": false,
		            ""forks_count"": 0,
		            ""mirror_url"": null,
		            ""open_issues_count"": 0,
		            ""forks"": 0,
		            ""open_issues"": 0,
		            ""watchers"": 0,
		            ""default_branch"": ""master"",
		            ""stargazers"": 0,
		            ""master_branch"": ""master""
	            },
	            ""pusher"": {
		            ""name"": ""Red-Folder"",
		            ""email"": ""markbryantaylor@gmail.com""
	            },
	            ""sender"": {
		            ""login"": ""Red-Folder"",
		            ""id"": 2298738,
		            ""avatar_url"": ""https://avatars3.githubusercontent.com/u/2298738?v=3"",
		            ""gravatar_id"": """",
		            ""url"": ""https://api.github.com/users/Red-Folder"",
		            ""html_url"": ""https://github.com/Red-Folder"",
		            ""followers_url"": ""https://api.github.com/users/Red-Folder/followers"",
		            ""following_url"": ""https://api.github.com/users/Red-Folder/following{/other_user}"",
		            ""gists_url"": ""https://api.github.com/users/Red-Folder/gists{/gist_id}"",
		            ""starred_url"": ""https://api.github.com/users/Red-Folder/starred{/owner}{/repo}"",
		            ""subscriptions_url"": ""https://api.github.com/users/Red-Folder/subscriptions"",
		            ""organizations_url"": ""https://api.github.com/users/Red-Folder/orgs"",
		            ""repos_url"": ""https://api.github.com/users/Red-Folder/repos"",
		            ""events_url"": ""https://api.github.com/users/Red-Folder/events{/privacy}"",
		            ""received_events_url"": ""https://api.github.com/users/Red-Folder/received_events"",
		            ""type"": ""User"",
		            ""site_admin"": false
	            }
            }";
    }


}
