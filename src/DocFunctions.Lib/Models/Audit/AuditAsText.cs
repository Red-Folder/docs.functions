using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Audit
{
    public class AuditAsText: IAuditVisitor
    {
        private AuditTree _target;

        private StringBuilder _text = new StringBuilder();
        private int _incrementLevel = 0;

        public AuditAsText(AuditTree auditTree)
        {
            _target = auditTree;
        }

        public void Increment(bool hasErrorred)
        {
            _incrementLevel++;
        }

        public void Decrement()
        {
            _incrementLevel--;
        }

        public void Append(DateTime created, string message)
        {
            if (_incrementLevel > 0)
            {
                _text.Append(new String('\t', _incrementLevel));
            }
            _text.AppendLine($"[{created.ToString("dd/MM/yyyy HH:mm:ss")}]: {message}");
        }

        public void Append(DateTime created, List<string> messages)
        {
            messages.ForEach(x => Append(created, x));
        }

        public override string ToString()
        {
            _target.Visit(this);
            return _text.ToString();
        }
    }
}
