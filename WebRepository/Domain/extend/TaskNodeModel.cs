using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{

    public partial class TaskNodeModel
    {
        /// <summary>
        /// 任务号
        /// </summary>
        [Column("taskNo")]
        public string TaskNo { get; set; }
        ///// <summary>
        ///// 序列号
        ///// </summary>
        //[Column("serialNumber")]
        //public string SerialNumber { get; set; }
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

    }

}