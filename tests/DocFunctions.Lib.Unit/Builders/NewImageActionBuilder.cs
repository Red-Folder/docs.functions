using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Wappers;
using docsFunctions.Shared.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Unit.Builders
{
    public class NewImageActionBuilder
    {
        private string _blogPath;
        private string _imageName;

        private Mock<IGithubReader> _mockGithubReader;
        private Mock<IFtpsClient> _mockFtpsClient;
        private Mock<IBlogMetaProcessor> _mockBlogMetaReader;

        private bool _githubReaderSet = false;
        private IGithubReader _githubReader;
        private bool _ftpsClientSet = false;
        private IFtpsClient _ftpsClient;
        private bool _blogMetaReaderSet = false;
        private IBlogMetaProcessor _blogMetaReader;

        public NewImageActionBuilder(string blogPath, string imageName)
        {
            _blogPath = blogPath;
            _imageName = imageName;

            _mockGithubReader = new Mock<IGithubReader>();
            _mockGithubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json"))).Returns("{}");

            _mockFtpsClient = new Mock<IFtpsClient>();

            _mockBlogMetaReader = new Mock<IBlogMetaProcessor>();
            _mockBlogMetaReader.Setup(m => m.Transform(It.IsAny<string>())).Returns(new Blog { Url = "testblog" });
        }

        public Mock<IGithubReader> MockGithubReader
        {
            get
            {
                return _mockGithubReader;
            }
        }

        public Mock<IFtpsClient> MockFtpsClient
        {
            get
            {
                return _mockFtpsClient;
            }
        }

        public Mock<IBlogMetaProcessor> MockBlogMetaReader
        {
            get
            {
                return _mockBlogMetaReader;
            }
        }

        public NewImageActionBuilder SetGithubReader(IGithubReader githubReader)
        {
            _githubReaderSet = true;
            _githubReader = githubReader;
            return this;
        }

        public NewImageActionBuilder SetFtpsClient(IFtpsClient ftpsclient)
        {
            _ftpsClientSet = true;
            _ftpsClient = ftpsclient;
            return this;
        }

        public NewImageActionBuilder SetBlogMetaProcessor(IBlogMetaProcessor blogMetaProcessor)
        {
            _blogMetaReaderSet = true;
            _blogMetaReader = blogMetaProcessor;
            return this;
        }

        public NewImageAction Build()
        {
            return new NewImageAction(_blogPath,
                                        _imageName,
                                        _githubReaderSet ? _githubReader : _mockGithubReader.Object,
                                        _ftpsClientSet ? _ftpsClient : _mockFtpsClient.Object,
                                        _blogMetaReaderSet ? _blogMetaReader : _mockBlogMetaReader.Object
                                     );
        }
    }
}
