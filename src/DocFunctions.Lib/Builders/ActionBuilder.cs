using DocFunctions.Lib.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Builders
{
    public class ActionBuilder : IActionBuilder
    {
        private List<IAction> _actions = new List<IAction>();

        public IAction[] Build()
        {
            return _actions.ToArray();
        }

        public IActionBuilder NewBlog(string blogPath)
        {
            _actions.Add(new NewBlogAction(blogPath, null));

            return this;
        }
    }
}
