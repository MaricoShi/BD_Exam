namespace Exam.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EScoreFile", "HasChildOrgPerson", c => c.Boolean(nullable: false));
            AddColumn("dbo.EScoreProjectInput", "SortIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EScoreProjectInput", "SortIndex");
            DropColumn("dbo.EScoreFile", "HasChildOrgPerson");
        }
    }
}
