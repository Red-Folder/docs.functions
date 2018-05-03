using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Audit
{
    public interface IAuditVisitor
    {
        void Append(DateTime created, string message);
        void Append(DateTime created, List<string> message);
        void Increment(bool hasErrorred);
        void Decrement();
    }
}
