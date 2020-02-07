using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace CbcXmlGenerator.Models
{
    public class StringWriterWithEncodings : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncodings() : this(Encoding.UTF8) { }

        public StringWriterWithEncodings(Encoding en)
        {
            this.encoding = en;
        }

        public override Encoding Encoding
        {
            get
            {
                return encoding;
            }
        }
    }
}