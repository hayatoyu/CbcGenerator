using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CbcXmlGenerator.Models
{
    [XmlRoot(Namespace = "urn:oecd:ties:cbc:v1",ElementName = "CBC_OECD")]    
    public class CbcOECDGB
    {
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string schemaLocation = "urn:oecd:ties:cbc:v1 CbcXML_v1.0.xsd";

        [XmlAttribute(AttributeName = "version")]
        public string version = "String";

        public MessageSpecGB MessageSpec { get; set; }

        public CbcBodyGB CbcBody { get; set; }
    }
}