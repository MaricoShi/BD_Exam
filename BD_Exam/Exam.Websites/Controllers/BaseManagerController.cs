using Exam.Websites.Help.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exam.Data;
using Exam.Data.Models;
using Exam.Websites.Models;

namespace Exam.Websites.Controllers
{
    public class BaseManagerController : Controller
    {
        ExamEntities _DB = new ExamEntities();
        public new void Dispose() {
            if (_DB != null)
                _DB.Dispose();
        }


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
            List<OrgViewModel> _list = new List<OrgViewModel>();
            try
            {

                _list = string.IsNullOrEmpty(ParentCode) ?
                    _DB.EBasOrg.Where(o => o.IsDeleted == false && string.IsNullOrEmpty(o.ParentCode)).OrderBy(s => s.SortCode)
                    .Select(p=>new OrgViewModel {
                    OrgCode=p.OrgCode,
                    OrgName=p.OrgName,
                    HasChildren=_DB.EBasOrg.Any(o=>o.ParentCode==p.OrgCode&&p.IsDeleted==false)
                    }).ToList()
                    :
                    _DB.EBasOrg.Where(o => o.IsDeleted == false && o.ParentCode == ParentCode).OrderBy(s => s.SortCode)
                    .Select(p => new OrgViewModel
                    {
                        OrgCode = p.OrgCode,
                        OrgName = p.OrgName,
                        HasChildren = _DB.EBasOrg.Any(o => o.ParentCode == p.OrgCode && p.IsDeleted == false)
                    })
                    .ToList();


            }
            catch (Exception)
            {

            }
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取机构数据
        [HttpPost]
        public ActionResult OrgOperate(string type, EBasOrg data)
        {
            bool ret = false;
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    using (ExamEntities _db = _DB)
                    {
                        switch (type)
                        {
                            case "add":
                                data.IsDeleted = false;
                                data.CreateTime = DateTime.Now;
                                //data.CreateBy=当前用户
                                //data.CreateBy=当前用户
                                break;
                            case "modify":
                                break;
                            case "delete":
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return Json(ret);
        }
        #endregion
        #endregion
      
    }
}
