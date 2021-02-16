using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{

    public partial class EquipmentWorkNodeModel
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Column("equipmentrCode")]
        public string EquipmentCode { get; set; }
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
	    /// 产品机型
	    /// </summary>
        [Column("machineType")]
        public string MachineType { get; set; }
        /// <summary>
        /// 设备状态:0=空闲，1=工作中，2=完成等
        /// </summary>
        [Column("status")]
        public string Status { get; set; }
    }

}