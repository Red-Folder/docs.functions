﻿using DocFunctions.Lib.Clients;
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

        [Fact]
        [Trait("Category", "Integration")]
        public void UploadImage()
        {
            var host = ConfigurationManager.AppSettings["ftps-host"];
            var username = ConfigurationManager.AppSettings["ftps-username"];
            var password = ConfigurationManager.AppSettings["ftps-password"];

            var filename = $"/site/mediaroot/test-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.jpg";

            var encodedImage = "iVBORw0KGgoAAAANSUhEUgAAARgAAACWCAIAAACKIQDHAAAAAXNSR0IArs4c\n6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVY\ndFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTCtCgrAAAAUOklEQVR4Xu2dCXAU\nVdeGG8ENAiIihB1l0WLRqMiigCwpBQyChEU2A2gUNGiAAAHCrkT / gEEQI6As\n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\nAAAAAElFTkSuQmCC\n";
            var contents = Convert.FromBase64String(encodedImage);

            var sut = new FtpsClient(host, username, password);

            sut.Upload(filename, contents);
        }

        [Fact(Skip="Not yet implemented")]
        [Trait("Category", "Integration")]
        public void DeleteImage()
        {
        }
    }
}
