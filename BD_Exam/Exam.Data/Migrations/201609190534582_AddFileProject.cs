namespace Exam.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EScoreFileProject",
                c => new
                    {
                        ProjectId = c.String(nullable: false, maxLength: 128),
                        ScoreFileDetailId = c.String(maxLength: 128),
                        ConfigId = c.String(),
                        ProjectCode = c.String(),
                        ProjectName = c.String(),
                        SortIndex = c.Int(nullable: false),
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
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.EScoreFileDetail", t => t.ScoreFileDetailId)
                .Index(t => t.ScoreFileDetailId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EScoreFileProject", "ScoreFileDetailId", "dbo.EScoreFileDetail");
            DropIndex("dbo.EScoreFileProject", new[] { "ScoreFileDetailId" });
            DropTable("dbo.EScoreFileProject");
        }
    }
}
