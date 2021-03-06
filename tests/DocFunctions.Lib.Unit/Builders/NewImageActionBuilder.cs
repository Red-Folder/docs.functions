﻿using DocFunctions.Lib.Actions;
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
    public class NewImageActionBuilder
    {
        private Added _added;
        private Mock<IGithubReader> _mockGithubReader;
        private Mock<IBlobClient> _mockBlobClient;
        private Mock<IBlogMetaProcessor> _mockBlogMetaReader;
        private Mock<IWebCache> _mockCache;

        private bool _githubReaderSet = false;
        private IGithubReader _githubReader;
        private bool _blobClientSet = false;
        private IBlobClient _blobClient;
        private bool _blogMetaReaderSet = false;
        private IBlogMetaProcessor _blogMetaReader;

        public NewImageActionBuilder(Added added)
        {
            _added = added;

            _mockGithubReader = new Mock<IGithubReader>();
            _mockGithubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json"), It.Is<string>(x => x == "commit-sha-xxxx"))).Returns("{}");

            _mockBlobClient = new Mock<IBlobClient>();

            _mockBlogMetaReader = new Mock<IBlogMetaProcessor>();
            _mockBlogMetaReader.Setup(m => m.Transform(It.IsAny<string>())).Returns(new Blog { Url = "testblog" });

            _mockCache = new Mock<IWebCache>();
        }

        public Mock<IGithubReader> MockGithubReader
        {
            get
            {
                return _mockGithubReader;
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

        public NewImageActionBuilder SetGithubReader(IGithubReader githubReader)
        {
            _githubReaderSet = true;
            _githubReader = githubReader;
            return this;
        }

        public NewImageActionBuilder SetBlobClient(IBlobClient blobClient)
        {
            _blobClientSet = true;
            _blobClient = blobClient;
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
            return new NewImageAction(_added,
                                        _githubReaderSet ? _githubReader : _mockGithubReader.Object,
                                        _blobClientSet ? _blobClient : _mockBlobClient.Object,
                                        _blogMetaReaderSet ? _blogMetaReader : _mockBlogMetaReader.Object,
                                        _mockCache.Object,
                                        new AuditTree()
                                     );
        }
    }
}
