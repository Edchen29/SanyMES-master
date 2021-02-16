using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 物料配送任务表
	/// </summary>
    [Table("material_distribute_task_header")]
    public partial class MaterialDistributeTaskHeader : SysEntity
    {
        public MaterialDistributeTaskHeader()
        {
        }

        /// <summary>
	    /// AGV任务号
	    /// </summary>
        [Column("taskNo")]
        public string TaskNo { get; set; }
        /// <summary>
	    /// 叫料需求标识
	    /// </summary>
        [Column("materialCallId")]
        public int? MaterialCallId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        [Column("productId")]
        public int? ProductId { get; set; }
        /// <summary>
	    /// 产品代号
	    /// </summary>
        [Column("productCode")]
        public string ProductCode { get; set; }
        /// <summary>
	    /// 小车编号
	    /// </summary>
        [Column("carNo")]
        public string CarNo { get; set; }
        /// <summary>
	    /// 料框编码
	    /// </summary>
        [Column("containerCode")]
        public string ContainerCode { get; set; }
        /// <summary>
	    /// 料框类型
	    /// </summary>
        [Column("containerType")]
        public string ContainerType { get; set; }
        /// <summary>
	    /// 配送操作员
	    /// </summary>
        [Column("userCode")]
        public string UserCode { get; set; }
        /// <summary>
	    /// 需求工位
	    /// </summary>
        [Column("needStation")]
        public string NeedStation { get; set; }
        /// <summary>
	    /// 需求位置
	    /// </summary>
        [Column("locationCode")]
        public string LocationCode { get; set; }
        /// <summary>
	    /// 呼叫类型
	    /// </summary>
        [Column("callType")]
        public string CallType { get; set; }
        /// <summary>
	    /// 起始位置
	    /// </summary>
        [Column("startPlace")]
        public string StartPlace { get; set; }
        /// <summary>
	    /// 目的位置
	    /// </summary>
        [Column("endPlace")]
        public string EndPlace { get; set; }
        /// <summary>
	    /// 需求时间
	    /// </summary>
        [Column("needTime")]
        public System.DateTime? NeedTime { get; set; }
        /// <summary>
	    /// 响应需求时间
	    /// </summary>
        [Column("responseTime")]
        public System.DateTime? ResponseTime { get; set; }
        /// <summary>
	    /// 结束时间
	    /// </summary>
        [Column("finishTime")]
        public System.DateTime? FinishTime { get; set; }
        /// <summary>
	    /// 状态
	    /// </summary>
        [Column("status")]
        public int? Status { get; set; }
        /// <summary>
        /// 备料确认
        /// </summary>
        [Column("materialConfirm")]
        public int? MaterialConfirm { get; set; }
    }
}