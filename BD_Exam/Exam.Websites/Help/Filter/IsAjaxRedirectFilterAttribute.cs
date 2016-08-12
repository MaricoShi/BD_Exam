using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam.Websites.Help.Filter
{
    /// <summary>
    /// 拦截Action的处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class IsAjaxRedirectFilterAttribute : ActionFilterAttribute
    {

        public string _RedirectUrl;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var _Request = filterContext.HttpContext.Request;
            var _Response = filterContext.HttpContext.Response;

            if (!filterContext.IsChildAction  
                && !_Request.IsAjaxRequest() 
                && !string.IsNullOrWhiteSpace(_RedirectUrl))
            {
                if (_RedirectUrl.Contains("?"))
                {
                    _RedirectUrl = _RedirectUrl.Substring(0, _RedirectUrl.IndexOf('?'));
                }
                _RedirectUrl += "?ac=" + filterContext.ActionDescriptor.ActionName;
                _Response.Redirect(_RedirectUrl);
            }
        }

    }
}