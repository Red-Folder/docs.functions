using docsFunctions.Shared.Models;
using Functional.Maybe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Wappers
{
    public interface IBlogMetaRepository
    {
        void Save(Blog blogMeta);
        Maybe<IList<Blog>> Get();
        Maybe<Blog> Get(string blogUrl);
        void Delete(Blog blogMeta);
    }
}
