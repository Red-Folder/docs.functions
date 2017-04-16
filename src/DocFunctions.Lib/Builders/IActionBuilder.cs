﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Builders
{
    public interface IActionBuilder
    {
        IActionBuilder NewBlog(string blogPath);
        
        void Build();
    }
}
