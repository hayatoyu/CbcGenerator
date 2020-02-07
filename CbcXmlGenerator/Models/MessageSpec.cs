using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace CbcXmlGenerator.Models
{
    [XmlRoot(Namespace = "urn:oecd:ties:cbc:v1")]
    [XmlType(Namespace = "urn:oecd:ties:cbc:v1")]
    public class MessageSpecGB
    {
        public string SendingEntityIN { get; set; }
        public string TransmittingCountry { get; set; }
        public string ReceivingCountry { get; set; }
        public string MessageType { get; set; }
        public string Language { get; set; }
        public string Warning { get; set; }
        public string Contact { get; set; }
        
        public string MessageRefId
        {
            get
            {
                return $"GB{ReportingPeriod.Substring(0,4)}GB{SendingEntityIN}{MessageTypeIndic}{DateTime.Now.ToString("yyyyMMdd'T'hhmmss")}001";
            }
            set
            {

            }
        }

        public string MessageTypeIndic { get; set; }
        public string ReportingPeriod { get; set; }

        private DateTime _timestamp;
        public string Timestamp
        {
            get
            {
                return _timestamp.ToString("yyyy-MM-dd'T'HH:mm:ss");
            }
            set
            {
                DateTime.TryParse(value, out _timestamp);
            }
        }
    }

    public class MessageSpecHK
    {

    }
}