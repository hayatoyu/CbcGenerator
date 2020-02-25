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

        public CbcOECDGB()
        {
            MessageSpec = new MessageSpecGB();
            CbcBody = new CbcBodyGB();
        }
    }

    [XmlRoot(Namespace ="http://www.ird.gov.hk/AEOI/cbc/v1",ElementName = "CBC_OECD")]
    public class CbcOECDHK
    {                
        [XmlAttribute(AttributeName = "schemaLocation",Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string schemaLocation = @"http://www.ird.gov.hk/AEOI/cbc/v1 Cbc_HK_XML_v1.0.xsd";

        [XmlAttribute(AttributeName = "version")]
        public string version = "1.0";

        public MessageSpecHK MessageSpec { get; set; }

        public CbcBodyHK CbcBody { get; set; }

        public CbcOECDHK()
        {
            MessageSpec = new MessageSpecHK();
            CbcBody = new CbcBodyHK();
        }
    }
}