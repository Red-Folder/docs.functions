using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Clients
{
    public interface IRepoClient
    {
        void AddFileToCommit(string repoFilename, string sourceFilename);
        void DeleteFileFromCommit(string repoFilename);
        void ModifyFileInCommit(string repoFilename, string sourceFilename);
        void PushCommit(string commitMessage);
    }
}
