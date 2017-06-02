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
    public class NewImageAction : IAction
    {
        private Added _data;
        private IGithubReader _githubReader;
        private IFtpsClient _ftpsClient;
        private IBlogMetaProcessor _blogMetaReader;



        public NewImageAction(Added data,
                                IGithubReader githubReader,
                                IFtpsClient ftpsClient,
                                IBlogMetaProcessor blogMetaReader)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");


            _data = data;
            _githubReader = githubReader;
            _ftpsClient = ftpsClient;
            _blogMetaReader = blogMetaReader;
        }

        public void Execute()
        {
            var blogMetaJson = GetMetaJsonFromGithub();
            var blogMeta = GetMetaFromMetaJson(blogMetaJson);

            var blogImage = GetImageFromGithub();
            UploadImage(blogMeta, blogImage);
        }

        private string GetMetaJsonFromGithub()
        {
            return _githubReader.GetRawFile(_data.Path + "/blog.json", _data.CommitShaForRead);
        }

        private Blog GetMetaFromMetaJson(string blogMetaJson)
        {
            return _blogMetaReader.Transform(blogMetaJson);
        }

        private byte[] GetImageFromGithub()
        {
            return _githubReader.GetRawImageFile($"{_data.FullFilename}", _data.CommitShaForRead);
        }

        private void UploadImage(Blog blogMeta, byte[] image)
        {
            _ftpsClient.Upload($"/site/mediaroot/blog/{blogMeta.Url}/{_data.Filename}", image);
        }
    }
}
