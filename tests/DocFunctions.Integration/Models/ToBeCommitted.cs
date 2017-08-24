using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Models
{
    public class ToBeCommitted
    {
        public List<ToBeAdded> ToAdd = new List<ToBeAdded>();
        public List<ToBeModified> ToModify = new List<ToBeModified>();
        public List<ToBeDeleted> ToDelete = new List<ToBeDeleted>();
    }
}
