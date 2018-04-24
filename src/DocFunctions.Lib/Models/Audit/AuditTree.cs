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

        public AuditTree(string message)
        {
            _baseNode = new AuditNode(message);
            _currentNode = _baseNode;
        }

        public void Add(string message)
        {
            _currentNode.Add(message);
        }

        public void AddFailure(string message)
        {
            Add(message);
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
