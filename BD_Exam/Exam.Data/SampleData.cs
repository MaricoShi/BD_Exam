using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

using Exam.Data.Models;

namespace Exam.Data
{
    public class SampleData : DropCreateDatabaseIfModelChanges<ExamEntities>
    {
        protected override void Seed(ExamEntities context)
        {
            Exam.Common.Log.WriteLogFile("开始初始化表数据");

            var tbgm = new List<EBasOrg>
            {
                new EBasOrg(){OrgCode="HJLD1LV",OrgName="海军雷达兵第一旅",
                    ShortName="海军雷达兵第一旅",StatusC="00",HierarchyCode="HJLD1LV",
                    HierarchyName="海军雷达兵第一旅",CreateByCode="111111",
                    CreateByName="系统管理员",CreateTime=DateTime.Now,IsDeleted=false}
            };
            tbgm.ForEach(a => context.EBasOrg.Add(a));
            context.SaveChanges();
            Exam.Common.Log.WriteLogFile("机构表初始化完毕");

            var esysuser = new List<ESysUser>
            {
                new ESysUser(){ UserID = Guid.NewGuid().ToString("N"),UserCode = "admin",
                    UserPwd = "szx001",UserName="系统管理员",UserStatus = "00",UserLevel = 1,CreateTime=DateTime.Now}
            };
            esysuser.ForEach(a => context.ESysUser.Add(a));
            context.SaveChanges();
            Exam.Common.Log.WriteLogFile("系统用户初始化完毕");

            var esys = new List<ESys>
            {
                new ESys(){ID="1",VersionCode="1",VersionName="1",IsUsed=true}
            };
            esys.ForEach(a => context.ESys.Add(a));
            context.SaveChanges();
            Exam.Common.Log.WriteLogFile("系统版本初始化完毕");

            Exam.Common.Log.WriteLogFile("数据初始化完毕");

        }
    }
}