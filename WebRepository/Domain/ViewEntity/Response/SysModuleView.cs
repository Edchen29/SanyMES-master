using Infrastructure;
using System.Collections.Generic;

namespace WebRepository
{
    public class SysModuleView: TreeEntity
    {
        /// <summary>
        /// 主页面URL
        /// </summary>
        /// <returns></returns>
        public string Url { get; set; }

        /// <summary>
        /// 节点图标文件名称
        /// </summary>
        /// <returns></returns>
        public string IconName { get; set; }

        public bool Checked { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }

        public string Code { get; set; }

        public int IsShow { get; set; }


        /// <summary>
        /// 模块中的元素
        /// </summary>
        public List<SysModuleElement> Elements { get; set; }

        public static implicit operator SysModuleView(SysModule module)
        {
            return module.MapTo<SysModuleView>();
        }

        public static implicit operator SysModule(SysModuleView view)
        {
            return view.MapTo<SysModule>();
        }
    }
}