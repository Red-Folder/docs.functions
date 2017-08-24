using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Models
{
    public class Commit
    {
        public List<ToBeAdded> ToAdd = new List<ToBeAdded>();
        public List<ToBeModified> ToModify = new List<ToBeModified>();
        public List<ToBeDeleted> ToDelete = new List<ToBeDeleted>();

        public partial class ToBeBase
        {
            internal string _repoFilename;

            public ToBeBase(string repoFilename)
            {
                _repoFilename = repoFilename;
            }

            public string RepoFilename
            {
                get
                {
                    return _repoFilename;
                }
            }
        }

        public partial class ToBeBaseWithSource: ToBeBase
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

        public class ToBeAdded: ToBeBaseWithSource
        {
            public ToBeAdded(string repoFilename, string sourceFilename): base(repoFilename, sourceFilename)
            {
            }
        }

        public class ToBeDeleted: ToBeBase
        {
            public ToBeDeleted(string repoFilename): base(repoFilename)
            {
            }
        }

        public class ToBeModified: ToBeBaseWithSource
        {
            public ToBeModified(string repoFilename, string sourceFilename): base (repoFilename, sourceFilename)
            {
            }
        }
    }
}
