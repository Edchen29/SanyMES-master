using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 制造BOM接口明细表
	/// </summary>
    [Table("interface_mbom_detail")]
    public partial class InterfaceMbomDetail : SysEntity
    {
        public InterfaceMbomDetail()
        {
        }

        /// <summary>
	    /// 主表标识
	    /// </summary>
        [Column("mbomHeaderId")]
        public int? MbomHeaderId { get; set; }
        /// <summary>
	    /// 产品代号
	    /// </summary>
        [Column("productCode")]
        public string ProductCode { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        [Column("stepCode")]
        public string StepCode { get; set; }
        /// <summary>
	    /// 工序标识
	    /// </summary>
        [Column("stepId")]
        public int? StepId { get; set; }
        /// <summary>
	    /// 物料
	    /// </summary>
        [Column("materialCode")]
        public string MaterialCode { get; set; }
        /// <summary>
	    /// 图号
	    /// </summary>
        [Column("drawingNumber")]
        public string DrawingNumber { get; set; }
        /// <summary>
	    /// 基数数量
	    /// </summary>
        [Column("baseQty")]
        public decimal? BaseQty { get; set; }
        /// <summary>
	    /// 需要质检
	    /// </summary>
        [Column("isCheck")]
        public bool? IsCheck { get; set; }
        /// <summary>
	    /// 交易标记
	    /// </summary>
        [Column("transferMark")]
        public int? TransferMark { get; set; }
        /// <summary>
	    /// 交易报错信息
	    /// </summary>
        [Column("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}