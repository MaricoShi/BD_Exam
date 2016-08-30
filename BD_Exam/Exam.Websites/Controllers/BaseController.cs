using Exam.Data;
using Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Exam.Websites.Controllers
{
    [AppAuth]
    [ExceFilter]
    public class BaseController : Controller
    {
        //当前上下文
        protected ExamEntities CurrentContext = new ExamEntities();

        //操作反馈定义
        protected HandleRetBackModel _RetBack = 
            new HandleRetBackModel() { retCode = "ERR001", retMsg = "操作过程出错" };

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        protected ESysUser CurrentUserInfo
        {
            get
            {
                try
                {
                    //string _UserC = User.Identity.Name;
                    //if (string.IsNullOrWhiteSpace(_UserC)) return null;

                    var _Ticket = ((System.Web.Security.FormsIdentity)(User.Identity)).Ticket;

                    ESysUser _User = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<ESysUser>(_Ticket.UserData);
                    if (_User == null) {
                        Response.Cookies.Clear();
                        FormsAuthentication.SignOut();

                        Response.Redirect(Url.Content("~/Account/Login"), true);
                    }

                    return _User;
                }
                catch (Exception) { return null; }
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ViewData["CurrentUserInfo"] = CurrentUserInfo;
        }

        /// 获取IP
        /// </summary>
        /// <returns></returns>
        protected string GetIP()
        {
            string ip = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Request.ServerVariables["HTTP_VIA"]))
                    ip = Convert.ToString(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                if (string.IsNullOrEmpty(ip))
                    ip = Convert.ToString(Request.ServerVariables["REMOTE_ADDR"]);
            }
            catch (Exception)
            {
            }
            return ip;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && CurrentContext != null)
            {
                CurrentContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// 处理返回实体
    /// </summary>
    public class HandleRetBackModel
    {
        /// <summary>
        /// 返回代码，000000成功
        /// </summary>
        public string retCode { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string retMsg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object retData { get; set; }
    }
}
