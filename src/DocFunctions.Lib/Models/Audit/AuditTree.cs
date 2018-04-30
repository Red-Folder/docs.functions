using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Models.Audit
{
    public class AuditTree
    {
        private AuditNode _baseNode;
        private AuditNode _currentNode;
        private bool _hasFailed = false;
        private ILogger _log = null;

        private IDisposable _context = null;

        public AuditTree()
        {
            _baseNode = new AuditNode();
            _currentNode = _baseNode;
        }


        public AuditTree(ILogger log)
        {
            _baseNode = new AuditNode();
            _currentNode = _baseNode;
            _log = log;
        }

        public void BeginContext(string requestId)
        {
            if (_context != null) throw new ApplicationException("Already within a context");

            _context = LogContext.PushProperty("RequestID", requestId);
        }

        public void EndContext()
        {
            if (_context == null) throw new ApplicationException("Not within a context");

            _context.Dispose();
            _context = null;
        }


        public void Audit(string message)
        {
            _currentNode.Add(message);

            Information(message);
        }

        public void Information(string message)
        {
            if (_log != null) _log.Information(message);
        }

        public void Error(string message, Exception ex)
        {
            if (_log != null) _log.Error(ex, message);

            _currentNode.Add(message);

            _hasFailed = true;
        }

        public void StartOperation(string message)
        {
            _currentNode = _currentNode.StartOperation(message, _currentNode);
        }

        public void EndOperation()
        {
            _currentNode = _currentNode.EndOperation();

            if (_currentNode == null)
            {
                _currentNode = _baseNode;
            }
        }

        public void Visit(IAuditVisitor visitor)
        {
            _baseNode.Visit(visitor);
        }
    }
}
