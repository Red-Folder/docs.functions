using DocFunctions.Lib.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Helpers;
using Xunit;

namespace DocFunctions.Lib.Integration.Clients
{
    public class FtpsClientTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void UploadFile()
        {
            var host = AppSettings.AppSetting("ftps-host");
            var username = AppSettings.AppSetting("ftps-username");
            var password = AppSettings.AppSetting("ftps-password");

            var filename = $"/site/contentroot/test-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.html";
            var contents = "<html><body>Hello World</body></html>";

            var sut = new FtpsClient(host, username, password);

            sut.Upload(filename, contents);
        }
    }
}
