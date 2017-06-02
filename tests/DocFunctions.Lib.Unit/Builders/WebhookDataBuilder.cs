using DocFunctions.Lib.Models.Github;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Unit.Builders
{
    public class WebhookDataBuilder
    {
        public WebhookData Build()
        {
            return new WebhookData
            {
                Commits = new List<Commit>
                {
                    new Commit
                    {
                        Added = new List<Added>
                        {
                            new Added { FullFilename = "2017-04-10-20-27-54/Image.jpg" },
                            new Added { FullFilename = "2017-04-10-20-27-54/blog.json" },
                            new Added { FullFilename = "2017-04-10-20-27-54/blog.md" }
                        },
                        Removed = new List<Removed>
                        {
                            new Removed { FullFilename = "2017-04-10-20-27-54/Image.jpg" },
                            new Removed { FullFilename = "2017-04-10-20-27-54/blog.json" },
                            new Removed { FullFilename = "2017-04-10-20-27-54/blog.md" }
                        }
                   }
                }
            };
        }
    }
}
