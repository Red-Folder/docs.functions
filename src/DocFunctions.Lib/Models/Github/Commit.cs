using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Github
{
    public class Commit
    {
        public string Sha;
        public string Message;

        public List<Added> Added = new List<Added>();

        public List<Removed> Removed = new List<Removed>();

        public List<Modified> Modified = new List<Modified>();
    }
}
