using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Wappers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Unit.Builders
{
    public class DeleteImageActionBuilder
    {
        private Removed _removed;
        private Mock<IBlobClient> _mockFtpsClient;
        private Mock<IWebCache> _mockCache;

        private bool _ftpsClientSet = false;
        private IBlobClient _ftpsClient;

        public DeleteImageActionBuilder(Removed removed)
        {
            _removed = removed;

            _mockFtpsClient = new Mock<IBlobClient>();
            _mockCache = new Mock<IWebCache>();
        }

        public Mock<IBlobClient> MockFtpsClient
        {
            get
            {
                return _mockFtpsClient;
            }
        }

        public DeleteImageActionBuilder SetFtpsClient(IBlobClient ftpsclient)
        {
            _ftpsClientSet = true;
            _ftpsClient = ftpsclient;
            return this;
        }

        public DeleteImageAction Build()
        {
            return new DeleteImageAction(_removed,
                                        _ftpsClientSet ? _ftpsClient : _mockFtpsClient.Object,
                                        _mockCache.Object
                                     );
        }

    }
}
