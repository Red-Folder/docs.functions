using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Wappers
{
    public interface IFtpsClient
    {
        void Upload(string filename, string contents);
        void Upload(string filename, byte[] contents);
        void Delete(string filename);
    }
}
