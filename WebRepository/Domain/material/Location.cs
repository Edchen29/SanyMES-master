using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 库位表
	/// </summary>
    [Table("location")]
    public partial class Location : SysEntity
    {
        public Location()
        {
        }

        /// <summary>
	    /// 库位
	    /// </summary>
        [Column("code")]
        public string Code { get; set; }
        /// <summary>
	    /// 线体标识
	    /// </summary>
        [Column("lineId")]
        public int? LineId { get; set; }
        /// <summary>
	    /// 线体
	    /// </summary>
        [Column("lineCode")]
        public string LineCode { get; set; }
        /// <summary>
	    /// 行
	    /// </summary>
        [Column("row")]
        public short? Row { get; set; }
        /// <summary>
	    /// 列
	    /// </summary>
        [Column("column")]
        public short? Column { get; set; }
        /// <summary>
	    /// 层
	    /// </summary>
        [Column("layer")]
        public short? Layer { get; set; }
        /// <summary>
	    /// 格
	    /// </summary>
        [Column("grid")]
        public short? Grid { get; set; }
        /// <summary>
	    /// 1号堆垛机双伸位索引
	    /// </summary>
        [Column("rowIndex1")]
        public short? RowIndex1 { get; set; }
        /// <summary>
	    /// 2号堆垛机双伸位索引
	    /// </summary>
        [Column("rowIndex2")]
        public short? RowIndex2 { get; set; }
        /// <summary>
	    /// 堆垛机编码
	    /// </summary>
        [Column("srmCode")]
        public string SrmCode { get; set; }
        /// <summary>
	    /// 目标区域
	    /// </summary>
        [Column("destinationArea")]
        public string DestinationArea { get; set; }
        /// <summary>
	    /// 巷道
	    /// </summary>
        [Column("roadWay")]
        public int? RoadWay { get; set; }
        /// <summary>
	    /// 库位类型
	    /// </summary>
        [Column("type")]
        public string Type { get; set; }
        /// <summary>
	    /// 高度上限
	    /// </summary>
        [Column("maxHeight")]
        public decimal? MaxHeight { get; set; }
        /// <summary>
	    /// 重量上限
	    /// </summary>
        [Column("maxWeight")]
        public decimal? MaxWeight { get; set; }
        /// <summary>
	    /// 是否禁用
	    /// </summary>
        [Column("isStop")]
        public bool? IsStop { get; set; }
        /// <summary>
	    /// 是否锁定
	    /// </summary>
        [Column("isLock")]
        public short? IsLock { get; set; }
        /// <summary>
	    /// 容器id号
	    /// </summary>
        [Column("containerId")]
        public int? ContainerId { get; set; }
        /// <summary>
	    /// 容器编码
	    /// </summary>
        [Column("containerCode")]
        public string ContainerCode { get; set; }
        /// <summary>
	    /// 状态
	    /// </summary>
        [Column("status")]
        public string Status { get; set; }
        /// <summary>
	    /// 区域编码
	    /// </summary>
        [Column("zoneCode")]
        public string ZoneCode { get; set; }
        /// <summary>
	    /// 仓库编码
	    /// </summary>
        [Column("warehouseCode")]
        public string WarehouseCode { get; set; }
        /// <summary>
	    /// 物料编号
	    /// </summary>
        [Column("goodsNo")]
        public string GoodsNo { get; set; }
        /// <summary>
	    /// 上次盘点日期
	    /// </summary>
        [Column("lastCycleCountDate")]
        public System.DateTime? LastCycleCountDate { get; set; }
    }
}