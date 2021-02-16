using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 产品明细接口表
	/// </summary>
    [Table("interface_product_detail")]
    public partial class InterfaceProductDetail : SysEntity
    {
        public InterfaceProductDetail()
        {
        }

        /// <summary>
	    /// 产品标识
	    /// </summary>
        [Column("productHeaderId")]
        public int? ProductHeaderId { get; set; }
        /// <summary>
	    /// 线体标识
	    /// </summary>
        [Column("lineId")]
        public int? LineId { get; set; }
        /// <summary>
	    /// 线体代号
	    /// </summary>
        [Column("lineCode")]
        public string LineCode { get; set; }
        /// <summary>
	    /// 工序标识
	    /// </summary>
        [Column("stepId")]
        public int? StepId { get; set; }
        /// <summary>
	    /// 工序
	    /// </summary>
        [Column("stepCode")]
        public string StepCode { get; set; }
        /// <summary>
	    /// 工序程序
	    /// </summary>
        [Column("programCode")]
        public string ProgramCode { get; set; }
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