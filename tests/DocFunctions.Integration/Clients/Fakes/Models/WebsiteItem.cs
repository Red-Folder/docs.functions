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
                var url = $"https://rfcdocs.blob.core.windows.net/rfcdocs/{_physicalFilename}";
                return url.ToLower();
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
