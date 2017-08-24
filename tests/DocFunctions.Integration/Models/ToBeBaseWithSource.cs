using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Models
{
    public class ToBeBaseWithSource: ToBeBase
    {
        internal string _sourceFilename;

        public ToBeBaseWithSource(string repoFilename, string sourceFilename) : base(repoFilename)
        {
            _sourceFilename = sourceFilename;
        }

        public string SourceFilename
        {
            get
            {
                return _sourceFilename;
            }
        }
    }
}
