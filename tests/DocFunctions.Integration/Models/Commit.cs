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

        public class ToBeAdded
        {
            public string RepoFilename;
            public string SourceFilename;

            public ToBeAdded(string repoFilename, string sourceFilename)
            {
                RepoFilename = repoFilename;
                SourceFilename = sourceFilename;
            }
        }

        public class ToBeDeleted
        {
            public string RepoFilename;

            public ToBeDeleted(string repoFilename)
            {
                RepoFilename = repoFilename;
            }
        }

        public class ToBeModified
        {
            public string RepoFilename;
            public string SourceFilename;

            public ToBeModified(string repoFilename, string sourceFilename)
            {
                RepoFilename = repoFilename;
                SourceFilename = sourceFilename;
            }
        }
    }
}
