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
    public class BaseManagerController : BaseController
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
            KeyValue _ret = new KeyValue() { Key = "error",Value="未做修改"};
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    using (ExamEntities _db = _DB)
                    {
                        var _parentorg=_db.EBasOrg.FirstOrDefault(org=>org.OrgCode==data.ParentCode);
                        if (_parentorg != null)
                        {
                            switch (type)
                            {
                                #region 新增
                                case "add":
                                    if (_db.EBasOrg.Any(o => o.OrgCode == data.OrgCode))
                                    {
                                        _ret.Key = "error";
                                        _ret.Value = "系统已经存在机构码：" + data.OrgCode;
                                    }
                                    else
                                    {
                                        data.IsDeleted = false;
                                        data.CreateTime = DateTime.Now;
                                       string _orgcode=_db.EBasOrg.Where(g => g.ParentCode == data.ParentCode).Max(k => k.OrgCode);
                                       #region 存在最大子集机构码
                                       if (!string.IsNullOrEmpty(_orgcode))
                                       {
                                           _orgcode = _orgcode.Replace(data.ParentCode, "");
                                           int _n;
                                           if (int.TryParse(_orgcode, out _n))
                                           {
                                               if (_n < 999)
                                               {
                                                   data.OrgCode = data.ParentCode + (++_n + "").PadLeft(3, '0');
                                                   data.HierarchyCode = _parentorg.HierarchyCode + @"|" + data.OrgCode;
                                                   data.HierarchyName = _parentorg.HierarchyName + @"|" + data.OrgName;
                                                   data.CreateByCode = "111111";
                                                   data.CreateByName = "系统管理员";
                                                   _db.EBasOrg.Add(data);
                                                   _ret.Key = "success";
                                               }
                                               else
                                               {
                                                   _ret.Key = "error";
                                                   _ret.Value = "子集机构不能超过999";
                                               }
                                           }
                                           else
                                           {
                                               _ret.Key = "error";
                                               _ret.Value = "机构代码异常";
                                           }
                                       }
                                       #endregion
                                       #region 不存在
                                       else
                                       {
                                           data.OrgCode = data.ParentCode + "001";
                                           data.HierarchyCode = _parentorg.HierarchyCode + @"|" + data.OrgCode;
                                           data.HierarchyName = _parentorg.HierarchyName + @"|" + data.OrgName;
                                           data.CreateByCode = "111111";
                                           data.CreateByName = "系统管理员";
                                           _db.EBasOrg.Add(data);
                                           _ret.Key = "success";
                                       }
                                       #endregion
                                    }
                                    break;
                                #endregion
                                #region 修改
                                case "modify":
                                        var _org = _db.EBasOrg.FirstOrDefault(o => o.OrgCode == data.OrgCode);
                                        if (_org != null)
                                        {
                                            _org.ModifyTime = DateTime.Now;
                                            _org.ModifyByCode = "111111";
                                            _org.ModifyByName = "系统管理员";
                                            _org.HierarchyCode = _parentorg.HierarchyCode + @"|" + data.OrgCode;
                                            _org.HierarchyName = _parentorg.HierarchyName + @"|" + data.OrgName;
                                            _org.SortCode = data.SortCode;
                                            _org.OrgCode = data.OrgCode;
                                            _org.OrgName = data.OrgName;
                                            _ret.Key = "success";
                                        }
                                    break;
                                #endregion
                                #region 删除/伪删除
                                case "delete":
                                        var Delorg = _db.EBasOrg.FirstOrDefault(o => o.OrgCode == data.OrgCode);
                                        if (Delorg != null)
                                        {
                                            Delorg.ModifyTime = DateTime.Now;
                                            Delorg.ModifyByCode = "111111";
                                            Delorg.ModifyByName = "系统管理员";
                                            Delorg.IsDeleted = true;
                                            _ret.Key = "success";
                                        }
                                    break;
                                #endregion
                            }
                            _db.SaveChanges();
                        }                            
                                
                    }
                }
            }
            catch (Exception ex)
            {
                _ret.Key = "error";
                _ret.Value = ex.Message;
            }
            return Json(_ret);
        }
        #endregion
        #endregion
      
    }
}
