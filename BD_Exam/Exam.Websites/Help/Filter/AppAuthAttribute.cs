using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam.Websites
{
    public class AppAuthAttribute : AuthorizeAttribute
    {
        public int erroPage = 0;

        /// <summary>
        /// 核心【验证用户是否登陆】
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if (!httpContext.Request.IsAuthenticated)
            {
                httpContext.Response.Redirect("~/Account/Login?returnUrl=" +
                    httpContext.Request.FilePath, true);
                return false;
            }
            return true;
        }
    }
}