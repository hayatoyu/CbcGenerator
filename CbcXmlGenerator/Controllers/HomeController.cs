using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CbcXmlGenerator.Controllers
{
    public class HomeController : Controller
    {
        ILog log = log4net.LogManager.GetLogger("RollingLogFileAppender1");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CbcGenerator()
        {
            return View();
        }

        [HttpPost]
        public string CbcGenerator(string act,string year,string country,string times,HttpPostedFileBase samplefile)
        {
            string timenow = DateTime.Now.ToString("yyyyMMddHH-mm-ss");
            string dirpath = Path.Combine(Server.MapPath("~/FileUpload/"), timenow);
            string filename = null,filepath = null,result = null;
            Models.CbcXmlAgent agent = null;
            StringBuilder stbr = new StringBuilder();
            Models.FileDownload file = new Models.FileDownload();
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
            if(samplefile != null)
            {
                filename = Path.GetFileName(samplefile.FileName);
                filepath = Path.Combine(dirpath, filename);
                samplefile.SaveAs(filepath);
                log.DebugFormat("Received file : {0}, Saved path : {1}", filename, filepath);
            }
            else
            {
                log.Debug("Sample File is null.");
            }
            
            if(act.Equals("1"))
            {
                // 驗證
                if(filename.Contains(".xml"))
                {
                    string validationDir = Server.MapPath("~/Validation/");
                    string xsdDirPath = Server.MapPath("~/Validation/" + country);
                    string xsdFilePath = null;
                    string ns = null;
                    string outputPath = new FileInfo(filepath).Directory.FullName + $"/{timenow}.txt";
                    DirectoryInfo xsdDir = new DirectoryInfo(xsdDirPath);
                    xsdFilePath = xsdDir.GetFiles().Where(f => f.Name.StartsWith("Cbc")).FirstOrDefault().FullName;
                    Models.NameSpaces spaces = JsonConvert.DeserializeObject<Models.NameSpaces>(System.IO.File.ReadAllText(Path.Combine(validationDir, "Namespace.json")));
                    ns = spaces.NameSpaceList.Where(n => n.CountryCode.Equals(country)).FirstOrDefault().Namespace;
                    agent = new Models.CbcXmlAgent();
                    result = agent.XmlValidate(xsdFilePath, filepath, ns, outputPath);
                }
                else
                {
                    log.Debug("Received File is not a xml file");
                    return "驗證功能請輸入xml檔案";
                }
            }
            else
            {
                // 產生Xml檔案
                if (country.Equals("GB"))
                {
                    // 英國
                    if (times.Equals("1"))
                    {
                        // 首次產生
                        agent = new Models.CbcXmlAgent(new Models.GBGenerator(year, Models.Commons.GenerateMode.Create));
                        log.DebugFormat("Generate Country : {0},Generate Year : {1},Generate Mode : {2}", "GB", year, Models.Commons.GenerateMode.Create);
                    }
                    else if (times.Equals("2"))
                    {
                        // 修改
                        agent = new Models.CbcXmlAgent(new Models.GBGenerator(year, Models.Commons.GenerateMode.Modify));
                        log.DebugFormat("Generate Country : {0},Generate Year : {1},Generate Mode : {2}", "GB", year, Models.Commons.GenerateMode.Modify);
                    }
                    else
                    {
                        // 英國沒這兩種
                        stbr.AppendLine("There is no test mode in GB.Please Check Out");
                        log.Debug(stbr.ToString());
                    }
                }
                else
                {
                    // 香港
                    if (times.Equals("1"))
                    {
                        // 首次產生
                        agent = new Models.CbcXmlAgent(new Models.HKGenerator(year, Models.Commons.GenerateMode.Create));
                        log.DebugFormat("Generate Country : {0},Generate Year : {1},Generate Mode : {2}", "HK", year, Models.Commons.GenerateMode.Create);
                    }
                    else if (times.Equals("2"))
                    {
                        // 修改
                        agent = new Models.CbcXmlAgent(new Models.HKGenerator(year, Models.Commons.GenerateMode.Modify));
                        log.DebugFormat("Generate Country : {0},Generate Year : {1},Generate Mode : {2}", "HK", year, Models.Commons.GenerateMode.Modify);
                    }
                    else if (times.Equals("3"))
                    {
                        // 首次產生(測試)
                        agent = new Models.CbcXmlAgent(new Models.HKGenerator(year, Models.Commons.GenerateMode.CreateTest));
                        log.DebugFormat("Generate Country : {0},Generate Year : {1},Generate Mode : {2}", "HK", year, Models.Commons.GenerateMode.CreateTest);
                    }
                    else if (times.Equals("4"))
                    {
                        // 修改(測試)
                        agent = new Models.CbcXmlAgent(new Models.HKGenerator(year, Models.Commons.GenerateMode.ModifyTest));
                        log.DebugFormat("Generate Country : {0},Generate Year : {1},Generate Mode : {2}", "HK", year, Models.Commons.GenerateMode.ModifyTest);
                    }
                }
                try
                {
                    if (agent != null)
                        result = agent.Generate(filepath);
                    else
                        throw new Exception(stbr.ToString());
                }
                catch(Exception e)
                {
                    log.Error(e.Message);
                    result = e.Message;
                }
                
            }
            log.DebugFormat("Return Result : {0}", result);
            return result;
        }

        [HttpGet]
        public ActionResult Download(string id)
        {
            Models.FileDownload file = new Models.FileDownload().GetFile(id);
            if(file != null)
            {
                file.DeleteFile(id);
                return File(file.FileStream, "application/octet-stream", file.FileName);
            }
            else
            {
                log.DebugFormat("File with Id {0} is null", id);
                return new EmptyResult();
            }
        }
    }
}