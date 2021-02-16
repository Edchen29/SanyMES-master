using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 工位工序表
	/// </summary>
    [Table("step_station")]
    public partial class StepStation : SysEntity
    {
        public StepStation()
        {
        }

        /// <summary>
	    /// 工序类型
	    /// </summary>
        [Column("stepType")]
        public string StepType { get; set; }
        /// <summary>
	    /// 工位标识
	    /// </summary>
        [Column("stationId")]
        public int? StationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("trussTakeStationId")]
        public int? TrussTakeStationId { get; set; }
        /// <summary>
	    /// 
	    /// </summary>
        [Column("trussReleaseStationId")]
        public int? TrussReleaseStationId { get; set; }
    }
}