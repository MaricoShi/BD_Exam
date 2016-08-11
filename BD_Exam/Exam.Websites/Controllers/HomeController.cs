using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam.Websites.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test1()
        {
            System.Threading.Thread.Sleep(2000);
            return View();
        }

        public ActionResult Test2()
        {
            System.Threading.Thread.Sleep(2000);
            return View();
        }

        public ActionResult Test3()
        {
            System.Threading.Thread.Sleep(2000);
            return View();
        }

    }
}
