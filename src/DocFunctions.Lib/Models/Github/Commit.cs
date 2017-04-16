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
        [DataMember(Name = "added")]
        public string[] Added;
    }
}
