using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace docsFunctions.Shared.Models
{
    public class Blog
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public DateTime Modified { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public List<string> KeyWords { get; set; }
        public List<Redirect> Redirects { get; set; }

        public string Series { get; set; }

        public string ContentUrl
        {
            get
            {
                return $"{_contentBaseUrl}/{ContentFile}";
            }
        }

        private string _contentBaseUrl;
        public string ContentBaseUrl
        {
            set
            {
                _contentBaseUrl = value;
            }
        }

        public string ContentFile
        {
            get
            {
                return $"{Url}/{Url}.html";
            }
        }
    }
}
