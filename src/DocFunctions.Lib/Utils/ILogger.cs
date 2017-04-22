using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Utils
{
    public interface ILogger
    {
        object StartOperation(string operationName);

        void EndOperation(object state);

        void Info(string message);
    }
}
