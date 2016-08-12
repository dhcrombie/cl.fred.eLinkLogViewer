using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELinkLogViewer.Controllers
{
    public class LogViewerController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Details(int id)
        {
            ViewBag.LogItem = Services.LogReader.GetLogItem(id);
            return View();
        }
    }
}
