using DocFunctions.Lib.Models.Github;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Wappers
{
    public interface IToBeProcessed
    {
        void Add(string id, Commit commit);
        Commit Get(string id);
        void MarkCompleted(string id);
    }
}
