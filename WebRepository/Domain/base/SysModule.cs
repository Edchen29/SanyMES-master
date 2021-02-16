using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 模块表
	/// </summary>
    [Table("sys_module")]
    public partial class SysModule : TreeEntity
    {
        public SysModule()
        {
        }

        /// <summary>
	    /// 地址
	    /// </summary>
        [Column("url")]
        public string Url { get; set; }
        /// <summary>
	    /// 图标名
	    /// </summary>
        [Column("iconName")]
        public string IconName { get; set; }
        /// <summary>
	    /// 状态
	    /// </summary>
        [Column("status")]
        public int? Status { get; set; }
        /// <summary>
	    /// 排序号
	    /// </summary>
        [Column("sortNo")]
        public int? SortNo { get; set; }
        /// <summary>
	    /// 代码
	    /// </summary>
        [Column("code")]
        public string Code { get; set; }
        /// <summary>
	    /// 显示
	    /// </summary>
        [Column("isShow")]
        public int? IsShow { get; set; }
    }
}