using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;     

namespace CbcXmlGenerator.Models
{
    public class Commons
    {
        public static readonly string connStr = WebConfigurationManager.ConnectionStrings["DefaultDB"].ConnectionString;

        public enum GenerateMode
        {
            Create, Modify,CreateTest,ModifyTest
        }

        public class CurrCode
        {
            [XmlAttribute]
            public string currCode { get; set; }

            public CurrCode()
            {
                currCode = "TWD";
            }
        }

        public class IssuedBy
        {
            [XmlAttribute]
            public string issuedBy { get; set; }

            public IssuedBy()
            {
                issuedBy = "TW";
            }
        }
    }
}