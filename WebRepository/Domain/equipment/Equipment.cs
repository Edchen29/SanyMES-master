using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 设备列表
	/// </summary>
    [Table("equipment")]
    public partial class Equipment : SysEntity
    {
        public Equipment()
        {
        }

        /// <summary>
	    /// 设备编号
	    /// </summary>
        [Column("code")]
        public string Code { get; set; }
        /// <summary>
	    /// 设备名称
	    /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
	    /// 车间标识
	    /// </summary>
        [Column("workshopId")]
        public int? WorkshopId { get; set; }
        /// <summary>
	    /// 工厂标识
	    /// </summary>
        [Column("factoryId")]
        public int? FactoryId { get; set; }
        /// <summary>
	    /// 线体
	    /// </summary>
        [Column("lineCode")]
        public string LineCode { get; set; }
        /// <summary>
	    /// 线体ID
	    /// </summary>
        [Column("lineId")]
        public int? LineId { get; set; }
        /// <summary>
	    /// 工位
	    /// </summary>
        [Column("stationCode")]
        public string StationCode { get; set; }
        /// <summary>
	    /// 工位ID
	    /// </summary>
        [Column("stationId")]
        public int? StationId { get; set; }
        /// <summary>
	    /// 设备IP地址
	    /// </summary>
        [Column("Ip")]
        public string Ip { get; set; }
        /// <summary>
	    /// OPC连接名
	    /// </summary>
        [Column("connectName")]
        public string ConnectName { get; set; }
        /// <summary>
	    /// 设备对应的IED的IP
	    /// </summary>
        [Column("ledIp")]
        public string LedIp { get; set; }
        /// <summary>
	    /// 设备类型
	    /// </summary>
        [Column("equipmentTypeId")]
        public int? EquipmentTypeId { get; set; }
        /// <summary>
	    /// 是否启用
	    /// </summary>
        [Column("enable")]
        public bool? Enable { get; set; }
        /// <summary>
	    /// 巷道
	    /// </summary>
        [Column("roadWay")]
        public int? RoadWay { get; set; }
        /// <summary>
	    /// 目标区域
	    /// </summary>
        [Column("destinationArea")]
        public string DestinationArea { get; set; }
        /// <summary>
	    /// 目的地址
	    /// </summary>
        [Column("goAddress")]
        public string GoAddress { get; set; }
        /// <summary>
	    /// 自身地址
	    /// </summary>
        [Column("selfAddress")]
        public string SelfAddress { get; set; }
        /// <summary>
	    /// 回退地址
	    /// </summary>
        [Column("backAddress")]
        public string BackAddress { get; set; }
        /// <summary>
	    /// 仓库编码
	    /// </summary>
        [Column("warehouseCode")]
        public string WarehouseCode { get; set; }
        /// <summary>
	    /// 站台编码
	    /// </summary>
        [Column("stationIndex")]
        public short? StationIndex { get; set; }
        /// <summary>
	    /// 第一台堆垛机对应的排索引
	    /// </summary>
        [Column("rowIndex1")]
        public long? RowIndex1 { get; set; }
        /// <summary>
	    /// 第二台堆垛机对应的排索引
	    /// </summary>
        [Column("rowIndex2")]
        public short? RowIndex2 { get; set; }
        /// <summary>
	    /// 列
	    /// </summary>
        [Column("columnIndex")]
        public short? ColumnIndex { get; set; }
        /// <summary>
	    /// 层
	    /// </summary>
        [Column("layerIndex")]
        public short? LayerIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("transport1")]
        public string Transport1 { get; set; }
        /// <summary>
	    /// 
	    /// </summary>
        [Column("transport2")]
        public string Transport2 { get; set; }
        /// <summary>
	    /// PLC的DB地址
	    /// </summary>
        [Column("basePlcDB")]
        public string BasePlcDB { get; set; }
    }
}