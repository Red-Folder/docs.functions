using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Models.Audit;
using DocFunctions.Lib.Models.Github;
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
        private Added _added;

        private Mock<IGithubReader> _mockGithubReader;
        private Mock<IMarkdownProcessor> _mockMarkdownProcessor;
        private Mock<IBlobClient> _mockBlobClient;
        private Mock<IBlogMetaProcessor> _mockBlogMetaReader;
        private Mock<IBlogMetaRepository> _mockBlogMetaRepository;
        private Mock<IWebCache> _mockCache;

        private bool _githubReaderSet = false;
        private IGithubReader _githubReader;
        private bool _markdownProcessorSet = false;
        private IMarkdownProcessor _markdownProcessor;
        private bool _blobClientSet = false;
        private IBlobClient _blobClient;
        private bool _blogMetaReaderSet = false;
        private IBlogMetaProcessor _blogMetaReader;
        private bool _blogMetaRepositorySet = false;
        private IBlogMetaRepository _blogMetaRepository;

        public NewBlogActionBuilder(Added added)
        {
            _added = added;

            _mockGithubReader = new Mock<IGithubReader>();
            _mockGithubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json"), It.Is<string>(x => x == "commit-sha-xxxx"))).Returns("{}");
            _mockGithubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.md"), It.Is<string>(x => x == "commit-sha-xxxx"))).Returns("## Hello World");

            _mockMarkdownProcessor = new Mock<IMarkdownProcessor>();
            _mockMarkdownProcessor.Setup(m => m.Process(It.IsAny<string>())).Returns("<h2>Hello World</h2>");

            _mockBlobClient = new Mock<IBlobClient>();

            _mockBlogMetaReader = new Mock<IBlogMetaProcessor>();
            _mockBlogMetaReader.Setup(m => m.Transform(It.IsAny<string>())).Returns(new Blog { Url = "testblog" });

            _mockBlogMetaRepository = new Mock<IBlogMetaRepository>();

            _mockCache = new Mock<IWebCache>();
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

        public Mock<IBlobClient> MockBlobClient
        {
            get
            {
                return _mockBlobClient;
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

        public NewBlogActionBuilder SetBlobClient(IBlobClient blobclient)
        {
            _blobClientSet = true;
            _blobClient = blobclient;
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
            return new NewBlogAction(_added,
                                        _githubReaderSet ? _githubReader : _mockGithubReader.Object,
                                        _markdownProcessorSet ? _markdownProcessor : _mockMarkdownProcessor.Object,
                                        _blobClientSet ? _blobClient : _mockBlobClient.Object,
                                        _blogMetaReaderSet ? _blogMetaReader : _mockBlogMetaReader.Object,
                                        _blogMetaRepositorySet ? _blogMetaRepository : _mockBlogMetaRepository.Object,
                                        _mockCache.Object,
                                        new AuditTree()
                                     );
        }
    }
}
