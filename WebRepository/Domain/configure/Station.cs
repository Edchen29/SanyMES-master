using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 工位表
	/// </summary>
    [Table("station")]
    public partial class Station : SysEntity
    {
        public Station()
        {
        }

        /// <summary>
	    /// 线体ID
	    /// </summary>
        [Column("lineId")]
        public int? LineId { get; set; }
        /// <summary>
	    /// 线体代号
	    /// </summary>
        [Column("lineCode")]
        public string LineCode { get; set; }
        /// <summary>
	    /// 工位代号
	    /// </summary>
        [Column("code")]
        public string Code { get; set; }
        /// <summary>
	    /// 工位名称
	    /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
	    /// 顺序
	    /// </summary>
        [Column("sequence")]
        public int? Sequence { get; set; }
        /// <summary>
	    /// 工位属性
	    /// </summary>
        [Column("attribute")]
        public string Attribute { get; set; }
        /// <summary>
	    /// 是否有效
	    /// </summary>
        [Column("enable")]
        public bool? Enable { get; set; }
    }
}