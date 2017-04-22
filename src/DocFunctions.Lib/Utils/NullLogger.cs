using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Utils
{
    public class NullLogger : ILogger
    {
        public object StartOperation(string operationName)
        {
            return null;
        }

        public void EndOperation(object state)
        {
        }

        public void Info(string message)
        {
        }
    }
}
