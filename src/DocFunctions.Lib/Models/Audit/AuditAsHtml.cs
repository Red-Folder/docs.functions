using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Audit
{
    public class AuditAsHtml : IAuditVisitor
    {
        private AuditTree _target;
        private StringBuilder _htmlBuilder = new StringBuilder();

        public AuditAsHtml(AuditTree target)
        {
            _target = target;
        }

        public void Append(DateTime created, string message)
        {
            _htmlBuilder.AppendLine("<li>");
            _htmlBuilder.AppendLine($"[{created.ToString("dd/MM/yyyy HH:mm:ss")}]: {message}");
            _htmlBuilder.AppendLine("</li>");
        }

        public void Decrement()
        {
            _htmlBuilder.AppendLine("</ul>");
        }

        public void Increment()
        {
            _htmlBuilder.AppendLine("<ul>");
        }

        public override string ToString()
        {
            Increment();
            _target.Visit(this);
            Decrement();
            return _htmlBuilder.ToString();
        }
    }
}
