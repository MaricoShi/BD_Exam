using Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;

namespace Exam.Websites.Controllers
{
    public class ExamController : BaseController
    {
        //
        // GET: /Exam/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EScoreFileAdd(EScoreFile _ScoreFile) 
        {
            if (_ScoreFile == null || string.IsNullOrWhiteSpace(_ScoreFile.OrgCode))
            { 
            
            }

            EScoreFile _EScoreFileInfo = CurrentContext.EScoreFile.Include(p => p.EScoreFileDetails)
                .FirstOrDefault(p => p.ScoreFileId == _ScoreFile.ScoreFileId);
            if (_EScoreFileInfo == null)
            {
                //应考人员信息
                var _ExamPersonE = CurrentContext.EBasPersonInfo.Where(p => p.IsDeleted == false
                    && p.PersonStatus == "00" && p.OrgCode != null && p.OrgCode.StartsWith(_ScoreFile.OrgCode));
                if (_ScoreFile.HasChildOrgPerson == false)
                { //不包含子集单位人员
                    _ExamPersonE = _ExamPersonE.Where(p => p.OrgCode == _ScoreFile.OrgCode);
                }
                var _ExamPerson = _ExamPersonE.ToList();
                //涉及的专业信息
                var _ProfessionInfos = _ExamPerson.Where(p => p.EBasProfessionInfo != null)
                    .GroupBy(p => p.EBasProfessionInfo).ToList();

                #region 成绩档案
                _EScoreFileInfo = new EScoreFile();    //成绩档案表
                _EScoreFileInfo.ScoreFileId = Guid.NewGuid().ToString("N");
                _EScoreFileInfo.OrgCode = _ScoreFile.OrgCode;
                _EScoreFileInfo.OrgName = CurrentContext.EBasOrg
                    .Where(p => p.OrgCode == _EScoreFileInfo.OrgCode)
                    .Select(p => p.OrgName).FirstOrDefault();
                _EScoreFileInfo.ExamTime = _ScoreFile.ExamTime;
                _EScoreFileInfo.CompanyAvgScore = 0;  //连队平均成绩
                _EScoreFileInfo.ProfessionCount = _ProfessionInfos.Count();
                _EScoreFileInfo.TestPersonNum = _ExamPerson.Count;  //应考人数
                _EScoreFileInfo.TakeTestPersonNum = _EScoreFileInfo.TestPersonNum;  //参考人数
                _EScoreFileInfo.TakeTestRate = 100;
                _EScoreFileInfo.IsDeleted = false;

                _EScoreFileInfo.EScoreFileDetails = new List<EScoreFileDetail>();  //成绩档案明细列表

                #region 成绩档案明细
                foreach (var _ProfKey in _ProfessionInfos)
                {
                    var _Prof = _ProfKey.Key;
                    //当前专业对应的有效项目信息
                    var _ProjectInfos = _Prof.EConfigProfessionProjects.Where(p => p.IsDeleted == false )
                        .OrderBy(p => p.SortIndex).ToList();
                    //当前专业对应的人员信息
                    var _ProfPersons = _ExamPerson.Where(p => p.ProfessionCode == _Prof.ProfessionCode).ToList();

                    EScoreFileDetail _Detail = new EScoreFileDetail(); //成绩档案明细
                    _Detail.ScoreFileDetailId = Guid.NewGuid().ToString("N");
                    _Detail.ScoreFileId = _EScoreFileInfo.ScoreFileId;
                    _Detail.ProfessionCode = _Prof.ProfessionCode;
                    _Detail.ProfessionName = _Prof.ProfessionName;
                    _Detail.ProjectCount = _ProjectInfos.Count();
                    _Detail.TakeTestPersonNum = _ProfPersons.Count();
                    _Detail.IsDeleted = false;

                    _EScoreFileInfo.EScoreFileDetails.Add(_Detail);

                    _Detail.EScoreProjectInputs = new List<EScoreProjectInput>();
                    #region 添加专业成绩录入表
                    foreach (var _Per in _ProfPersons)
                    {
                        foreach (var _Proj in _ProjectInfos)
                        {
                            EScoreProjectInput _ProjInput = new EScoreProjectInput();  //专业成绩录入
                            _ProjInput.ProjectInputId = Guid.NewGuid().ToString("N");
                            _ProjInput.ScoreFileDetailId = _Detail.ScoreFileDetailId;
                            _ProjInput.PersonId = _Per.PersonId;
                            _ProjInput.PersonName = _Per.PersonName;
                            _ProjInput.ProjectCode = _Proj.ConfigId;
                            _ProjInput.ProjectName = _Proj.ProjectName;
                            _ProjInput.SortIndex = _Proj.SortIndex;
                            _ProjInput.TakeRate = _Proj.TakeRate;
                            _ProjInput.IsDeleted = false;

                            _Detail.EScoreProjectInputs.Add(_ProjInput);
                        }
                    }
                    #endregion

                    #region 添加专业对应项目
                    foreach (var _proj in _ProjectInfos)
                    {
                        EScoreFileProject _FileProj = new EScoreFileProject();
                        _FileProj.ProjectId = Guid.NewGuid().ToString("N");
                        _FileProj.ScoreFileDetailId = _Detail.ScoreFileDetailId;
                        _FileProj.ConfigId = _proj.ConfigId;
                        _FileProj.ProjectCode = _proj.ProjectCode;
                        _FileProj.ProjectName = _proj.ProjectName;
                        _FileProj.SortIndex = _proj.SortIndex;
                        _FileProj.TakeRate = _proj.TakeRate;
                        _FileProj.IsDeleted = false;
                        _Detail.EScoreFileProjects.Add(_FileProj);
                    }
                    #endregion
                }
                #endregion
                #endregion
            }
            
            return View(_EScoreFileInfo);
        }


        /// <summary>
        /// 成绩登记保存
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EScoreFileSave(EScoreFile info)
        {
            try
            {
                #region 验证
                if (info == null)
                {
                    _RetBack.retMsg = "保存的数据不能为空！！！";
                    return Json(_RetBack);
                }
                if (string.IsNullOrWhiteSpace(info.OrgCode))
                {
                    _RetBack.retMsg = "单位不能为空值，操作结束！！！";
                    return Json(_RetBack);
                }
                if (info.EScoreFileDetails == null) {
                    _RetBack.retMsg = "成绩档案明细不能为空值，操作结束！！！";
                    return Json(_RetBack);
                }
                #endregion

                var _ScoreFile = CurrentContext.EScoreFile
                    .FirstOrDefault(p => p.ScoreFileId == info.ScoreFileId);

                #region 成绩的新增修改
                if (_ScoreFile == null)
                {
                    #region 新增成绩档案
                    _ScoreFile = new EScoreFile();
                    _ScoreFile.ScoreFileId = string.IsNullOrWhiteSpace(info.ScoreFileId) ?
                        Guid.NewGuid().ToString("N") : info.ScoreFileId;
                    _ScoreFile.OrgCode = info.OrgCode;
                    _ScoreFile.OrgName = CurrentContext.EBasOrg.Where(p => p.OrgCode == info.OrgCode)
                        .Select(p => p.OrgName).FirstOrDefault();
                    _ScoreFile.ExamTime = info.ExamTime;
                    _ScoreFile.CreateByCode = CurrentUserInfo.UserID;
                    _ScoreFile.CreateByName = CurrentUserInfo.UserName;
                    _ScoreFile.CreateTime = DateTime.Now;
                    _ScoreFile.CreateBy = GetIP();
                    _ScoreFile.IsDeleted = false;

                    CurrentContext.EScoreFile.Add(_ScoreFile);
                    #endregion
                }
                _ScoreFile.CompanyAvgScore = 0;
                _ScoreFile.ProfessionCount = info.EScoreFileDetails.Count;
                _ScoreFile.TestPersonNum = 0;
                _ScoreFile.TakeTestPersonNum = 0;
                _ScoreFile.TakeTestRate = 0;

                _ScoreFile.ModifyByCode = CurrentUserInfo.UserID;
                _ScoreFile.ModifyByName = CurrentUserInfo.UserName;
                _ScoreFile.ModifyTime = DateTime.Now;
                _ScoreFile.ModifyBy = GetIP();

                //删除档案明细及相关信息
                #region 删除档案明细及相关信息
                if (_ScoreFile.EScoreFileDetails != null) {
                    var _ScoreFileDetails = _ScoreFile.EScoreFileDetails.ToList();
                    foreach (var _Detail in _ScoreFileDetails)
                    {
                        var _ScoreProjectInputs = _Detail.EScoreProjectInputs.ToList();
                        foreach (var _proj in _ScoreProjectInputs)
                        {
                            CurrentContext.EScoreProjectInput.Remove(_proj);
                        }
                        var _AbsentPersonInfoes = _Detail.EAbsentPersonInfoes.ToList();
                        foreach (var _absent in _AbsentPersonInfoes)
                        {
                            CurrentContext.EAbsentPersonInfo.Remove(_absent);
                        }
                        var _EScoreFileProjects = _Detail.EScoreFileProjects.ToList();
                        foreach (var _fileProj in _EScoreFileProjects)
                        {
                            CurrentContext.EScoreFileProject.Remove(_fileProj);
                        }
                        CurrentContext.EScoreFileDetail.Remove(_Detail);
                    }
                }
                #endregion

                //新增档案明细
                #region 新增档案明细
                decimal SumProfAvgScore = 0;
                foreach (var _DetailInfo in info.EScoreFileDetails)
                {
                    #region 档案明细新增信息
                    if (_DetailInfo.EScoreProjectInputs == null)
                    {
                        _RetBack.retMsg = "人员项目成绩不能为空值，操作结束！！！";
                        return Json(_RetBack);
                    }
                    _DetailInfo.EAbsentPersonInfoes = _DetailInfo.EAbsentPersonInfoes
                        ?? new List<EAbsentPersonInfo>();

                    EScoreFileDetail _Detail = new EScoreFileDetail();
                    _Detail.ScoreFileDetailId = Guid.NewGuid().ToString("N");
                    _Detail.ScoreFileId = _ScoreFile.ScoreFileId;
                    _Detail.ProfessionCode = _DetailInfo.ProfessionCode;
                    _Detail.ProfessionName = CurrentContext.EBasProfessionInfo
                        .Where(p => p.ProfessionCode == _DetailInfo.ProfessionCode)
                        .Select(p => p.ProfessionName).FirstOrDefault();
                    _Detail.ProjectCount = _DetailInfo.EScoreFileProjects.Count();
                    _Detail.TakeTestPersonNum = _DetailInfo.EScoreProjectInputs
                        .GroupBy(p => p.PersonId).Count();
                    _Detail.AbsentPersonNum = _DetailInfo.EAbsentPersonInfoes.Count;

                    if (_Detail.TakeTestPersonNum != 0)
                        _Detail.ProfessionAvgScore = _DetailInfo.EScoreProjectInputs
                            .Sum(p => p.ExamScore * (p.TakeRate / 100))
                            / _Detail.TakeTestPersonNum;

                    _Detail.CreateByCode = CurrentUserInfo.UserID;
                    _Detail.CreateByName = CurrentUserInfo.UserName;
                    _Detail.CreateTime = DateTime.Now;
                    _Detail.CreateBy = GetIP();
                    _Detail.ModifyByCode = CurrentUserInfo.UserID;
                    _Detail.ModifyByName = CurrentUserInfo.UserName;
                    _Detail.ModifyTime = DateTime.Now;
                    _Detail.ModifyBy = GetIP();
                    _Detail.IsDeleted = false;

                    CurrentContext.EScoreFileDetail.Add(_Detail);
                    #endregion

                    #region 新增专业成绩录入表
                    foreach (var _ProjInputInfo in _DetailInfo.EScoreProjectInputs)
                    {
                        #region 专业成绩录入
                        var _Project = CurrentContext.EConfigProfessionProject
                            .Where(p => p.ConfigId == _ProjInputInfo.ProjectCode)
                            .Select(p => new { p.ProjectName, p.SortIndex }).FirstOrDefault();

                        EScoreProjectInput _ProjInput = new EScoreProjectInput();
                        _ProjInput.ProjectInputId = Guid.NewGuid().ToString("N");
                        _ProjInput.ScoreFileDetailId = _Detail.ScoreFileDetailId;
                        _ProjInput.PersonId = _ProjInputInfo.PersonId;
                        _ProjInput.PersonName = CurrentContext.EBasPersonInfo
                            .Where(p => p.PersonId == _ProjInputInfo.PersonId)
                            .Select(p => p.PersonName).FirstOrDefault();
                        _ProjInput.ProjectCode = _ProjInputInfo.ProjectCode;
                        _ProjInput.ProjectName = _Project.ProjectName;
                        _ProjInput.SortIndex = _Project.SortIndex;
                        _ProjInput.TakeRate = _ProjInputInfo.TakeRate;
                        _ProjInput.ExamScore = _ProjInputInfo.ExamScore;

                        _ProjInput.CreateByCode = CurrentUserInfo.UserID;
                        _ProjInput.CreateByName = CurrentUserInfo.UserName;
                        _ProjInput.CreateTime = DateTime.Now;
                        _ProjInput.CreateBy = GetIP();
                        _ProjInput.ModifyByCode = CurrentUserInfo.UserID;
                        _ProjInput.ModifyByName = CurrentUserInfo.UserName;
                        _ProjInput.ModifyTime = DateTime.Now;
                        _ProjInput.ModifyBy = GetIP();
                        _ProjInput.IsDeleted = false;

                        CurrentContext.EScoreProjectInput.Add(_ProjInput);
                        #endregion
                    }
                    #endregion

                    #region 新增缺考人员信息表
                    foreach (var _AbsentInfo in _DetailInfo.EAbsentPersonInfoes)
                    {
                        #region 缺考人员录入
                        EAbsentPersonInfo _Absent = new EAbsentPersonInfo();
                        _Absent.AbsentId = Guid.NewGuid().ToString("N");
                        _Absent.ScoreFileDetailId = _Detail.ScoreFileDetailId;
                        _Absent.PersonId = _AbsentInfo.PersonId;
                        _Absent.PersonName = CurrentContext.EBasPersonInfo
                            .Where(p => p.PersonId == _AbsentInfo.PersonId)
                            .Select(p => p.PersonName).FirstOrDefault();
                        _Absent.AbsentReason = _AbsentInfo.AbsentReason;
                        _Absent.Remark = _AbsentInfo.Remark;

                        _Absent.CreateByCode = CurrentUserInfo.UserID;
                        _Absent.CreateByName = CurrentUserInfo.UserName;
                        _Absent.CreateTime = DateTime.Now;
                        _Absent.CreateBy = GetIP();
                        _Absent.ModifyByCode = CurrentUserInfo.UserID;
                        _Absent.ModifyByName = CurrentUserInfo.UserName;
                        _Absent.ModifyTime = DateTime.Now;
                        _Absent.ModifyBy = GetIP();
                        _Absent.IsDeleted = false;

                        CurrentContext.EAbsentPersonInfo.Add(_Absent);
                        #endregion
                    }
                    #endregion

                    #region 新增专业项目历史配置
                    foreach (var _FileProjInfo in _DetailInfo.EScoreFileProjects)
                    {
                        var _Project = CurrentContext.EConfigProfessionProject
                            .Where(p => p.ConfigId == _FileProjInfo.ConfigId)
                            .Select(p => new { p.ProjectName, p.SortIndex }).FirstOrDefault();

                        EScoreFileProject _FileProj = new EScoreFileProject();
                        _FileProj.ProjectId = Guid.NewGuid().ToString("N");
                        _FileProj.ScoreFileDetailId = _Detail.ScoreFileDetailId;
                        _FileProj.ConfigId = _FileProjInfo.ConfigId;
                        _FileProj.ProjectCode = _FileProjInfo.ProjectCode;
                        _FileProj.ProjectName = _Project.ProjectName;
                        _FileProj.SortIndex = _Project.SortIndex;
                        _FileProj.TakeRate = _FileProjInfo.TakeRate;

                        _FileProj.CreateByCode = CurrentUserInfo.UserID;
                        _FileProj.CreateByName = CurrentUserInfo.UserName;
                        _FileProj.CreateTime = DateTime.Now;
                        _FileProj.CreateBy = GetIP();
                        _FileProj.ModifyByCode = CurrentUserInfo.UserID;
                        _FileProj.ModifyByName = CurrentUserInfo.UserName;
                        _FileProj.ModifyTime = DateTime.Now;
                        _FileProj.ModifyBy = GetIP();
                        _FileProj.IsDeleted = false;

                        CurrentContext.EScoreFileProject.Add(_FileProj);
                    }
                    #endregion

                    _ScoreFile.TestPersonNum += _Detail.TakeTestPersonNum + _Detail.AbsentPersonNum;
                    _ScoreFile.TakeTestPersonNum += _Detail.TakeTestPersonNum;
                    SumProfAvgScore += _Detail.ProfessionAvgScore;
                }

                #endregion
                if (_ScoreFile.ProfessionCount != 0)
                    _ScoreFile.CompanyAvgScore = SumProfAvgScore / _ScoreFile.ProfessionCount;
                if (_ScoreFile.TestPersonNum != 0)
                    _ScoreFile.TakeTestRate = ((decimal)_ScoreFile.TakeTestPersonNum 
                        / _ScoreFile.TestPersonNum) * 100;

                #endregion

                CurrentContext.SaveChanges();
                _RetBack.retCode = "000000";
                _RetBack.retMsg = "保存成功";
                //_RetBack.retData = _ScoreFile;
            }
            catch (Exception ex)
            {
                _RetBack.retMsg = ex.Message;
            }


            return Json(_RetBack);
        }
    }
}
