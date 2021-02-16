using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 任务号
	/// </summary>
    [Table("sys_count")]
    public partial class SysCount
    {
        public SysCount()
        {
        }

        /// <summary>
	    /// 任务类型
	    /// </summary>
        [Column("Type")]
        public string Type { get; set; }
        /// <summary>
	    /// 值
	    /// </summary>
        [Column("Value")]
        public string Value { get; set; }
    }
}