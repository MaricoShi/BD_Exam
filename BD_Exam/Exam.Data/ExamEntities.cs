
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

using Exam.Data.Models;

namespace Exam.Data
{
    public class ExamEntities : DbContext
    {
        static ExamEntities()
        {
            //System.Data.Entity.Database.SetInitializer(new SampleData());
            //Database.SetInitializer<ExamEntities>(null);
        }
        public ExamEntities()
            : base("name=ExamEntities")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<EAbsentPersonInfo> EAbsentPersonInfo { get; set; }
        public virtual DbSet<EBasOrg> EBasOrg { get; set; }
        public virtual DbSet<EBasPersonInfo> EBasPersonInfo { get; set; }
        public virtual DbSet<EBasProfessionInfo> EBasProfessionInfo { get; set; }
        public virtual DbSet<EConfigProfessionProject> EConfigProfessionProject { get; set; }
        public virtual DbSet<EScoreFile> EScoreFile { get; set; }
        public virtual DbSet<EScoreFileDetail> EScoreFileDetail { get; set; }
        public virtual DbSet<EScoreProjectInput> EScoreProjectInput { get; set; }
        public virtual DbSet<ESysData> ESysData { get; set; }
        public virtual DbSet<ESysDataType> ESysDataType { get; set; }

        public virtual DbSet<ESys> ESys { get; set; }

    }
}
