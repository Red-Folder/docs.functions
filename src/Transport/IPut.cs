using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedFolder.Transport
{
    public interface IPut
    {
        void Put(byte[] payload);
    }
}
