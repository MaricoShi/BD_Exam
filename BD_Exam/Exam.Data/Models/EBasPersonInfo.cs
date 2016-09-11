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

    [Table("EBasPersonInfo")]
    public partial class EBasPersonInfo
    {
        [Key]
        public string PersonId { get; set; }
        public string PersonCode { get; set; }
        public string PersonName { get; set; }
        public string Sex { get; set; }
        public byte[] PersonPhoto { get; set; }
        public string PersonStatus { get; set; }
        public string PersonPost { get; set; }
        public string PersonRank { get; set; }
        public string ProfessionCode { get; set; }
        public string ProfessionName { get; set; }
        public string Nation { get; set; }
        public string Recruitment { get; set; }

        [ForeignKey("EBasOrg")]
        public string OrgCode { get; set; }
        public string Education { get; set; }
        public Nullable<System.DateTime> RecruitTime { get; set; }
        public Nullable<System.DateTime> BeProfessionTime { get; set; }
        public Nullable<bool> IsEnded { get; set; }
        public Nullable<System.DateTime> ProfileTime { get; set; }
        public Nullable<System.DateTime> TuidangTime { get; set; }
        public string PoliticalStatus { get; set; }
        public Nullable<System.DateTime> CaucusTime { get; set; }
        public string TrainingOutline { get; set; }
        public string TrainingLevel { get; set; }
        public string DeclarationLevel { get; set; }
        public string MasterWeapon { get; set; }
        public string WorkResume { get; set; }
        public string JoinTrainCamp { get; set; }
        public string ImportantExercise { get; set; }
        public string JoinFighting { get; set; }
        public string TrainRewardsPenalties { get; set; }
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
        public virtual EBasProfessionInfo EBasProfessionInfo { get; set; }
    }
}
