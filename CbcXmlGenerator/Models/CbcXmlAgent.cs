using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CbcXmlGenerator.Models
{
    public class CbcXmlAgent
    {
        ILog log = log4net.LogManager.GetLogger(typeof(CbcXmlAgent));
        IGenerator generator = null;
        StringBuilder stbr = null;
        private bool isSuccess = true;

        public CbcXmlAgent()
        {
            stbr = new StringBuilder();
        }

        public CbcXmlAgent(IGenerator g) : this()
        {
            generator = g;
        }

        #region 驗證 Xml
        public string XmlValidate(string xsdPath,string xmlPath,string nameSpace,string outputPath)
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
            reader.Close();

            // 驗證完要加入到資料庫
            FileInfo fileInfo = new FileInfo(outputPath);
            FileDownload filedownload = new FileDownload();
            filedownload.FileName = fileInfo.Name;
            filedownload.Id = fileInfo.Name.Replace(".txt", "");
            filedownload.FileStream = File.ReadAllBytes(outputPath);
            filedownload.CreateDate = DateTime.Now;
            filedownload = filedownload.Insert(filedownload);

            // 刪除檔案
            fileInfo.Directory.Delete(true);
            log.DebugFormat("Delete {0},{1} and located folder",xmlPath, outputPath);

            return JsonConvert.SerializeObject(new { Id = filedownload.Id });
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

        #region 產生 Xml
        public string Generate(string filepath)
        {
            return generator.Generate(filepath);
        }
        #endregion
    }
}