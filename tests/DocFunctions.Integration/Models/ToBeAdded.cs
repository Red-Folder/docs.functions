using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Models
{
    public class ToBeAdded : ToBeBaseWithSource
    {
        public ToBeAdded(string repoFilename, string sourceFilename) : base(repoFilename, sourceFilename)
        {
        }
    }
}
