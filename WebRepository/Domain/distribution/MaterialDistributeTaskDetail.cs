using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 物料配送任务表
	/// </summary>
    [Table("material_distribute_task_detail")]
    public partial class MaterialDistributeTaskDetail : SysEntity
    {
        public MaterialDistributeTaskDetail()
        {
        }

        /// <summary>
	    /// 头表标识
	    /// </summary>
        [Column("materialDistributeTaskHeaderId")]
        public int? MaterialDistributeTaskHeaderId { get; set; }
        /// <summary>
	    /// 订单编号
	    /// </summary>
        [Column("orderCode")]
        public string OrderCode { get; set; }
        /// <summary>
	    /// 容器编码【料框编码】
	    /// </summary>
        [Column("containerCode")]
        public string ContainerCode { get; set; }
        /// <summary>
	    /// 物料编码
	    /// </summary>
        [Column("materialCode")]
        public string MaterialCode { get; set; }
        /// <summary>
	    /// 序列号
	    /// </summary>
        [Column("serialNumber")]
        public string SerialNumber { get; set; }
        /// <summary>
	    /// 数量
	    /// </summary>
        [Column("qty")]
        public decimal? Qty { get; set; }
        /// <summary>
	    /// 配盘操作员
	    /// </summary>
        [Column("userCode")]
        public string UserCode { get; set; }
    }
}