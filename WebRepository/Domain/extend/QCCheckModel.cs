using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{

    public partial class QCCheckModel
    {
        /// <summary>
	    /// 工单号
	    /// </summary>
        [Column("orderCode")]
        public string OrderCode { get; set; }
        /// <summary>
	    /// 产品
	    /// </summary>
        [Column("productCode")]
        public string ProductCode { get; set; }
        /// <summary>
	    /// 序列号
	    /// </summary>
        [Column("serialNumber")]
        public string SerialNumber { get; set; }
        /// <summary>
        /// 判定部件
        /// </summary>
        [Column("materialCode")]
        public string MaterialCode { get; set; }
        /// <summary>
	    /// 是否合格
	    /// </summary>
        [Column("isGood")]
        public int? IsGood { get; set; }
        /// <summary>
	    /// 不良代号
	    /// </summary>
        [Column("NGCode")]
        public string NGCode { get; set; }
        /// <summary>
	    /// 判定人员
	    /// </summary>
        [Column("NGMarkUser")]
        public string NGMarkUser { get; set; }
    }

}