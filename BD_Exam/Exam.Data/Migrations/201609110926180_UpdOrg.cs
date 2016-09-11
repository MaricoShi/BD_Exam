namespace Exam.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdOrg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EBasPersonInfo", "OrgName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EBasPersonInfo", "OrgName");
        }
    }
}
