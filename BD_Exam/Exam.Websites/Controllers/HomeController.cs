using Exam.Data;
using Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam.Websites.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            try
            {
                
            }
            catch (Exception)
            {
                
            }
            
            return View();
        }

        public ActionResult Test1()
        {
            System.Threading.Thread.Sleep(500);
            return View();
        }

        public ActionResult Test2()
        {
            System.Threading.Thread.Sleep(500);
            return View();
        }

        public ActionResult Test3()
        {
            System.Threading.Thread.Sleep(500);
            return View();
        }

    }
}
