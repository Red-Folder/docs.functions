using DocFunctions.Lib.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Actions;

namespace DocFunctions.Lib
{
    public class WebhookActionBuilder
    {
        private IActionBuilder _actionBuilder;

        public WebhookActionBuilder(IActionBuilder actionBuilder)
        {
            _actionBuilder = actionBuilder;
        }

        public void Process(WebhookData data)
        {
            foreach (var commit in data.Commits)
            {
                _actionBuilder.Clear();

                GetNewBlogs(commit).ForEach(x => _actionBuilder.NewBlog(x));
                GetDeletedBlogs(commit).ForEach(x => _actionBuilder.DeleteBlog(x));

                GetNewImages(commit).ForEach(x => _actionBuilder.NewImage(x.Item1, x.Item2));
                GetDeletedImages(commit).ForEach(x => _actionBuilder.DeleteImage(x.Item1, x.Item2));

                var actions = _actionBuilder.Build();
                Execute(actions);   
            }
        }

        private List<string> GetNewBlogs(Commit commit)
        {
            var filesList = new List<Tuple<string, string>>();
            foreach (var added in commit.Added)
            {
                filesList.Add(new Tuple<string, string>(added.Split('/')[0], added.Split('/')[1]));
            }
            var newBlogs = filesList
                                .Where(x => x.Item2.ToLower().EndsWith(".md") || x.Item2.ToLower().EndsWith(".json"))
                                .Select(x => x.Item1)
                                .Distinct();

            return newBlogs.ToList();
        }

        private List<Tuple<string, string>> GetNewImages(Commit commit)
        {
            var filesList = new List<Tuple<string, string>>();
            foreach (var added in commit.Added)
            {
                filesList.Add(new Tuple<string, string>(added.Split('/')[0], added.Split('/')[1]));
            }
            var newImages = filesList
                                .Where(x => x.Item2.ToLower().EndsWith(".png") || x.Item2.ToLower().EndsWith(".jpg") || x.Item2.ToLower().EndsWith(".gif"))
                                .Distinct();

            return newImages.ToList();
        }

        private List<string> GetDeletedBlogs(Commit commit)
        {
            var filesList = new List<Tuple<string, string>>();
            foreach (var added in commit.Removed)
            {
                filesList.Add(new Tuple<string, string>(added.Split('/')[0], added.Split('/')[1]));
            }
            var deletedBlogs = filesList
                                .Where(x => x.Item2.ToLower().EndsWith(".md") || x.Item2.ToLower().EndsWith(".json"))
                                .Select(x => x.Item1)
                                .Distinct();

            return deletedBlogs.ToList();
        }

        private List<Tuple<string, string>> GetDeletedImages(Commit commit)
        {
            var filesList = new List<Tuple<string, string>>();
            foreach (var added in commit.Removed)
            {
                filesList.Add(new Tuple<string, string>(added.Split('/')[0], added.Split('/')[1]));
            }
            var deletedImages = filesList
                                .Where(x => x.Item2.ToLower().EndsWith(".png") || x.Item2.ToLower().EndsWith(".jpg") || x.Item2.ToLower().EndsWith(".gif"))
                                .Distinct();

            return deletedImages.ToList();
        }

        private void Execute(IAction[] actions)
        {
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action.Execute();
                }
            }
        }
    }
}
