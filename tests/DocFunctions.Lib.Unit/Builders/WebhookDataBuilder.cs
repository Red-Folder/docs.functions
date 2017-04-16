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
                Commits = new Commit[]
                {
                    new Commit
                    {
                        Added = new string[]
                        {
                            "2017-04-10-20-27-54/Image.jpg",
                            "2017-04-10-20-27-54/blog.json",
                            "2017-04-10-20-27-54/blog.md"
                        }
                   }
                }
            };
        }
    }
}
