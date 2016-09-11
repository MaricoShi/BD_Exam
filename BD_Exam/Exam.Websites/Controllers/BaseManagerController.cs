using Exam.Websites.Help.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exam.Data;
using Exam.Data.Models;
using Exam.Websites.Models;
using System.Data.Entity;

namespace Exam.Websites.Controllers
{
    public class BaseManagerController : BaseController
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

        /// <summary>
        /// 专业配置
        /// </summary>
        /// <returns></returns>
        [IsAjaxRedirectFilter(_RedirectUrl = "~/BaseManager/Index")]
        public ActionResult ProfessionInfo() 
        {
            return View();
        }

        /// <summary>
        /// 专业项目配置保存
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProfessionInfoSave(EBasProfessionInfo info) 
        {
            try
            {
                if (info == null)
                {
                    _RetBack.retMsg = "保存的数据不能为空！！！";
                    return Json(_RetBack);
                }
                if (string.IsNullOrWhiteSpace(info.ProfessionName))
                {
                    _RetBack.retMsg = "专业项目配置中专业名称不能为空值，操作结束！！！";
                    return Json(_RetBack);
                }

                var professionInfo = CurrentContext.EBasProfessionInfo.Include(p => p.EConfigProfessionProjects)
                    .FirstOrDefault(p => p.ProfessionCode == info.ProfessionCode);

                #region 专业的新增修改
                if (professionInfo == null)
                {
                    //专业代码
                    string professionCode = "ZY" + DateTime.Now.ToString("yy");
                    int code = 0;
                    string pcode = CurrentContext.EBasProfessionInfo.Where(p =>
                        p.ProfessionCode.StartsWith(professionCode)).Max(p => p.ProfessionCode);

                    if (pcode == null || !int.TryParse(pcode.Replace(professionCode, ""), out code))
                        code = 0;
                    code++;

                    //新增
                    professionInfo = new EBasProfessionInfo();
                    professionInfo.ProfessionCode = professionCode + code.ToString().PadLeft(4, '0');
                    professionInfo.ProfessionStatus = "00";
                    professionInfo.CreateByCode = CurrentUserInfo.UserID;
                    professionInfo.CreateByName = CurrentUserInfo.UserName;
                    professionInfo.CreateTime = DateTime.Now;
                    professionInfo.CreateBy = GetIP();
                    professionInfo.IsDeleted = false;

                    CurrentContext.EBasProfessionInfo.Add(professionInfo);
                }
                professionInfo.ProfessionName = info.ProfessionName.Trim();
                professionInfo.ModifyByCode = CurrentUserInfo.UserID;
                professionInfo.ModifyByName = CurrentUserInfo.UserName;
                professionInfo.ModifyTime = DateTime.Now;
                professionInfo.ModifyBy = GetIP();

                info.ProfessionCode = professionInfo.ProfessionCode;
                #endregion
                if (info.EConfigProfessionProjects != null)
                {
                    #region 项目配置操作
                    //更新项目配置，比例之和需为100
                    decimal sumTakeRate = info.EConfigProfessionProjects.Sum(p => p.TakeRate);
                    if (sumTakeRate != 100)
                    {
                        _RetBack.retMsg = "该专业的所有项目“成绩占专业比例”之和不为100%，操作结束！！！";
                        return Json(_RetBack);
                    }
                    if (info.EConfigProfessionProjects.Any(p => string.IsNullOrWhiteSpace(p.ProjectName)))
                    {
                        _RetBack.retMsg = "专业项目配置中项目名称不能为空值，操作结束！！！";
                        return Json(_RetBack);
                    }
                    professionInfo.EConfigProfessionProjects = professionInfo.EConfigProfessionProjects
                        ?? new List<EConfigProfessionProject>();
                    //对应专业所有项目置为删除状态
                    foreach (var item in professionInfo.EConfigProfessionProjects
                        .Where(p => p.IsDeleted != true))
                    {
                        item.IsDeleted = true;
                    }

                    //项目代码获取
                    string projectCode = "XM" + DateTime.Now.ToString("yy");
                    int code = 0;
                    string pcode = professionInfo.EConfigProfessionProjects.Where(p =>
                        p.ProjectCode.StartsWith(projectCode)).Max(p => p.ProjectCode);

                    if (pcode == null || !int.TryParse(pcode.Replace(projectCode, ""), out code))
                        code = 0;

                    int sortIndex = 0;
                    foreach (var item in info.EConfigProfessionProjects)
                    {
                        sortIndex++;

                        var projects = professionInfo.EConfigProfessionProjects
                            .FirstOrDefault(p => p.ConfigId == item.ConfigId);
                        #region 项目配置的新增修改
                        if (projects == null)
                        {
                            code++;

                            projects = new EConfigProfessionProject();
                            projects.ConfigId = Guid.NewGuid().ToString("N");
                            projects.ProjectCode = projectCode + code.ToString().PadLeft(4, '0');

                            projects.CreateByCode = CurrentUserInfo.UserID;
                            projects.CreateByName = CurrentUserInfo.UserName;
                            projects.CreateTime = DateTime.Now;
                            projects.CreateBy = GetIP();
                            CurrentContext.EConfigProfessionProject.Add(projects);
                        }
                        projects.SortIndex = sortIndex;
                        projects.ProfessionCode = professionInfo.ProfessionCode;
                        projects.ProjectName = item.ProjectName.Trim();
                        projects.TakeRate = item.TakeRate;
                        projects.ModifyByCode = CurrentUserInfo.UserID;
                        projects.ModifyByName = CurrentUserInfo.UserName;
                        projects.ModifyTime = DateTime.Now;
                        projects.ModifyBy = GetIP();
                        projects.IsDeleted = false;
                        #endregion
                    }
                    #endregion
                }

                CurrentContext.SaveChanges();
                _RetBack.retCode = "000000";
                _RetBack.retMsg = "保存成功";
                _RetBack.retData = info;
            }
            catch (Exception ex)
            {
                _RetBack.retMsg = ex.Message;
            }
            

            return Json(_RetBack);
        }

        /// <summary>
        /// 专业项目配置信息移除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProfessionInfoDel(string id,string pro) 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id) || (pro != "p" && pro != "j"))
                {
                    _RetBack.retMsg = "未知的请求";
                    return Json(_RetBack);
                }
                switch (pro)
                {
                    case "p":  //专业删除
                        var profession = CurrentContext.EBasProfessionInfo
                            .Include(p => p.EConfigProfessionProjects)
                            .FirstOrDefault(p => p.ProfessionCode == id);
                        if (profession == null)
                        {
                            _RetBack.retMsg = "不存在该专业信息！！！";
                            return Json(_RetBack);
                        }
                        if (CurrentContext.EBasPersonInfo.Any(p => p.IsDeleted == false
                            && p.ProfessionCode == profession.ProfessionCode)) {
                                _RetBack.retMsg = "该专业下还有有效人员，无法进行删除！！！";
                                return Json(_RetBack);
                        }
                        foreach (var item in profession.EConfigProfessionProjects.Where(p=>p.IsDeleted != true ))
                        {
                            item.IsDeleted = true;
                        }
                        profession.IsDeleted = true;
                        break;
                    case "j":  //项目配置删除
                        var project = CurrentContext.EConfigProfessionProject
                            .FirstOrDefault(p=>p.ConfigId==id);
                        if (project == null) 
                        {
                            _RetBack.retMsg = "不存在该项目配置信息！！！";
                            return Json(_RetBack);
                        }
                        project.IsDeleted = true;
                        break;
                    default:
                        break;
                }
                CurrentContext.SaveChanges();
                _RetBack.retCode = "000000";
                _RetBack.retMsg = "删除成功";
            }
            catch (Exception ex)
            {
                _RetBack.retMsg = ex.Message;
            }
            return Json(_RetBack);
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
                    CurrentContext.EBasOrg.Where(o => o.IsDeleted == false && string.IsNullOrEmpty(o.ParentCode)).OrderBy(s => s.SortCode)
                    .Select(p=>new OrgViewModel {
                    OrgCode=p.OrgCode,
                    OrgName=p.OrgName,
                    SortCode = p.SortCode,
                    ShortName = p.ShortName,
                    HasChildren = CurrentContext.EBasOrg.Any(o => o.ParentCode == p.OrgCode && o.IsDeleted == false)
                    }).ToList()
                    :
                    CurrentContext.EBasOrg.Where(o => o.IsDeleted == false && o.ParentCode == ParentCode).OrderBy(s => s.SortCode)
                    .Select(p => new OrgViewModel
                    {
                        OrgCode = p.OrgCode,
                        OrgName = p.OrgName,
                        SortCode=p.SortCode,
                        ShortName=p.ShortName,
                        HasChildren = CurrentContext.EBasOrg.Any(o => o.ParentCode == p.OrgCode && o.IsDeleted == false)
                    })
                    .ToList();


            }
            catch (Exception)
            {

            }
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改机构数据
        [HttpPost]
        public ActionResult OrgOperate(string type, EBasOrg data)
        {
            KeyValue _ret = new KeyValue() { Key = "error",Value="未做修改"};
            try
            {
                if (!string.IsNullOrEmpty(type))
                {

                    using (ExamEntities _db = CurrentContext)
                    {

                        var _parentorg = _db.EBasOrg.FirstOrDefault(org => org.OrgCode == data.ParentCode);
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
                                        data.NextOrgCode = 0;
                                        var _sublings= CurrentContext.EBasOrg.Where(p => p.ParentCode == _parentorg.OrgCode);
                                        data.SortCode = _sublings.Any() ? _sublings.Max(s=>s.SortCode)+1 : 0;
                                        if (_parentorg.NextOrgCode < 999)
                                        {
                                            data.OrgCode = data.ParentCode + (++_parentorg.NextOrgCode + "").PadLeft(3, '0');
                                            data.HierarchyCode = _parentorg.HierarchyCode + @"|" + data.OrgCode;
                                            data.HierarchyName = _parentorg.HierarchyName + @"|" + data.OrgName;
                                            data.CreateByCode = "111111";
                                            data.CreateByName = "系统管理员";
                                            _db.EBasOrg.Add(data);
                                            _ret.Key = "success";
                                            _ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(new { OrgCode = data.OrgCode, OrgName = data.OrgName, ShortName = data.ShortName, SortCode =data.SortCode});
                                        }
                                        else
                                        {
                                            _ret.Key = "error";
                                            _ret.Value = "子集机构不能超过999";
                                        }
                                    }
                                    break;
                                #endregion
                                #region 修改
                                case "modify":
                                    var _org = _db.EBasOrg.FirstOrDefault(o => o.OrgCode == data.OrgCode&&o.IsDeleted==false);
                                    var _ptorg = _db.EBasOrg.FirstOrDefault(o => o.OrgCode == _org.ParentCode && o.IsDeleted == false);
                                    if (_org != null)
                                    {
                                        _org.ModifyTime = DateTime.Now;
                                        _org.ModifyByCode = "111111";
                                        _org.ModifyByName = "系统管理员";
                                        _org.ShortName = data.ShortName;
                                        string _HierarchyName = _ptorg.HierarchyName + @"|" + data.OrgName;
                                        if(_org.HierarchyName !=_HierarchyName)
                                        {
                                            var editList = CurrentContext.EBasOrg.Where(org => org.HierarchyCode.StartsWith(_org.HierarchyCode) && org.OrgCode != _org.OrgCode);
                                           foreach (var item in editList)
                                           {
                                               item.HierarchyName = item.HierarchyName.Replace(_org.HierarchyName, _HierarchyName);
                                           }
                                           _org.HierarchyName = _HierarchyName;
                                        }
                                        //_org.SortCode = data.SortCode;
                                        _org.OrgName = data.OrgName;
                                        _ret.Key = "success";
                                        _ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(new { OrgCode = data.OrgCode, OrgName = data.OrgName, ShortName = data.ShortName, SortCode = data.SortCode });
                                    }
                                    else
                                    {
                                        _ret.Key = "error";
                                        _ret.Value = "未能找到此机构";
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

        #region 机构上移下移
        [HttpGet]
        public ActionResult SortNode(string orgcode,string tp)
        {
            KeyValue ret = new KeyValue {
            Key="error",
            Value="未做操作"
            };
            try
            {
                EBasOrg Org = CurrentContext.EBasOrg.FirstOrDefault(o => o.OrgCode == orgcode);
                if (Org != null)
                {
                    var list = CurrentContext.EBasOrg
                          .Where(g => g.ParentCode == Org.ParentCode && g.IsDeleted == false)
                          .OrderBy(s => s.SortCode)
                          .ThenByDescending(k => k.CreateTime);

                    int i = 0;
                    foreach (var item in list)
                    {
                        item.SortCode = i++;
                    }

                    int selfSort = Org.SortCode;
                    
                    switch (tp)
                    {
                        #region 上移
                        case "up":
                            EBasOrg beforeOrg = list.FirstOrDefault(p => p.SortCode == selfSort - 1);
                            if (beforeOrg != null)
                            {
                                beforeOrg.SortCode = Org.SortCode--;
                                ret.Key = "success";
                                ret.Value = "操作成功";
                                CurrentContext.SaveChanges();
                            }
                            else
                            {
                                ret.Key = "error";
                                ret.Value = "操作失败";
                            }
                            break;
                        #endregion
                        #region 下移
                        case "down":
                            EBasOrg afterOrg = list.FirstOrDefault(p => p.SortCode == selfSort + 1);
                            if (afterOrg != null)
                            {
                                afterOrg.SortCode = Org.SortCode++;
                                ret.Key = "success";
                                ret.Value = "操作成功";
                                CurrentContext.SaveChanges();
                            }
                            else
                            {
                                ret.Key = "error";
                                ret.Value = "操作失败";
                            }
                            break;
                        #endregion
                    }                    
                }
            }
            catch (Exception ex)
            {
                ret.Key = "error";
                ret.Value = ex.Message;
            }
            return Json(ret,JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        public ActionResult PersonInfoAdd(EBasPersonInfo info) 
        {
            EBasPersonInfo _PersonInfo = CurrentContext.EBasPersonInfo
                .FirstOrDefault(p => p.PersonId == info.PersonId);
            if (_PersonInfo == null) {
                _PersonInfo = new EBasPersonInfo();
                _PersonInfo.PersonId = Guid.NewGuid().ToString("N");
                _PersonInfo.OrgCode = info.OrgCode;
            }
            return View(_PersonInfo);
        }

    }
}
