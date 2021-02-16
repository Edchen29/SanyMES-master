using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    public partial class MaterialDemandConfirmModel
    {
        public List<MaterialDemandConfirm> Data;
    }
    /// <summary>
	/// 物料需求确认
	/// </summary>
    public partial class MaterialDemandConfirm
    {
        /// <summary>
        /// 需求ID
        /// </summary>
        [Column("id")]
        public int? Id { get; set; }
        /// <summary>
	    /// 料框编号
	    /// </summary>
        [Column("containerCode")]
        public string ContainerCode { get; set; }
        /// <summary>
	    /// 备料存放位置
	    /// </summary>
        [Column("containerPlace")]
        public string ContainerPlace { get; set; }
        /// <summary>
	    /// 订单号
	    /// </summary>
        [Column("orderCode")]
        public string OrderCode { get; set; }
        /// <summary>
	    /// 产品
	    /// </summary>
        [Column("productCode")]
        public string ProductCode { get; set; }
        /// <summary>
	    /// 部件料号
	    /// </summary>
        [Column("partMaterialCode")]
        public string PartMaterialCode { get; set; }
        /// <summary>
	    /// 物料编码
	    /// </summary>
        [Column("materialCode")]
        public string MaterialCode { get; set; }
        /// <summary>
	    /// 配送数量
	    /// </summary>
        [Column("distributeQty")]
        public decimal? DistributeQty { get; set; }

    }
}