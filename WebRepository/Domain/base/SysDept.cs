using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 部门表
	/// </summary>
    [Table("sys_dept")]
    public partial class SysDept : TreeEntity
    {
        public SysDept()
        {
        }

        /// <summary>
	    /// 快捷键
	    /// </summary>
        [Column("HotKey")]
        public string HotKey { get; set; }
        /// <summary>
	    /// 分支
	    /// </summary>
        [Column("IsLeaf")]
        public bool? IsLeaf { get; set; }
        /// <summary>
	    /// 自动展开
	    /// </summary>
        [Column("IsAutoExpand")]
        public bool? IsAutoExpand { get; set; }
        /// <summary>
	    /// 图标
	    /// </summary>
        [Column("IconName")]
        public string IconName { get; set; }
        /// <summary>
	    /// 状态
	    /// </summary>
        [Column("Status")]
        public int? Status { get; set; }
        /// <summary>
	    /// 排序号
	    /// </summary>
        [Column("SortNo")]
        public int? SortNo { get; set; }
    }
}