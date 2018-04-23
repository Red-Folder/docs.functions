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
        private Mock<IBlobClient> _mockBlobClient;
        private Mock<IWebCache> _mockCache;

        private bool _blobClientSet = false;
        private IBlobClient _blobClient;

        public DeleteImageActionBuilder(Removed removed)
        {
            _removed = removed;

            _mockBlobClient = new Mock<IBlobClient>();
            _mockCache = new Mock<IWebCache>();
        }

        public Mock<IBlobClient> MockBlobClient
        {
            get
            {
                return _mockBlobClient;
            }
        }

        public DeleteImageActionBuilder SetBlobClient(IBlobClient blobClient)
        {
            _blobClientSet = true;
            _blobClient = blobClient;
            return this;
        }

        public DeleteImageAction Build()
        {
            return new DeleteImageAction(_removed,
                                        _blobClientSet ? _blobClient : _mockBlobClient.Object,
                                        _mockCache.Object
                                     );
        }

    }
}
