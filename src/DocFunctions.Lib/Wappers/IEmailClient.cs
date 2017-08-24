using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Wappers
{
    public interface IEmailClient
    {
        void Send(string emailContent);
    }
}
