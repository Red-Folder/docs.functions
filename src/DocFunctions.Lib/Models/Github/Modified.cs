using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Github
{

    public class Modified: AbstractAction
    {
        public static implicit operator Added(Modified modified)
        {
            return new Added
            {
                FullFilename = modified.FullFilename,
                CommitSha = modified.CommitSha,
                CommitShaForRead = modified.CommitShaForRead
            };
        }

        public static implicit operator Removed(Modified modified)
        {
            return new Removed
            {
                FullFilename = modified.FullFilename,
                CommitSha = modified.CommitSha,
                CommitShaForRead = modified.CommitShaForRead
            };
        }
    }
}
