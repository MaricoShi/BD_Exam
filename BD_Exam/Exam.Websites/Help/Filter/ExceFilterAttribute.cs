using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Exam.Websites
{
    /// <summary>
    /// 拦截Action的处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            //产生异常
            if (filterContext.Exception != null)
            {
                filterContext.Controller.ViewData["ErrorMessage"] =
                    String.Format(@"<b>异常消息: {0}
                        </br><b>触发Controller:{1}
                        </br><b>触发Action:{2}</p>
                        </br><b>异常类型:  </b>{3}",
                    filterContext.Exception.GetBaseException().Message,
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName,
                    filterContext.Exception.GetBaseException().GetType().ToString());
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error",
                    ViewData = filterContext.Controller.ViewData,
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}