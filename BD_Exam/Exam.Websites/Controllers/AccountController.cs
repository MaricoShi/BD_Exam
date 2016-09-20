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
    public class AccountController : BaseController
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

                if (model.RememberMe)
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

        public ActionResult UpdPwd() {

            UpdPwdModel _UpdPwd = new UpdPwdModel();
            _UpdPwd.UserId = CurrentUserInfo.UserID;

            return View(_UpdPwd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdPwd(UpdPwdModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "请输入相关信息。");
                return View(model);
            }
            if (model.NewPassword != model.ConfNewPassword) {
                ModelState.AddModelError("", "两次输入的新密码不相同。");
                return View(model);
            }
            var _User = CurrentContext.ESysUser.FirstOrDefault(p => p.UserID == model.UserId);
            if (_User == null)
            {
                ModelState.AddModelError("", "当前用户不存在。");
                return View(model);
            }
            if (_User.UserPwd != model.OldPassword) {
                ModelState.AddModelError("", "原密码错误。");
                return View(model);
            }
            _User.UserPwd = model.NewPassword;
            CurrentContext.SaveChanges();


            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            return Redirect(Url.Content("~/Account/Login"));

        }

    }
}
