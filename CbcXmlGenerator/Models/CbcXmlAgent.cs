using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace CbcXmlGenerator.Models
{
    public class CbcXmlAgent
    {
        IGenerator generator = null;
        StringBuilder stbr = null;
        private bool isSuccess = false;

        #region 驗證 Xml
        public void XmlValidate(string xsdPath,string xmlPath,string nameSpace,string outputPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(nameSpace, XmlReader.Create(xsdPath));
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(XmlSettingsValidationEventHandler);

            XmlReader reader = XmlReader.Create(xmlPath, settings);

            while(reader.Read()) { }

            if((stbr == null || stbr.Length == 0) && isSuccess)
            {
                stbr.AppendLine("驗證成功");
            }
            using (StreamWriter outputFile = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                outputFile.WriteLine(stbr.ToString());
            }
                
        }

        public void XmlSettingsValidationEventHandler(object sender,ValidationEventArgs e)
        {
            stbr = new StringBuilder();
            if(e.Severity == XmlSeverityType.Warning)
            {
                stbr.AppendFormat("WARNING: {0}\n", e.Message);
                isSuccess = false;
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                stbr.AppendFormat("WARNING: {0}\n", e.Message);
                isSuccess = false;
            }
            isSuccess = true;
        }

        #endregion
        
    }
}