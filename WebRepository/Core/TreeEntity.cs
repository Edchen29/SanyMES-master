using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
    /// 树状结构实体
    /// </summary>
    public abstract class TreeEntity : SysEntity
    {

        /// <summary>
	    /// 父级Id
	    /// </summary>
        [Column("ParentId")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        [Column("ParentName")]
        public string ParentName { get; set; }

        /// <summary>
	    /// 分支Id
	    /// </summary>
        [Column("CascadeId")]
        public string CascadeId { get; set; }

        /// <summary>
	    /// 节点名
	    /// </summary>
        [Column("Name")]
        public string Name { get; set; }
    }

}
