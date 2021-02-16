using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
    /// 下发AGV配送数据
    /// </summary>
    public partial class SendTaskModel
    {
        /// <summary>
        /// 任务号
        /// </summary>
        [Column("taskNo")]
        public string TaskNo { get; set; }
        /// <summary>
        /// 需求工位
        /// </summary>
        [Column("needStation")]
        public string NeedStation { get; set; }
        /// <summary>
        /// 小车编号
        /// </summary>
        [Column("carNo")]
        public string CarNo { get; set; }
        /// <summary>
        /// 料框编号
        /// </summary>
        [Column("containerCode")]
        public string ContainerCode { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Column("status")]
        public int? Status { get; set; }
        /// <summary>
        /// 需配送的订单数据列表
        /// </summary>
        public List<SendTask> Data;
        public partial class SendTask
        {
            /// <summary>
            /// 订单号
            /// </summary>
            [Column("orderCode")]
            public string OrderCode { get; set; }
            /// <summary>
            /// 序列号
            /// </summary>
            [Column("serialNumber")]
            public string SerialNumber { get; set; }
        }
    }
}