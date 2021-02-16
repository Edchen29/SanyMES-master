using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// EXCEL报表
	/// </summary>
    [Table("excelreport")]
    public partial class Excelreport : SysEntity
    {
        public Excelreport()
        {
        }

        /// <summary>
	    /// 名称
	    /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
	    /// sql
	    /// </summary>
        [Column("sql")]
        public string Sql { get; set; }
        /// <summary>
	    /// 参数
	    /// </summary>
        [Column("params")]
        public string Params { get; set; }
        /// <summary>
	    /// 备注
	    /// </summary>
        [Column("remark")]
        public string Remark { get; set; }
    }
}