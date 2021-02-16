using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{

    public partial class Revise
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Column("orderCode")]
        public string OrderCode { get; set; }
        /// <summary>
        /// 修正类型
        /// </summary>
        [Column("reviseType")]
        public string ReviseType { get; set; }

    }
    /// <summary>
    /// 订单修正数据
    /// </summary>
    public partial class ReviseModel
    {
        /// <summary>
        /// 修正数据列表
        /// </summary>
        public List<Revise> Data;
    }
}