//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exam.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EScoreFile")]
    public partial class EScoreFile
    {
        public EScoreFile()
        {
            this.EScoreFileDetails = new HashSet<EScoreFileDetail>();
        }
    
        [Key]
        public string ScoreFileId { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public System.DateTime ExamTime { get; set; }
        public decimal CompanyAvgScore { get; set; }
        public int ProfessionCount { get; set; }
        public int TestPersonNum { get; set; }
        public int TakeTestPersonNum { get; set; }
        public decimal TakeTestRate { get; set; }
        public bool HasChildOrgPerson { get; set; }
        public string Remark { get; set; }
        public string CreateByCode { get; set; }
        public string CreateByName { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string ModifyByCode { get; set; }
        public string ModifyByName { get; set; }
        public string ModifyBy { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual EBasOrg EBasOrg { get; set; }
        public virtual ICollection<EScoreFileDetail> EScoreFileDetails { get; set; }
    }
}
