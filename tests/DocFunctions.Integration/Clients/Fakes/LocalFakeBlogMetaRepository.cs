using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeBlogMetaRepository : IBlogMetaRepository
    {
        private LocalFakeDataManager _dataManager;

        public LocalFakeBlogMetaRepository(LocalFakeDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public void Delete(string blogUrl)
        {
            _dataManager.DeleteBlogFromRepo(blogUrl);
        }

        public global::Functional.Maybe.Maybe<IList<global::docsFunctions.Shared.Models.Blog>> Get()
        {
            throw new NotImplementedException();
        }

        public global::Functional.Maybe.Maybe<global::docsFunctions.Shared.Models.Blog> Get(string blogUrl)
        {
            throw new NotImplementedException();
        }

        public void Save(global::docsFunctions.Shared.Models.Blog blogMeta)
        {
            _dataManager.SaveBlogToRepo(blogMeta);
        }
    }
}
