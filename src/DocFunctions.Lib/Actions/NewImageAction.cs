using DocFunctions.Lib.Wappers;
using docsFunctions.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Actions
{
    public class NewImageAction : IAction
    {
        private string _blogPath;
        private string _imageName;
        private IGithubReader _githubReader;
        private IFtpsClient _ftpsClient;
        private IBlogMetaProcessor _blogMetaReader;



        public NewImageAction(string blogPath,
                                string imageName,
                                IGithubReader githubReader,
                                IFtpsClient ftpsClient,
                                IBlogMetaProcessor blogMetaReader)
        {
            if (blogPath == null) throw new ArgumentNullException("blogPath");
            if (imageName == null) throw new ArgumentNullException("imageName");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");


            _blogPath = blogPath;
            _imageName = imageName;
            _githubReader = githubReader;
            _ftpsClient = ftpsClient;
            _blogMetaReader = blogMetaReader;
        }

        public void Execute()
        {
            var blogMetaJson = GetMetaJsonFromGithub();
            var blogMeta = GetMetaFromMetaJson(blogMetaJson);

            var blogImage = GetImageFromGithub();
            UploadImages(blogMeta, blogImage);
        }

        private string GetMetaJsonFromGithub()
        {
            return _githubReader.GetRawFile(_blogPath + "/blog.json");
        }

        private Blog GetMetaFromMetaJson(string blogMetaJson)
        {
            return _blogMetaReader.Transform(blogMetaJson);
        }

        private byte[] GetImageFromGithub()
        {
            return _githubReader.GetRawImageFile($"{_blogPath}/{_imageName}");
        }

        private void UploadImages(Blog blogMeta, byte[] image)
        {
            _ftpsClient.Upload($"/site/contentroot/{blogMeta.Url}/{_imageName}", image);
        }
    }
}
