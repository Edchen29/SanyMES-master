using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// AGV监控
	/// </summary>
    [Table("agv_monitor")]
    public partial class AgvMonitor : SysEntity
    {
        public AgvMonitor()
        {
        }

        /// <summary>
	    /// AGV编号
	    /// </summary>
        [Column("carNo")]
        public string CarNo { get; set; }
        /// <summary>
	    /// 任务号
	    /// </summary>
        [Column("taskNo")]
        public string TaskNo { get; set; }
        /// <summary>
	    /// 电量
	    /// </summary>
        [Column("percentCapacity")]
        public int? PercentCapacity { get; set; }
        /// <summary>
	    /// 异常标志
	    /// </summary>
        [Column("exceptionFlag")]
        public int? ExceptionFlag { get; set; }
        /// <summary>
	    /// 状态
	    /// </summary>
        [Column("state")]
        public int? State { get; set; }
        /// <summary>
	    /// 异常信息
	    /// </summary>
        [Column("exceptionInfo")]
        public string ExceptionInfo { get; set; }
    }
}