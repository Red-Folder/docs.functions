using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Github.Raw
{
    public class WebhookData
    {
        public string Before;
        public string After;
        public Commit[] Commits;
    }
}
