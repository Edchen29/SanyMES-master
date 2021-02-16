using Infrastructure;

namespace WebRepository
{
    public  class SysUserView: SysEntity
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string Account { get; set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string PassWord { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public int Sex { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        /// <returns></returns>
        public int Status { get; set; }

        /// <summary>
        /// 组织类型
        /// </summary>
        /// <returns></returns>
        public int Type { get; set; }

        /// <summary>
        /// 所属组织名称，多个可用，分隔
        /// </summary>
        /// <value>The organizations.</value>
        public string Organizations { get; set; }

        public string OrganizationIds { get; set; }

        public static implicit operator SysUserView(SysUser user)
        {
            return user.MapTo<SysUserView>();
        }

        public static implicit operator SysUser(SysUserView view)
        {
            return view.MapTo<SysUser>();
        }

        public SysUserView()
        {
            Organizations = string.Empty;
            OrganizationIds = string.Empty;
        }
    }
}
