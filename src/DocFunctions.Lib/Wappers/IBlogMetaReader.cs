﻿using docsFunctions.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Wappers
{
    public interface IBlogMetaReader
    {
        Blog Transform(string metaJson);
    }
}