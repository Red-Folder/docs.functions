using DocFunctions.Lib.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Builders
{
    public interface IActionBuilder
    {
        IActionBuilder NewBlog(string blogPath);

        IActionBuilder NewImage(string blogPath, string image);

        IAction[] Build();

        IActionBuilder Clear();
    }
}
