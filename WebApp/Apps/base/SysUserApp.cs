using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using WebRepository;

namespace WebApp
{
    public class SysUserApp : WebApp<SysUser>
    {
        private SysRevelanceApp _revelanceApp;

        private IAuth _auth;

        public SysUserApp(IUnitWork unitWork, IRepository<SysUser> repository,
            SysRevelanceApp app, IAuth auth) : base(unitWork, repository)
        {
            _revelanceApp = app;
            _auth = auth;
        }

        public SysUserApp SetLoginInfo(LoginInfo loginInfo)
        {
            Repository._loginInfo = loginInfo;
            _revelanceApp = _revelanceApp.SetLoginInfo(Repository._loginInfo);
            return this;
        }

        /// <summary>
        /// 加载当前登录用户可访问的一个部门及子部门全部用户
        /// </summary>
        public TableData Load(PageReq request, int? orgId)
        {
            var loginUser = _auth.GetCurrentUser();

            string cascadeId = ".0.";
            if (orgId != null)
            {
                var org = loginUser.Orgs.SingleOrDefault(u => u.Id == orgId.Value);
                cascadeId = org.CascadeId;
            }

            var ids = loginUser.Orgs.Where(u => u.CascadeId.Contains(cascadeId)).Select(u => u.Id.Value).ToArray();
            var userIds = _revelanceApp.Get(Define.USERORG, false, ids);

            //SQL2008不支持分页的语法
            //var data = UnitWork.Find<User>(u => userIds.Contains(u.Id)).ToList();
            //var users = data
            //       .OrderBy(u => u.Name)
            //       .Skip((request.page - 1) * request.limit)
            //       .Take(request.limit);

            var users = Repository.Find(u => userIds.Contains(u.Id.Value) && u.Account != "System")
                   .OrderBy(u => u.Name)
                   .Skip((request.page - 1) * request.limit)
                   .Take(request.limit);

            var records = Repository.GetCount(u => userIds.Contains(u.Id.Value) && u.Account != "System");

            var userviews = new List<SysUserView>();
            foreach (var user in users.ToList())
            {
                SysUserView uv = user;
                var orgs = LoadByUser(user.Id.Value);
                uv.Organizations = string.Join(",", orgs.Select(u => u.Name).ToList());
                uv.OrganizationIds = string.Join(",", orgs.Select(u => u.Id).ToList());
                userviews.Add(uv);
            }

            return new TableData
            {
                count = records,
                data = userviews,
            };
        }

        public void Add(SysUserView view)
        {
            if (string.IsNullOrEmpty(view.OrganizationIds))
                throw new Exception("请为用户分配机构");

            SysUser user = view;

            if (Repository.IsExist(u => u.Account == view.Account))
            {
                throw new Exception("账号已存在");
            }
            if (Repository.IsExist(u => u.Name == view.Name))
            {
                throw new Exception("用户名已存在");
            }

            user.Password = Encryption.Encrypt(user.Password); //密码加密
            Repository.Add(user);
            view.Id = user.Id;   //要把保存后的ID存入view
            UnitWork.Save();
            int[] orgIds = Array.ConvertAll(view.OrganizationIds.Split(','), int.Parse);

            _revelanceApp.DeleteBy(Define.USERORG, user.Id.Value);
            _revelanceApp.AddRelevance(Define.USERORG, orgIds.ToLookup(u => user.Id.Value));
        }

        public void Update(SysUserView view)
        {
            if (string.IsNullOrEmpty(view.OrganizationIds))
                throw new Exception("请为用户分配机构");

            SysUser user = view;

            Repository.Update(u => u.Id == view.Id, u => new SysUser
            {
                Account = user.Account,
                Name = user.Name,
                Sex = user.Sex,
                Status = user.Status,
                UpdateBy = Repository._loginInfo.Account,
                UpdateTime = DateTime.Now
            });

            int[] orgIds = Array.ConvertAll(view.OrganizationIds.Split(','), int.Parse);

            _revelanceApp.DeleteBy(Define.USERORG, user.Id.Value);
            _revelanceApp.AddRelevance(Define.USERORG, orgIds.ToLookup(u => user.Id.Value));
        }

        /// <summary>
        /// 加载用户的所有机构
        /// </summary>
        public IEnumerable<SysDept> LoadByUser(int userId)
        {
            var result = from userorg in UnitWork.Find<SysRelevance>(null)
                         join org in UnitWork.Find<SysDept>(null) on userorg.SecondId equals org.Id
                         where userorg.FirstId == userId && userorg.RelKey == Define.USERORG
                         select org;
            return result;
        }

        //修改个人密码
        public void ChangeUserPassword(string OldPassword, string Password, SysUser CurrentUser)
        {
            SysUser user = CurrentUser;

            if (user.Password.Equals(Encryption.Encrypt(OldPassword)))
            {
                if (OldPassword == Password)
                {
                    throw new Exception("新密码不应与旧密码相同");
                }
                else
                {
                    user.Password = Encryption.Encrypt(Password);
                    Repository.Update(user);
                }

            }
            else
            {
                throw new Exception("旧密码不正确");
            }
        }

        //重设用户密码
        public void ResetPassword(SysUser CurrentUser)
        {
            SysUser user = CurrentUser;
            
            user.Password = Encryption.Encrypt("123456");
            Repository.Update(user);
        }

        public SysUser GetByAccount(string account)
        {
            return Repository.FindSingle(u => u.Account == account);
        }
    }
}