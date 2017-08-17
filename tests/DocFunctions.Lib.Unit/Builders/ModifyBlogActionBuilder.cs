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
    public class ModifyBlogActionBuilder
    {
        private Modified _modified;
        private Mock<IGithubReader> _mockGithubReader;
        private Mock<IFtpsClient> _mockFtpsClient;
        private Mock<IBlogMetaProcessor> _mockBlogMetaReader;
        private Mock<IBlogMetaRepository> _mockBlogMetaRepository;

        private bool _githubReaderSet = false;
        private IGithubReader _githubReader;
        private bool _ftpsClientSet = false;
        private IFtpsClient _ftpsClient;
        private bool _blogMetaReaderSet = false;
        private IBlogMetaProcessor _blogMetaReader;
        private bool _blogMetaRepositorySet = false;
        private IBlogMetaRepository _blogMetaRepository;

        public ModifyBlogActionBuilder(Modified modified)
        {
            _modified = modified;

            _mockGithubReader = new Mock<IGithubReader>();
            _mockGithubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json"), It.Is<string>(x => x == "commit-sha-xxxx-new"))).Returns(@"{""id"":""new""}");
            _mockGithubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json"), It.Is<string>(x => x == "commit-sha-xxxx-old"))).Returns(@"{""id"":""old""}");

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

        public ModifyBlogActionBuilder SetGithubReader(IGithubReader githubReader)
        {
            _githubReaderSet = true;
            _githubReader = githubReader;
            return this;
        }

        public ModifyBlogActionBuilder SetFtpsClient(IFtpsClient ftpsclient)
        {
            _ftpsClientSet = true;
            _ftpsClient = ftpsclient;
            return this;
        }

        public ModifyBlogActionBuilder SetBlogMetaProcessor(IBlogMetaProcessor blogMetaProcessor)
        {
            _blogMetaReaderSet = true;
            _blogMetaReader = blogMetaProcessor;
            return this;
        }

        public ModifyBlogActionBuilder SetBlogMetaRepository(IBlogMetaRepository blogMetaRepository)
        {
            _blogMetaRepositorySet = true;
            _blogMetaRepository = blogMetaRepository;
            return this;
        }

        public ModifyBlogAction Build()
        {
            return new ModifyBlogAction(_modified,
                                        _githubReaderSet ? _githubReader : _mockGithubReader.Object,
                                        _ftpsClientSet ? _ftpsClient : _mockFtpsClient.Object,
                                        _blogMetaReaderSet ? _blogMetaReader : _mockBlogMetaReader.Object,
                                        _blogMetaRepositorySet ? _blogMetaRepository: _mockBlogMetaRepository.Object
                                     );
        }

    }
}
