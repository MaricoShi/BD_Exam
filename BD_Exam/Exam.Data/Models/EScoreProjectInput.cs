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

    [Table("EScoreProjectInput")]
    public partial class EScoreProjectInput
    {
        [Key]
        public string ProjectInputId { get; set; }
        public string ScoreFileDetailId { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public decimal TakeRate { get; set; }
        public decimal ExamScore { get; set; }
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
    
        public virtual EScoreFileDetail EScoreFileDetail { get; set; }
    }
}
