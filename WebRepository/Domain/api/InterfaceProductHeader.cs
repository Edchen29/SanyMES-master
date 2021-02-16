using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 产品接口表
	/// </summary>
    [Table("interface_product_header")]
    public partial class InterfaceProductHeader : SysEntity
    {
        public InterfaceProductHeader()
        {
        }

        /// <summary>
	    /// 产品代号
	    /// </summary>
        [Column("code")]
        public string Code { get; set; }
        /// <summary>
	    /// 产品名称
	    /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
	    /// 类别
	    /// </summary>
        [Column("type")]
        public string Type { get; set; }
        /// <summary>
	    /// 图号
	    /// </summary>
        [Column("drawingNumber")]
        public string DrawingNumber { get; set; }
        /// <summary>
	    /// 外形尺寸
	    /// </summary>
        [Column("specification")]
        public string Specification { get; set; }
        /// <summary>
	    /// 重量
	    /// </summary>
        [Column("weight")]
        public decimal? Weight { get; set; }
        /// <summary>
	    /// 机型
	    /// </summary>
        [Column("machineType")]
        public string MachineType { get; set; }
        /// <summary>
	    /// 生产车间
	    /// </summary>
        [Column("workShop")]
        public string WorkShop { get; set; }
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