using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Models
{
    public class ToBeModified : ToBeBaseWithSource
    {
        public ToBeModified(string repoFilename, string sourceFilename) : base(repoFilename, sourceFilename)
        {
        }
    }
}
