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
    public class ModifyImageActionBuilder
    {
        private Modified _modified;
        private Mock<IFtpsClient> _mockFtpsClient;

        private bool _ftpsClientSet = false;
        private IFtpsClient _ftpsClient;

        public ModifyImageActionBuilder(Modified modified)
        {
            _modified = modified;

            _mockFtpsClient = new Mock<IFtpsClient>();
        }

        public Mock<IFtpsClient> MockFtpsClient
        {
            get
            {
                return _mockFtpsClient;
            }
        }

        public ModifyImageActionBuilder SetFtpsClient(IFtpsClient ftpsclient)
        {
            _ftpsClientSet = true;
            _ftpsClient = ftpsclient;
            return this;
        }

        public ModifyImageAction Build()
        {
            return new ModifyImageAction(_modified,
                                        _ftpsClientSet ? _ftpsClient : _mockFtpsClient.Object
                                     );
        }

    }
}
