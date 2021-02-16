// ***********************************************************************
// <summary>角色模型视图</summary>
// ***********************************************************************

using Infrastructure;

namespace WebRepository
{
    public partial class SysRoleView: SysEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
	    /// 角色类型
	    /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 所属组织名称，多个可用，分隔
        /// </summary>
        public string Organizations { get; set; }

        /// <summary>
        /// 所属组织ID，多个可用，分隔
        /// </summary>
        public string OrganizationIds { get; set; }

        /// <summary>
        ///是否属于某用户 
        /// </summary>
        public bool Checked { get; set; }

        public static implicit operator SysRoleView(SysRole role)
        {
            return role.MapTo<SysRoleView>();
        }

        public static implicit operator SysRole(SysRoleView rolevm)
        {
            return rolevm.MapTo<SysRole>();
        }

    }
}
