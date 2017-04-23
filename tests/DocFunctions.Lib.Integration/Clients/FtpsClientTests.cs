using DocFunctions.Lib.Clients;
using System;
using System.Configuration;
using Xunit;

namespace DocFunctions.Lib.Integration.Clients
{
    public class FtpsClientTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void UploadFile()
        {
            var host = ConfigurationManager.AppSettings["ftps-host"];
            var username = ConfigurationManager.AppSettings["ftps-username"];
            var password = ConfigurationManager.AppSettings["ftps-password"];

            var filename = $"/site/contentroot/test-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.html";
            var contents = "<html><body>Hello World</body></html>";

            var sut = new FtpsClient(host, username, password);

            sut.Upload(filename, contents);
        }
    }
}
