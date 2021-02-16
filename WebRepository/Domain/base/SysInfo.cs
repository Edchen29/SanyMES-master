using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 系统信息
	/// </summary>
    [Table("sys_info")]
    public partial class SysInfo : SysEntity
    {
        public SysInfo()
        {
        }

        /// <summary>
	    /// 系统关键字
	    /// </summary>
        [Column("appKey")]
        public string AppKey { get; set; }
        /// <summary>
	    /// 系统密钥
	    /// </summary>
        [Column("appSecret")]
        public string AppSecret { get; set; }
        /// <summary>
	    /// 标题
	    /// </summary>
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
	    /// 备注
	    /// </summary>
        [Column("remark")]
        public string Remark { get; set; }
        /// <summary>
	    /// 图标
	    /// </summary>
        [Column("icon")]
        public string Icon { get; set; }
        /// <summary>
	    /// 主页
	    /// </summary>
        [Column("returnUrl")]
        public string ReturnUrl { get; set; }
        /// <summary>
	    /// 生效
	    /// </summary>
        [Column("isEnable")]
        public bool? IsEnable { get; set; }
    }
}