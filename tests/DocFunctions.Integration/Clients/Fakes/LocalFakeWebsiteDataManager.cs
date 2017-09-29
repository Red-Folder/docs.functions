using DocFunctions.Integration.Clients.Fakes.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeWebsiteDataManager
    {
        private List<WebsiteItem> _website = new List<WebsiteItem>();

        public void AddBlogToWebsite(string filename, string content)
        {
            _website.Add(new WebsiteItem(filename.ToLower(), content));
        }

        public void AddImageToWebsite(string filename, byte[] content)
        {
            _website.Add(new WebsiteItem(filename.ToLower(), content));
        }

        public void DeleteFromWebsite(string filename)
        {
            _website.RemoveAll(x => x.PhysicalFilename.ToLower() == filename.ToLower());
        }

        public bool UrlExists(string url)
        {
            var websiteItem = Get(url);
            return (websiteItem != null);
        }

        public long UrlSize(string url)
        {
            var websiteItem = Get(url);
            if (websiteItem != null)
            { 
                var content = websiteItem.Contents;
                if (content is byte[])
                {
                    return (content as byte[]).Length;
                }
                else
                {
                    return (content as string).Length;
                }
            }
            else
            {
                throw new Exception("Url not found");
            }
        }

        public string UrlContent(string url)
        {
            var websiteItem = Get(url);
            if (websiteItem != null)
            {
                var content = websiteItem.Contents;
                if (content is byte[])
                {
                    throw new Exception("Url not in expected format");
                }
                else
                {
                    return (content as string);
                }
            }
            else
            {
                throw new Exception("Url not found");
            }
        }

        private WebsiteItem Get(string url)
        {
            var cleanUrl = url.Split('?')[0].ToLower();
            try
            {
                return _website.Where(x => x.FullUrl == cleanUrl).Single();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
