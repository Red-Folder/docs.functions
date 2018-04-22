using System;
using docsFunctions.Shared.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeRepoDataManager
    {
        private List<Blog> _repo = new List<Blog>();

        public void SaveBlogToRepo(Blog blogMeta)
        {
            _repo.Add(blogMeta);
        }

        public void DeleteBlogFromRepo(string blogUrl)
        {
            _repo.RemoveAll(x => x.Url == blogUrl);
        }

        public bool UrlExists(string fullUrl)
        {
            Regex regex = new Regex(@"/api/Blog/(.*)\?");

            var blogUrl = regex.Match(fullUrl).Groups[1].Value;

            return _repo.Any(x => x.Url == blogUrl);
        }

        public string UrlContent(string fullUrl)
        {
            Regex regex = new Regex(@"/api/Blog/(.*)\?");

            var blogUrl = regex.Match(fullUrl).Groups[1].Value;

            var metaObject = _repo.Where(x => x.Url == blogUrl).FirstOrDefault();

            var metaString = Newtonsoft.Json.JsonConvert.SerializeObject(metaObject);
            return metaString;
        }
    }
}
