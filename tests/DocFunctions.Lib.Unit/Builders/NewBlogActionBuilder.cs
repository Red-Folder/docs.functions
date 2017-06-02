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
    public class NewBlogActionBuilder
    {
        private string _blogPath;

        private Mock<IGithubReader> _mockGithubReader;
        private Mock<IMarkdownProcessor> _mockMarkdownProcessor;
        private Mock<IFtpsClient> _mockFtpsClient;
        private Mock<IBlogMetaProcessor> _mockBlogMetaReader;
        private Mock<IBlogMetaRepository> _mockBlogMetaRepository;

        private bool _githubReaderSet = false;
        private IGithubReader _githubReader;
        private bool _markdownProcessorSet = false;
        private IMarkdownProcessor _markdownProcessor;
        private bool _ftpsClientSet = false;
        private IFtpsClient _ftpsClient;
        private bool _blogMetaReaderSet = false;
        private IBlogMetaProcessor _blogMetaReader;
        private bool _blogMetaRepositorySet = false;
        private IBlogMetaRepository _blogMetaRepository;

        public NewBlogActionBuilder(string blogPath)
        {
            _blogPath = blogPath;

            _mockGithubReader = new Mock<IGithubReader>();
            // TODO
            //_mockGithubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json"))).Returns("{}");
            //_mockGithubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.md"))).Returns("## Hello World");

            _mockMarkdownProcessor = new Mock<IMarkdownProcessor>();
            _mockMarkdownProcessor.Setup(m => m.Process(It.IsAny<string>())).Returns("<h2>Hello World</h2>");

            _mockFtpsClient = new Mock<IFtpsClient>();

            _mockBlogMetaReader = new Mock<IBlogMetaProcessor>();
            _mockBlogMetaReader.Setup(m => m.Transform(It.IsAny<string>())).Returns(new Blog { Url = "testblog" });

            _mockBlogMetaRepository = new Mock<IBlogMetaRepository>();
        }

        public Mock<IGithubReader> MockGithubReader
        {
            get
            {
                return _mockGithubReader;
            }
        }

        public Mock<IMarkdownProcessor> MockMarkdownProcessor
        {
            get
            {
                return _mockMarkdownProcessor;
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

        public Mock<IBlogMetaRepository> MockBlogMetaRepository
        {
            get
            {
                return _mockBlogMetaRepository;
            }
        }

        public NewBlogActionBuilder SetGithubReader(IGithubReader githubReader)
        {
            _githubReaderSet = true;
            _githubReader = githubReader;
            return this;
        }

        public NewBlogActionBuilder SetMarkdownProcessor(IMarkdownProcessor markdownProcessor)
        {
            _markdownProcessorSet = true;
            _markdownProcessor = markdownProcessor;
            return this;
        }

        public NewBlogActionBuilder SetFtpsClient(IFtpsClient ftpsclient)
        {
            _ftpsClientSet = true;
            _ftpsClient = ftpsclient;
            return this;
        }

        public NewBlogActionBuilder SetBlogMetaProcessor(IBlogMetaProcessor blogMetaProcessor)
        {
            _blogMetaReaderSet = true;
            _blogMetaReader = blogMetaProcessor;
            return this;
        }

        public NewBlogActionBuilder SetBlogMetaRepository(IBlogMetaRepository blogMetaRespository)
        {
            _blogMetaRepositorySet = true;
            _blogMetaRepository = blogMetaRespository;
            return this;
        }

        public NewBlogAction Build()
        {
            //TODO
            //return new NewBlogAction(_blogPath,
            //                            _githubReaderSet ? _githubReader : _mockGithubReader.Object,
            //                            _markdownProcessorSet ? _markdownProcessor : _mockMarkdownProcessor.Object,
            //                            _ftpsClientSet ? _ftpsClient : _mockFtpsClient.Object,
            //                            _blogMetaReaderSet ? _blogMetaReader : _mockBlogMetaReader.Object,
            //                            _blogMetaRepositorySet ? _blogMetaRepository : _mockBlogMetaRepository.Object
            //                         );

            throw new NotImplementedException();
        }
    }
}
