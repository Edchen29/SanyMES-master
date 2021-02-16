using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 定时任务调度表
	/// </summary>
    [Table("sys_job")]
    public partial class SysJob : SysEntity
    {
        public SysJob()
        {
        }

        /// <summary>
	    /// 任务名称
	    /// </summary>
        [Column("jobName")]
        public string JobName { get; set; }
        /// <summary>
	    /// 任务组名
	    /// </summary>
        [Column("jobGroup")]
        public string JobGroup { get; set; }
        /// <summary>
	    /// 任务方法
	    /// </summary>
        [Column("methodName")]
        public string MethodName { get; set; }
        /// <summary>
	    /// 参数
	    /// </summary>
        [Column("methodParams")]
        public string MethodParams { get; set; }
        /// <summary>
	    /// cron执行表达式
	    /// </summary>
        [Column("cronExpression")]
        public string CronExpression { get; set; }
        /// <summary>
	    /// 
	    /// </summary>
        [Column("lastFireTime")]
        public System.DateTime? LastFireTime { get; set; }
        /// <summary>
	    /// 
	    /// </summary>
        [Column("nextFireTime")]
        public System.DateTime? NextFireTime { get; set; }
        /// <summary>
	    /// 错误策略
	    /// </summary>
        [Column("misfirePolicy")]
        public string MisfirePolicy { get; set; }
        /// <summary>
	    /// 状态（0正常 1暂停）
	    /// </summary>
        [Column("status")]
        public string Status { get; set; }
        /// <summary>
	    /// 备注信息
	    /// </summary>
        [Column("remark")]
        public string Remark { get; set; }
    }
}