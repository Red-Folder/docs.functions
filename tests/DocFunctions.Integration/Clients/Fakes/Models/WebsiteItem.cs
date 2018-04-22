using System;

namespace DocFunctions.Integration.Clients.Fakes.Models
{
    public class WebsiteItem
    {
        private string _physicalFilename;
        private Object _contents;

        public WebsiteItem(string physicalFilename, Object contents)
        {
            _physicalFilename = physicalFilename;
            _contents = contents;
        }

        public string FullUrl
        {
            get
            {
                if (IsImage)
                {
                    var url = $"https://rfcdocs.blob.core.windows.net/rfcdocs-media/media/blog/{_physicalFilename.Replace("/site/mediaroot/blog/", "")}";
                    return url.ToLower();
                }
                else
                {
                    var urlSuffix = _physicalFilename
                                        .Replace("/site/contentroot/", "")
                                        .Replace(".html", "");
                    var url = $"https://rfc-doc-functions-staging.azurewebsites.net/api/Blog/{urlSuffix}";
                    return url.ToLower();
                }
            }
        }

        public bool IsImage
        {
            get
            {
                return (_physicalFilename.ToLower().EndsWith(".png") || _physicalFilename.ToLower().EndsWith(".jpg"));
            }
        }

        public string PhysicalFilename
        {
            get
            {
                return _physicalFilename;
            }
        }

        public Object Contents
        {
            get
            {
                return _contents;
            }
        }
    }
}
