using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CbcXmlGenerator.Controllers
{
    public class HomeController : Controller
    {
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
        public string CbcGenerator(string act,string country,string times,HttpPostedFileBase samplefile)
        {
            
        }

        [HttpGet]
        public ActionResult Download()
        {
            return new EmptyResult();
        }
    }
}