﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbcXmlGenerator.Models
{
    public interface IGenerator
    {
        string Generate(string path);
    }
}
