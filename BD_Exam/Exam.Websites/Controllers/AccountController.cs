using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Exam.Websites.Models;
using System.Web.Security;
using Exam.Data;

namespace Exam.Websites.Controllers
{
    [ExceFilter]
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "请输入用户名或密码。");
                return View(model);
            }

            using (ExamEntities _db = new ExamEntities())
            {

            }

            //var cookie = new HttpCookie("Exam.Websites.Login");
            //var cv = Newtonsoft.Json.JsonConvert.SerializeObject(car);
            //cookie.Value = cv.AESEncrypt(AESKey, AESIV);
            //cookie.Expires = DateTime.Now.AddMonths(1);
            //Response.Cookies.Add(cookie);
            //FormsAuthentication.SetAuthCookie(car.CarId, true);

            return RedirectToLocal(returnUrl);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect(Url.Content("~/Account/Login"));
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
