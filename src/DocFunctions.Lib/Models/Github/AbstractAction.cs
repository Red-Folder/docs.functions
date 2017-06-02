using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Github
{
    public abstract class AbstractAction
    {
        public string FullFilename { get; set; }
        public string CommitSha { get; set; }
        public string CommitShaForRead { get; set; }

        public string Path
        {
            get
            {
                if (FullFilename.Contains("/"))
                {
                    return FullFilename.Replace("/" + Filename, "");
                }
                else
                {
                    return FullFilename.Replace(Filename, "");
                }
            }
        }

        public string Filename
        {
            get
            {
                return System.IO.Path.GetFileName(FullFilename);
            }
        }

        public bool IsBlogFile
        {
            get
            {
                return (Extension == ".md" || Extension == ".json");
            }
        }

        public bool IsImageFile
        {
            get
            {
                return (Extension == ".png" || Extension == ".jpg" || Extension == ".gif");
            }
        }

        private string Extension
        {
            get
            {
                return System.IO.Path.GetExtension(FullFilename);
            }
        }
    }
}
