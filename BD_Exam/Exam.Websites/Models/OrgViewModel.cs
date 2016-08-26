using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam.Websites.Models
{
    public class OrgViewModel
    {
        /// <summary>
        /// 机构代码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 是否拥有子集
        /// </summary>
        public bool HasChildren { get; set; }
    }
}