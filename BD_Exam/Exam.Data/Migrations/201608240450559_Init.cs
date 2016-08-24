namespace Exam.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EAbsentPersonInfo",
                c => new
                    {
                        AbsentId = c.String(nullable: false, maxLength: 128),
                        ScoreFileDetailId = c.String(maxLength: 128),
                        PersonId = c.String(),
                        PersonName = c.String(),
                        AbsentReason = c.String(),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.AbsentId)
                .ForeignKey("dbo.EScoreFileDetail", t => t.ScoreFileDetailId)
                .Index(t => t.ScoreFileDetailId);
            
            CreateTable(
                "dbo.EScoreFileDetail",
                c => new
                    {
                        ScoreFileDetailId = c.String(nullable: false, maxLength: 128),
                        ScoreFileId = c.String(maxLength: 128),
                        ProfessionCode = c.String(),
                        ProfessionName = c.String(),
                        ProfessionAvgScore = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProjectCount = c.Int(nullable: false),
                        TakeTestPersonNum = c.Int(nullable: false),
                        AbsentPersonNum = c.Int(nullable: false),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.ScoreFileDetailId)
                .ForeignKey("dbo.EScoreFile", t => t.ScoreFileId)
                .Index(t => t.ScoreFileId);
            
            CreateTable(
                "dbo.EScoreFile",
                c => new
                    {
                        ScoreFileId = c.String(nullable: false, maxLength: 128),
                        OrgCode = c.String(maxLength: 128),
                        OrgName = c.String(),
                        ExamTime = c.DateTime(nullable: false),
                        CompanyAvgScore = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProfessionCount = c.Int(nullable: false),
                        TestPersonNum = c.Int(nullable: false),
                        TakeTestPersonNum = c.Int(nullable: false),
                        TakeTestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.ScoreFileId)
                .ForeignKey("dbo.EBasOrg", t => t.OrgCode)
                .Index(t => t.OrgCode);
            
            CreateTable(
                "dbo.EBasOrg",
                c => new
                    {
                        OrgCode = c.String(nullable: false, maxLength: 128),
                        OrgName = c.String(),
                        ShortName = c.String(),
                        SortCode = c.Int(nullable: false),
                        StatusC = c.String(),
                        ParentCode = c.String(),
                        NextOrgNum = c.Int(nullable: false),
                        NextOrgCode = c.Int(nullable: false),
                        HierarchyCode = c.String(),
                        HierarchyName = c.String(),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.OrgCode);
            
            CreateTable(
                "dbo.EBasPersonInfo",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 128),
                        PersonCode = c.String(),
                        PersonName = c.String(),
                        Sex = c.String(),
                        PersonPhoto = c.Binary(),
                        PersonStatus = c.String(),
                        PersonPost = c.String(),
                        PersonRank = c.String(),
                        ProfessionCode = c.String(maxLength: 128),
                        ProfessionName = c.String(),
                        Nation = c.String(),
                        Recruitment = c.String(),
                        OrgCode = c.String(maxLength: 128),
                        Education = c.String(),
                        RecruitTime = c.DateTime(),
                        BeProfessionTime = c.DateTime(),
                        IsEnded = c.Boolean(),
                        ProfileTime = c.DateTime(),
                        TuidangTime = c.DateTime(),
                        PoliticalStatus = c.String(),
                        CaucusTime = c.DateTime(),
                        TrainingOutline = c.String(),
                        TrainingLevel = c.String(),
                        DeclarationLevel = c.String(),
                        MasterWeapon = c.String(),
                        WorkResume = c.String(),
                        JoinTrainCamp = c.String(),
                        ImportantExercise = c.String(),
                        JoinFighting = c.String(),
                        TrainRewardsPenalties = c.String(),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.EBasOrg", t => t.OrgCode)
                .ForeignKey("dbo.EBasProfessionInfo", t => t.ProfessionCode)
                .Index(t => t.ProfessionCode)
                .Index(t => t.OrgCode);
            
            CreateTable(
                "dbo.EBasProfessionInfo",
                c => new
                    {
                        ProfessionCode = c.String(nullable: false, maxLength: 128),
                        ProfessionName = c.String(),
                        ProfessionStatus = c.String(),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.ProfessionCode);
            
            CreateTable(
                "dbo.EConfigProfessionProject",
                c => new
                    {
                        ConfigId = c.String(nullable: false, maxLength: 128),
                        ProfessionCode = c.String(maxLength: 128),
                        ProjectCode = c.String(),
                        ProjectName = c.String(),
                        TakeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.ConfigId)
                .ForeignKey("dbo.EBasProfessionInfo", t => t.ProfessionCode)
                .Index(t => t.ProfessionCode);
            
            CreateTable(
                "dbo.EScoreProjectInput",
                c => new
                    {
                        ProjectInputId = c.String(nullable: false, maxLength: 128),
                        ScoreFileDetailId = c.String(maxLength: 128),
                        PersonId = c.String(),
                        PersonName = c.String(),
                        ProjectCode = c.String(),
                        ProjectName = c.String(),
                        TakeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExamScore = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.ProjectInputId)
                .ForeignKey("dbo.EScoreFileDetail", t => t.ScoreFileDetailId)
                .Index(t => t.ScoreFileDetailId);
            
            CreateTable(
                "dbo.ESys",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        VersionCode = c.String(),
                        VersionName = c.String(),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ESysData",
                c => new
                    {
                        BusintypeId = c.String(nullable: false, maxLength: 128),
                        BusintypeCode = c.String(maxLength: 128),
                        BusinCode = c.String(),
                        BusinName = c.String(),
                        BusinLevel = c.String(),
                        UpBusinCode = c.String(),
                        BusinStatus = c.String(),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.BusintypeId)
                .ForeignKey("dbo.ESysDataType", t => t.BusintypeCode)
                .Index(t => t.BusintypeCode);
            
            CreateTable(
                "dbo.ESysDataType",
                c => new
                    {
                        BusintypeCode = c.String(nullable: false, maxLength: 128),
                        BusintypeName = c.String(),
                        Remark = c.String(),
                        CreateByCode = c.String(),
                        CreateByName = c.String(),
                        CreateBy = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(),
                        ModifyByName = c.String(),
                        ModifyBy = c.String(),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.BusintypeCode);
            
            CreateTable(
                "dbo.ESysUser",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 32),
                        UserCode = c.String(nullable: false, maxLength: 50),
                        UserPwd = c.String(nullable: false, maxLength: 100),
                        UserName = c.String(maxLength: 100),
                        UserStatus = c.String(nullable: false, maxLength: 2),
                        UserLevel = c.Int(nullable: false),
                        Remark = c.String(maxLength: 500),
                        CreateByCode = c.String(maxLength: 32),
                        CreateByName = c.String(maxLength: 100),
                        CreateBy = c.String(maxLength: 32),
                        CreateTime = c.DateTime(),
                        ModifyByCode = c.String(maxLength: 32),
                        ModifyByName = c.String(maxLength: 100),
                        ModifyBy = c.String(maxLength: 32),
                        ModifyTime = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ESysData", "BusintypeCode", "dbo.ESysDataType");
            DropForeignKey("dbo.EScoreProjectInput", "ScoreFileDetailId", "dbo.EScoreFileDetail");
            DropForeignKey("dbo.EScoreFileDetail", "ScoreFileId", "dbo.EScoreFile");
            DropForeignKey("dbo.EScoreFile", "OrgCode", "dbo.EBasOrg");
            DropForeignKey("dbo.EConfigProfessionProject", "ProfessionCode", "dbo.EBasProfessionInfo");
            DropForeignKey("dbo.EBasPersonInfo", "ProfessionCode", "dbo.EBasProfessionInfo");
            DropForeignKey("dbo.EBasPersonInfo", "OrgCode", "dbo.EBasOrg");
            DropForeignKey("dbo.EAbsentPersonInfo", "ScoreFileDetailId", "dbo.EScoreFileDetail");
            DropIndex("dbo.ESysData", new[] { "BusintypeCode" });
            DropIndex("dbo.EScoreProjectInput", new[] { "ScoreFileDetailId" });
            DropIndex("dbo.EConfigProfessionProject", new[] { "ProfessionCode" });
            DropIndex("dbo.EBasPersonInfo", new[] { "OrgCode" });
            DropIndex("dbo.EBasPersonInfo", new[] { "ProfessionCode" });
            DropIndex("dbo.EScoreFile", new[] { "OrgCode" });
            DropIndex("dbo.EScoreFileDetail", new[] { "ScoreFileId" });
            DropIndex("dbo.EAbsentPersonInfo", new[] { "ScoreFileDetailId" });
            DropTable("dbo.ESysUser");
            DropTable("dbo.ESysDataType");
            DropTable("dbo.ESysData");
            DropTable("dbo.ESys");
            DropTable("dbo.EScoreProjectInput");
            DropTable("dbo.EConfigProfessionProject");
            DropTable("dbo.EBasProfessionInfo");
            DropTable("dbo.EBasPersonInfo");
            DropTable("dbo.EBasOrg");
            DropTable("dbo.EScoreFile");
            DropTable("dbo.EScoreFileDetail");
            DropTable("dbo.EAbsentPersonInfo");
        }
    }
}
