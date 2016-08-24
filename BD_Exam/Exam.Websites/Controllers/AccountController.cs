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
    [AppAuth]
    [ExceFilter]
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
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
                var _User = _db.ESysUser.FirstOrDefault(p => p.UserCode.ToUpper() == model.UserName.Trim().ToUpper());
                if (_User == null) {
                    ModelState.AddModelError("", "当前用户不存在。");
                    return View(model);
                }
                if (_User.UserPwd != model.Password) {
                    ModelState.AddModelError("", "密码错误，请输入正确密码。");
                    return View(model);
                }

                _User.UserPwd = null;

                FormsAuthentication.SetAuthCookie(_User.UserID, true);
                var _UserData = Newtonsoft.Json.JsonConvert.SerializeObject(_User);

                FormsAuthenticationTicket _Ticket = new FormsAuthenticationTicket(1,
                    _User.UserID, DateTime.Now, DateTime.Now.AddMonths(1), true, _UserData);
                HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                    FormsAuthentication.Encrypt(_Ticket));//加密身份信息，保存至Cookie

                Cookie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(Cookie);
            }

            return RedirectToLocal(returnUrl);

        }


        public ActionResult Logout()
        {
            Response.Cookies.Clear();
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
