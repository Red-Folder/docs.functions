using System;
using System.Collections.Generic;

namespace DocFunctions.Lib.Models.Audit
{
    public class AuditNode
    {
        private string _message;
        private List<AuditNode> _childMessages = null;

        //private WeakReference _temporaryParentReference;
        private AuditNode _temporaryParentReference;

        public AuditNode(string message = null)
        {
            _message = message;
        }

        private AuditNode(string message, AuditNode parent)
        {
            _message = message;
            //_temporaryParentReference = new WeakReference(parent);
            _temporaryParentReference = parent;
        }

        public void Add(string message)
        {
            Add(new AuditNode(message));
        }

        public AuditNode StartOperation(string message, AuditNode parent)
        {
            var newAuditNode = new AuditNode(message, parent);
            Add(newAuditNode);
            return newAuditNode;
        }

        public AuditNode EndOperation()
        {
            //var parent = _temporaryParentReference.Target as AuditNode;
            var parent = _temporaryParentReference;
            _temporaryParentReference = null;
            return parent;
        }

        private void Add(AuditNode message)
        {
            if (_childMessages == null) _childMessages = new List<AuditNode>();
            _childMessages.Add(message);
        }

        public void Visit(IAuditVisitor visitor)
        {
            if (_message != null)
            {
                visitor.Append(_message);
            }
            if (_childMessages != null && _childMessages.Count > 0)
            {
                if (_message != null)
                {
                    visitor.Increment();
                }

                foreach (var child in _childMessages)
                {
                    child.Visit(visitor);
                }

                if (_message != null)
                {
                    visitor.Decrement();
                }
            }
        }
    }
}
