// ***********************************************************************
// <summary>
// 获取登录用户的全部信息
// 所有和当前登录用户相关的操作都在这里
// </summary>
// ***********************************************************************

using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp;
using WebRepository;

namespace WebMvc
{
    [Area("base")]
    public class UserSessionController : BaseController
    {
        private readonly AuthStrategyContext _authStrategyContext;

        public UserSessionController(IAuth authUtil) : base(authUtil)
        {
            _authStrategyContext = _authUtil.GetCurrentUser();
        }

        public List<string> GetCurrentUser()
        {
            return new List<string> { _authStrategyContext.User.Account, _authStrategyContext.User.Name };
        }

        /// <summary>
        /// 获取登录用户可访问的所有模块，及模块的操作菜单
        /// </summary>
        public string GetModulesTree()
        {
            //OpenAuth原左边栏菜单
            var moduleTree = _authStrategyContext.Modules.GenerateTree(u => u.Id, u => u.ParentId);
            return JsonHelper.Instance.Serialize(moduleTree);
        }

        /// <summary>
        /// datatable结构的模块列表
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        public string GetModulesTable(int pId, PageReq pageRequest)
        {
            string cascadeId = ".0.";
            if (pId != 0)
            {
                var obj = _authStrategyContext.Modules.SingleOrDefault(u => u.Id == pId);
                if (obj != null)
                {
                    cascadeId = obj.CascadeId;
                }                
            }

            var query = _authStrategyContext.Modules
                .Where(u => u.CascadeId.Contains(cascadeId));
            var data = query.OrderBy(u => u.Id)
                .Skip((pageRequest.page - 1) * pageRequest.limit)
                .Take(pageRequest.limit);

            return JsonHelper.Instance.Serialize(new TableData
            {
                data = data.ToList(),
                count = query.Count(),
            });
        }

        /// <summary>
        /// 获取用户可访问的模块列表
        /// </summary>
        public string GetModules()
        {
            return JsonHelper.Instance.Serialize(_authStrategyContext.Modules);
        }

        /// <summary>
        /// 获取登录用户可访问的所有部门
        /// <para>用于树状结构</para>
        /// </summary>
        public string GetOrgs()
        {
            return JsonHelper.Instance.Serialize(_authStrategyContext.Orgs);
        }

        /// <summary>
        /// 加载机构的全部下级机构
        /// </summary>
        /// <param name="orgId">机构ID</param>
        /// <param name="pageRequest">机构ID</param>
        /// <returns></returns>
        public string GetSubOrgs(int orgId, PageReq pageRequest)
        {
            string cascadeId = ".0.";
            if (orgId != 0)
            {
                var org = _authStrategyContext.Orgs.SingleOrDefault(u => u.Id == orgId);
                if (org == null)
                {
                    return JsonHelper.Instance.Serialize(new TableData
                    {
                        msg = "未找到指定的节点",
                        code = 500,
                    });
                }
                cascadeId = org.CascadeId;
            }

            var query = _authStrategyContext.Orgs.Where(u => u.CascadeId.Contains(cascadeId));
            var data = query.OrderBy(u => u.Id)
                .Skip((pageRequest.page - 1) * pageRequest.limit)
                .Take(pageRequest.limit);

            return JsonHelper.Instance.Serialize(new TableData
            {
                data = data.ToList(),
                count = query.Count(),
            });
        }
    }
}