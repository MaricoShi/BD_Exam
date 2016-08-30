namespace Exam.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EConfigProfessionProject", "SortIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EConfigProfessionProject", "SortIndex");
        }
    }
}
