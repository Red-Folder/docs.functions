using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Audit
{
    public interface IAuditVisitor
    {
        void Append(string message);
        void Increment();
        void Decrement();
    }
}
