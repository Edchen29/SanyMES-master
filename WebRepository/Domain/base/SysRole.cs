using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 角色表
	/// </summary>
    [Table("sys_role")]
    public partial class SysRole : SysEntity
    {
        public SysRole()
        {
        }

        /// <summary>
	    /// 角色名
	    /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
	    /// 状态
	    /// </summary>
        [Column("status")]
        public int? Status { get; set; }
    }
}