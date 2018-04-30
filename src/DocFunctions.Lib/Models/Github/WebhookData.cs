using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Github
{
    public class WebhookData
    {
        private static string IGNORE = "[ignore]";

        public static WebhookData Deserialize(string rawJson)
        {
            var raw = JsonConvert.DeserializeObject<Models.Github.Raw.WebhookData>(rawJson);

            var commits = new List<Commit>();

            foreach(var rawCommit in raw.Commits)
            {
                if (!rawCommit.Message.ToLower().Contains(IGNORE))
                {
                    var commit = new Commit();
                    commit.Sha = rawCommit.Id;
                    commit.Message = rawCommit.Message;

                    commit.Added = new List<Added>();
                    commit.Removed = new List<Removed>();
                    commit.Modified = new List<Modified>();

                    foreach (var rawAdded in rawCommit.Added)
                    {
                        commit.Added.Add(new Added { FullFilename = rawAdded, CommitSha = rawCommit.Id, CommitShaForRead = rawCommit.Id });
                    }

                    foreach (var rawRemoved in rawCommit.Removed)
                    {
                        commit.Removed.Add(new Removed { FullFilename = rawRemoved, CommitSha = rawCommit.Id, CommitShaForRead = GetPreviousCommitSha(raw, rawCommit.Id) });
                    }

                    foreach (var rawModified in rawCommit.Modified)
                    {
                        commit.Modified.Add(new Modified { FullFilename = rawModified, CommitSha = rawCommit.Id, CommitShaForRead = GetPreviousCommitSha(raw, rawCommit.Id) });
                    }

                    commits.Add(commit);
                }
            }

            return new WebhookData { Commits = commits };
        }

        private static string GetPreviousCommitSha(Models.Github.Raw.WebhookData raw, string currentCommitSha)
        {
            var orderedList = GetOrderedCommitShas(raw);

            var currentPosition = orderedList.IndexOf(currentCommitSha);

            return orderedList[currentPosition - 1];
        }

        private static List<string> GetOrderedCommitShas(Models.Github.Raw.WebhookData raw)
        {
            var list = raw.Commits.OrderByDescending(x => x.Timestamp).Select(x => x.Id).ToList();
            list.Add(raw.Before);

            list.Reverse();

            return list;
        }

        public List<Commit> Commits;
    }
}
