using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam.Websites.Controllers
{
    public class BaseManagerController : Controller
    {
        //
        // GET: /BaseManager/

        public ActionResult Index(string ac = "PersonQuery")
        {
            ViewBag.ActionName = ac;
            return View();
        }

        /// <summary>
        /// 人员档案室
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonQuery()
        {
            return View();
        }

        /// <summary>
        /// 单位档案室
        /// </summary>
        /// <returns></returns>
        public ActionResult OrgQuery() 
        {
            return View();
        }

    }
}
