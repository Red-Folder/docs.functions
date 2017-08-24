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

        private static AuditTree _instance;

        public static AuditTree Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AuditTree(null);
                }
                return _instance;
            }
        }

        private AuditTree(string message)
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
        }

        public void Visit(IAuditVisitor visitor)
        {
            _baseNode.Visit(visitor);
        }
    }
}
