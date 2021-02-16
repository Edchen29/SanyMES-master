using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 模块元素
	/// </summary>
    [Table("sys_module_element")]
    public partial class SysModuleElement : SysEntity
    {
        public SysModuleElement()
        {
        }

        /// <summary>
	    /// 元素Id
	    /// </summary>
        [Column("domId")]
        public string DomId { get; set; }
        /// <summary>
	    /// 元素名
	    /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
	    /// 脚本
	    /// </summary>
        [Column("script")]
        public string Script { get; set; }
        /// <summary>
	    /// 图标
	    /// </summary>
        [Column("icon")]
        public string Icon { get; set; }
        /// <summary>
	    /// 样式
	    /// </summary>
        [Column("class")]
        public string Class { get; set; }
        /// <summary>
	    /// 备注
	    /// </summary>
        [Column("remark")]
        public string Remark { get; set; }
        /// <summary>
	    /// 排序号
	    /// </summary>
        [Column("sort")]
        public int? Sort { get; set; }
        /// <summary>
	    /// 模组Id
	    /// </summary>
        [Column("moduleId")]
        public int? ModuleId { get; set; }
        /// <summary>
	    /// 区域菜单
	    /// </summary>
        [Column("areaMenus")]
        public string AreaMenus { get; set; }
    }
}