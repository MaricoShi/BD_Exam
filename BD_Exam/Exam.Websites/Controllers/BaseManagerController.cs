using Exam.Websites.Help.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exam.Data;
using Exam.Data.Models;

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
        [IsAjaxRedirectFilter(_RedirectUrl = "~/BaseManager/Index")]
        public ActionResult PersonQuery()
        {
            return View();
        }

        #region 单位档案室页面
        /// <summary>
        /// 单位档案室
        /// </summary>
        /// <returns></returns>
        [IsAjaxRedirectFilter(_RedirectUrl = "~/BaseManager/Index")]
        public ActionResult OrgQuery() 
        {
            return View();
        }
        #endregion

        #region 人员新增修改查看页面
        /// <summary>
        /// 人员新增修改查看
        /// </summary>
        /// <returns></returns>
        [IsAjaxRedirectFilter(_RedirectUrl = "~/BaseManager/Index")]
        public ActionResult PerSonDetail()
        {
            return View();
        }
        #endregion

        #region 机构数据
        #region 机构数据页面
        /// <summary>
        /// 机构数据
        /// </summary>
        /// <returns></returns>
        [IsAjaxRedirectFilter(_RedirectUrl = "~/BaseManager/Index")]
        public ActionResult OrgDatas()
        {
            return View();
        }
        #endregion

        #region 获取机构数据
        [HttpGet]
        public JsonResult getOrgDataList(string ParentCode)
        {
            List<EBasOrg> _list = new List<EBasOrg>();
            try
            {
                using (ExamEntities context = new ExamEntities())
                {

                    _list = string.IsNullOrEmpty(ParentCode) ?
                        context.EBasOrg.Where(o => o.IsDeleted == false && string.IsNullOrEmpty(o.ParentCode)).OrderBy(s => s.SortCode).ToList()
                        :
                        context.EBasOrg.Where(o => o.IsDeleted == false && o.ParentCode == ParentCode).OrderBy(s => s.SortCode).ToList();

                }
            }
            catch (Exception)
            {

            }
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
      
    }
}
