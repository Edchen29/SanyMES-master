﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebRepository;

namespace WebApp
{
    public class SysRevelanceApp : WebApp<SysRelevance>
    {
        public SysRevelanceApp(IUnitWork unitWork, IRepository<SysRelevance> repository) : base(unitWork, repository)
        {
        }

        public SysRevelanceApp SetLoginInfo(LoginInfo loginInfo)
        {
            Repository._loginInfo = loginInfo;
            return this;
        }

        /// <summary>
        /// 添加关联
        /// <para>比如给用户分配资源，那么firstId就是用户ID，secIds就是资源ID列表</para>
        /// </summary>
        /// <param name="type">关联的类型，如Define.XXX</param>
        public void Assign(string type, int firstId, int[] secIds)
        {
            Assign(type, secIds.ToLookup(u => firstId));
        }

        public void Assign(string key, ILookup<int, int> idMaps)
        {
            DeleteBy(key, idMaps);
            UnitWork.BatchAdd((from sameVals in idMaps
                               from value in sameVals
                               select new SysRelevance
                               {
                                   RelKey = key,
                                   FirstId = sameVals.Key,
                                   SecondId = value,
                                   CreateBy = Repository._loginInfo.Account,
                                   CreateTime = DateTime.Now
                               }).ToArray());
            UnitWork.Save();
        }

        /// <summary>
        /// 删除关联
        /// </summary>
        /// <param name="key">关联标识</param>
        /// <param name="idMaps">关联的&lt;firstId, secondId&gt;数组</param>
        public void DeleteBy(string key, ILookup<int, int> idMaps)
        {
            foreach (var sameVals in idMaps)
            {
                foreach (var value in sameVals)
                {
                    Repository.Delete(u => u.RelKey == key && u.FirstId == sameVals.Key && u.SecondId == value);
                }
            }
        }

        /// <summary>
        /// 取消关联
        /// </summary>
        /// <param name="type">关联的类型，如Define.XXX</param>
        /// <param name="firstId">The first identifier.</param>
        /// <param name="secIds">The sec ids.</param>
        public void UnAssign(string type, int firstId, int[] secIds)
        {
            DeleteBy(type, secIds.ToLookup(u => firstId));
        }

        public void DeleteBy(string key, params int[] firstIds)
        {
            List<int> firstIdslist = firstIds.ToList();
            Repository.Delete(u => firstIdslist.Contains(u.FirstId.Value) && u.RelKey == key);
        }

        /// <summary>
        /// 添加新的关联
        /// </summary>
        /// <param name="key">关联标识</param>
        /// <param name="idMaps">关联的&lt;firstId, secondId&gt;数组</param>
        public void AddRelevance(string key, ILookup<int, int> idMaps)
        {
            DeleteBy(key, idMaps);
            UnitWork.BatchAdd<SysRelevance>((from sameVals in idMaps
                                             from value in sameVals
                                             select new SysRelevance
                                             {
                                                 RelKey = key,
                                                 FirstId = sameVals.Key,
                                                 SecondId = value,
                                                 CreateBy = Repository._loginInfo.Account,
                                                 CreateTime = DateTime.Now
                                             }).ToArray());
            UnitWork.Save();
        }

        /// <summary>
        /// 根据关联表的一个键获取另外键的值
        /// </summary>
        /// <param name="key">映射标识</param>
        /// <param name="returnSecondIds">返回的是否为映射表的第二列，如果不是则返回第一列</param>
        /// <param name="ids">已知的ID列表</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<int> Get(string key, bool returnSecondIds, params int[] ids)
        {
            List<int> idlist = ids.ToList();
            if (returnSecondIds)
            {
                return Repository.Find(u => u.RelKey == key
                                       && idlist.Contains(u.FirstId.Value)).Select(u => u.SecondId.Value).ToList();
            }
            else
            {
                return Repository.Find(u => u.RelKey == key
                     && idlist.Contains(u.SecondId.Value)).Select(u => u.FirstId.Value).ToList();
            }
        }
    }
}