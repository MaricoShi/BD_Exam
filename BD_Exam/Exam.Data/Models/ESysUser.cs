using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Data.Models
{
    [Table("ESysUser")]
    public class ESysUser
    {
        [Key, Display(Name = "用户ID"), MaxLength(32)]
        public string UserID { get; set; }

        [Required,Display(Name = "用户账户"), MaxLength(50)]
        public string UserCode { get; set; }

        [Required,Display(Name = "用户密码"), MaxLength(100)]
        public string UserPwd { get; set; }

        [Display(Name = "用户姓名"), MaxLength(100)]
        public string UserName { get; set; }

        /// <summary>
        /// 00 正常
        /// </summary>
        [Required, Display(Name = "用户状态"), MaxLength(2)]
        public string UserStatus { get; set; }

        [Display(Name = "用户级别")]
        public int UserLevel { get; set; }

        [Display(Name = "备注说明"), MaxLength(500)]
        public string Remark { get; set; }

        [Display(Name = "创建用户ID"), MaxLength(32)]
        public string CreateByCode { get; set; }

        [Display(Name = "创建用户姓名"), MaxLength(100)]
        public string CreateByName { get; set; }

        [Display(Name = "创建来源IP"), MaxLength(32)]
        public string CreateBy { get; set; }

        [Display(Name = "创建时间")]
        public Nullable<System.DateTime> CreateTime { get; set; }

        [Display(Name = "修改用户ID"), MaxLength(32)]
        public string ModifyByCode { get; set; }

        [Display(Name = "修改用户姓名"), MaxLength(100)]
        public string ModifyByName { get; set; }

        [Display(Name = "修改来源IP"), MaxLength(32)]
        public string ModifyBy { get; set; }

        [Display(Name = "修改时间")]
        public Nullable<System.DateTime> ModifyTime { get; set; }

        [Display(Name = "是否删除")]
        public bool IsDeleted { get; set; }
    }
}
