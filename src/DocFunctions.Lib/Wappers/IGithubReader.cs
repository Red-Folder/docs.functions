using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Wappers
{
    public interface IGithubReader
    {
        string GetRawFile(string path, string commitSha);
        byte[] GetRawImageFile(string path, string commitSha);
        Task<Models.Github.Commit> BuildCommitForFullRepoSync();
    }
}
