using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 主键关联表
	/// </summary>
    [Table("sys_relevance")]
    public partial class SysRelevance : SysEntity
    {
        public SysRelevance()
        {
        }

        /// <summary>
	    /// 关联键
	    /// </summary>
        [Column("relKey")]
        public string RelKey { get; set; }
        /// <summary>
	    /// 主键
	    /// </summary>
        [Column("firstId")]
        public int? FirstId { get; set; }
        /// <summary>
	    /// 关联键
	    /// </summary>
        [Column("secondId")]
        public int? SecondId { get; set; }
    }
}