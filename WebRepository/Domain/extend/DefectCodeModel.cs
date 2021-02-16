using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 不良代码表
	/// </summary>
    [Table("defect_code")]
    public partial class DefectCodeModel
    {

        /// <summary>
	    /// 不良代号
	    /// </summary>
        [Column("code")]
        public string Code { get; set; }
        /// <summary>
	    /// 不良名称
	    /// </summary>
        [Column("name")]
        public string Name { get; set; }
    }
}