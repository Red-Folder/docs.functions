using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Wappers;
using docsFunctions.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Actions
{
    public class DeleteBlogAction : IAction
    {
        private Removed _data;
        private IGithubReader _githubReader;
        private IFtpsClient _ftpsClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;

        public DeleteBlogAction(Removed data,
                                IGithubReader githubReader,
                                IFtpsClient ftpsClient,
                                IBlogMetaProcessor blogMetaReader,
                                IBlogMetaRepository blogMetaRepository)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");

            _data = data;
            _githubReader = githubReader;
            _ftpsClient = ftpsClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
        }

        public void Execute()
        {
            var blogMetaJson = GetMetaJsonFromGithub();
            var blogMeta = GetMetaFromMetaJson(blogMetaJson);

            DeleteBlogMarkup(blogMeta);

            DeleteBlogMeta(blogMeta);
        }

        private string GetMetaJsonFromGithub()
        {
            return _githubReader.GetRawFile(_data.Path + "/blog.json", _data.CommitShaForRead);
        }

        private Blog GetMetaFromMetaJson(string blogMetaJson)
        {
            return _blogMetaReader.Transform(blogMetaJson);
        }

        private void DeleteBlogMeta(Blog blogMeta)
        {
            _blogMetaRepository.Delete(blogMeta.Url);
        }

        private void DeleteBlogMarkup(Blog blogMeta)
        {
            _ftpsClient.Delete("/site/contentroot/" + blogMeta.Url + ".html");
        }
    }
}
