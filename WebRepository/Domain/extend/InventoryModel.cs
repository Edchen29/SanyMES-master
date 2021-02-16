using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 库存查询模型
	/// </summary>
    public partial class InventoryModel
    {
        /// <summary>
        /// 仓库类型
        /// </summary>
        [Column("warehouseType")]
        public string WarehouseType { get; set; }
        /// <summary>
	    /// 库位编号
	    /// </summary>
        [Column("locationCode")]
        public string LocationCode { get; set; }
        /// <summary>
	    /// 容器编码
	    /// </summary>
        [Column("containerCode")]
        public string ContainerCode { get; set; }
        /// <summary>
	    /// 物料编码
	    /// </summary>
        [Column("materialCode")]
        public string MaterialCode { get; set; }
        /// <summary>
	    /// 批号
	    /// </summary>
        [Column("lot")]
        public string Lot { get; set; }
        /// <summary>
	    /// 库存状态
	    /// </summary>
        [Column("status")]
        public string Status { get; set; }
        /// <summary>
	    /// 数量
	    /// </summary>
        [Column("qty")]
        public decimal? Qty { get; set; }
    }
}