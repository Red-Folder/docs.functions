using docsFunctions.Shared.Models;
using System.Collections.Generic;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeRepoDataManager
    {
        private List<Blog> _repo = new List<Blog>();

        public void SaveBlogToRepo(Blog blogMeta)
        {
            _repo.Add(blogMeta);
        }

        public void DeleteBlogFromRepo(string blogUrl)
        {
            _repo.RemoveAll(x => x.Url == blogUrl);
        }
    }
}
