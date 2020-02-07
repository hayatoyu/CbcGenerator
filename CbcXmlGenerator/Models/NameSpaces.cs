using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CbcXmlGenerator.Models
{
    public class NameSpaces
    {
        public CbcNameSpace[] NameSpaceList;

        public class CbcNameSpace
        {
            public string CountryCode { get; set; }
            public string Namespace { get; set; }
        }
    }
}