using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Github.Raw
{
    public class Commit
    {
        public string Id;
        public string Message;
        public DateTime Timestamp;
        public string[] Added;
        public string[] Removed;
        public string[] Modified;
    }
}
