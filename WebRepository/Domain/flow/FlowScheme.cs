using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 工作流模板信息表
	/// </summary>
    [Table("FlowScheme")]
    public partial class FlowScheme : SysEntity
    {
        public FlowScheme()
        {
        }
        /// <summary>
	    /// 流程编号
	    /// </summary>
        [Column("SchemeCode")]
        public string SchemeCode { get; set; }
        /// <summary>
	    /// 流程名称
	    /// </summary>
        [Column("SchemeName")]
        public string SchemeName { get; set; }
        /// <summary>
	    /// 流程分类
	    /// </summary>
        [Column("SchemeType")]
        public string SchemeType { get; set; }
        /// <summary>
	    /// 流程内容
	    /// </summary>
        [Column("SchemeContent")]
        public string SchemeContent { get; set; }
        /// <summary>
	    /// 产品
	    /// </summary>
        [Column("ProductId")]
        public int? ProductId { get; set; }
        /// <summary>
	    /// 机型
	    /// </summary>
        [Column("MachineType")]
        public string MachineType { get; set; }
        /// <summary>
	    /// 删除标记
	    /// </summary>
        [Column("DeleteMark")]
        public int? DeleteMark { get; set; }
        /// <summary>
	    /// 有效
	    /// </summary>
        [Column("Disabled")]
        public int? Disabled { get; set; }
        /// <summary>
	    /// 备注
	    /// </summary>
        [Column("Description")]
        public string Description { get; set; }
    }
}