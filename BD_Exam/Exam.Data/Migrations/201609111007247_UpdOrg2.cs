namespace Exam.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdOrg2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EBasPersonInfo", "OrgCode", "dbo.EBasOrg");
            DropForeignKey("dbo.EBasPersonInfo", "ProfessionCode", "dbo.EBasProfessionInfo");
            DropIndex("dbo.EBasPersonInfo", new[] { "ProfessionCode" });
            DropIndex("dbo.EBasPersonInfo", new[] { "OrgCode" });
            AlterColumn("dbo.EBasPersonInfo", "ProfessionCode", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.EBasPersonInfo", "OrgCode", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.EBasPersonInfo", "ProfessionCode");
            CreateIndex("dbo.EBasPersonInfo", "OrgCode");
            AddForeignKey("dbo.EBasPersonInfo", "OrgCode", "dbo.EBasOrg", "OrgCode", cascadeDelete: true);
            AddForeignKey("dbo.EBasPersonInfo", "ProfessionCode", "dbo.EBasProfessionInfo", "ProfessionCode", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EBasPersonInfo", "ProfessionCode", "dbo.EBasProfessionInfo");
            DropForeignKey("dbo.EBasPersonInfo", "OrgCode", "dbo.EBasOrg");
            DropIndex("dbo.EBasPersonInfo", new[] { "OrgCode" });
            DropIndex("dbo.EBasPersonInfo", new[] { "ProfessionCode" });
            AlterColumn("dbo.EBasPersonInfo", "OrgCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.EBasPersonInfo", "ProfessionCode", c => c.String(maxLength: 128));
            CreateIndex("dbo.EBasPersonInfo", "OrgCode");
            CreateIndex("dbo.EBasPersonInfo", "ProfessionCode");
            AddForeignKey("dbo.EBasPersonInfo", "ProfessionCode", "dbo.EBasProfessionInfo", "ProfessionCode");
            AddForeignKey("dbo.EBasPersonInfo", "OrgCode", "dbo.EBasOrg", "OrgCode");
        }
    }
}
