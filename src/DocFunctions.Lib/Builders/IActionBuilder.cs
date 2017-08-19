using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Models.Github;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Builders
{
    public interface IActionBuilder
    {
        IActionBuilder NewBlog(Added added);
        IActionBuilder ModifyBlog(Modified modified);
        IActionBuilder DeleteBlog(Removed removed);

        IActionBuilder NewImage(Added added);
        IActionBuilder ModifyImage(Modified modified);
        IActionBuilder DeleteImage(Removed removed);

        IAction[] Build();

        IActionBuilder Clear();
    }
}
