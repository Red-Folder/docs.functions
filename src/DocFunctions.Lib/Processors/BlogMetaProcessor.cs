using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;

namespace DocFunctions.Lib.Processors
{
    public class BlogMetaProcessor : IBlogMetaProcessor
    {

        private string _contentBaseUrl = "";

        public BlogMetaProcessor(string contentBaseUrl)
        {
            _contentBaseUrl = contentBaseUrl;
        }

        public Blog Transform(string metaJson)
        {
            var meta = JObject.Parse(metaJson);

            return new Blog
            {
                Id = (string)meta["id"],
                Url = (string)meta["url"],
                Author = "Mark Taylor",
                Published = meta["published"].Value<DateTime>(),
                Modified = meta["modified"].Value<DateTime>(),
                Title = (string)meta["title"],
                Enabled = Boolean.Parse((string)meta["enabled"]),

                Description = (string)meta["description"],
                Image = (string)meta["image"],

                Redirects = GetRedirects(meta),
                KeyWords = GetKeyWorks(meta),

                Series = (string)meta["series"],

                ContentUrl = $"{_contentBaseUrl}/{(string)meta["url"]}/{(string)meta["url"]}.html"
            };

        }

        private List<Redirect> GetRedirects(JObject meta)
        {
            var redirects = new List<Redirect>();
            if (meta["redirects"] != null)
            {
                foreach (var redirect in meta["redirects"])
                {
                    redirects.Add(new Redirect
                    {
                        RedirectType = (string)redirect["redirectType"] == "301" ? System.Net.HttpStatusCode.MovedPermanently : System.Net.HttpStatusCode.TemporaryRedirect,
                        Url = (string)redirect["url"],
                        RedirectByRoute = Boolean.Parse((string)redirect["redirectByRoute"]),
                        RedirectByParameter = Boolean.Parse((string)redirect["redirectByParameter"])
                    });
                }
            }
            return redirects;
        }

        private List<string> GetKeyWorks(JObject meta)
        {
            var keyWords = new List<string>();
            if (meta["keyWords"] != null)
            {
                foreach (var keyWord in meta["keyWords"])
                {
                    keyWords.Add((string)keyWord);
                }
            }
            return keyWords;
        }
    }
}
