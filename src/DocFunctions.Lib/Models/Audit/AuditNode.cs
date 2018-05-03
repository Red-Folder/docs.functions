using System;
using System.Collections.Generic;
using System.Linq;

namespace DocFunctions.Lib.Models.Audit
{
    public class AuditNode
    {
        private string _message;
        private Exception _ex;

        private DateTime _created = DateTime.Now;
        private List<AuditNode> _childMessages = null;

        private bool _hasErrorred = false;

        //private WeakReference _temporaryParentReference;
        private AuditNode _temporaryParentReference;

        public bool HasErrorred { get => _hasErrorred; set => _hasErrorred = value; }

        public AuditNode(string message = null)
        {
            _message = message;
        }

        public AuditNode(Exception ex)
        {
            _ex = ex;
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

        public void Add(Exception ex)
        {
            Add(new AuditNode(ex));

            // Set Parent to error
            _temporaryParentReference._hasErrorred = true;
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
                visitor.Append(_created, _message);
            }

            if (_ex != null)
            {
                var messages = ExceptionToMessages();
                visitor.Append(_created, messages);
            }

            if (_childMessages != null && _childMessages.Count > 0)
            {
                if (_hasErrorred || (_childMessages.Any(x => x._childMessages != null && x._childMessages.Count > 0)))
                {
                    if (_message != null)
                    {
                        visitor.Increment(_hasErrorred);
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

        private List<string> ExceptionToMessages()
        {
            var messages = new List<string>();

            if (_ex is AggregateException)
            {
                (_ex as AggregateException).InnerExceptions.ToList().ForEach(x =>
                {
                    messages.Add(x.Message);
                    messages.Add(x.StackTrace);
                });
            }
            else
            {
                messages.Add(_ex.Message);
                messages.Add(_ex.StackTrace);
            }

            return messages;
        }
    }
}
