using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 用户表
	/// </summary>
    [Table("sys_user")]
    public partial class SysUser : SysEntity
    {
        public SysUser()
        {
        }

        /// <summary>
	    /// 账号
	    /// </summary>
        [Column("account")]
        public string Account { get; set; }
        /// <summary>
	    /// 密码
	    /// </summary>
        [Column("password")]
        public string Password { get; set; }
        /// <summary>
	    /// 用户名
	    /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
	    /// 性别
	    /// </summary>
        [Column("sex")]
        public int? Sex { get; set; }
        /// <summary>
	    /// 状态
	    /// </summary>
        [Column("status")]
        public int? Status { get; set; }
    }
}