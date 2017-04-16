using DocFunctions.Lib.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using DocFunctions.Lib.Models.Github;

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
                var filesList = new List<Tuple<string, string>>();
                foreach (var added in commit.Added)
                {
                    filesList.Add(new Tuple<string, string>(added.Split('/')[0], added.Split('/')[1]));
                }
                var newBlogs = filesList
                                    .Where(x => x.Item2.ToLower().EndsWith(".md") || x.Item2.ToLower().EndsWith(".json"))
                                    .Select(x => x.Item1)
                                    .Distinct();

                newBlogs.ToList().ForEach(x => _actionBuilder.NewBlog(x));

                _actionBuilder.Build();
            }
        }
    }
}
