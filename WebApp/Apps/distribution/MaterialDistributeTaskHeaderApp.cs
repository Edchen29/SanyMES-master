using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using WebRepository;

namespace WebApp
{
    /// <summary>
	/// 物料配送任务表
	/// </summary>
    
    public partial class MaterialDistributeTaskHeaderApp
    {
        private IUnitWork _unitWork;
        private BaseDBContext _context;
        public IRepository<MaterialDistributeTaskHeader> _app;
        public IRepository<MaterialDistributeTaskDetail> _appd;
        private static IHostingEnvironment _hostingEnvironment;
        ExcelHelper imp = new ExcelHelper(_hostingEnvironment);
        private static CookieContainer cookies = new CookieContainer();
        HttpHelper httpHelper = new HttpHelper("http://localhost:23512");

        public MaterialDistributeTaskHeaderApp(IUnitWork unitWork, IRepository<MaterialDistributeTaskHeader> repository, IRepository<MaterialDistributeTaskDetail> repositoryd, IHostingEnvironment hostingEnvironment, BaseDBContext context)
        {
            _unitWork = unitWork;
            _app = repository;
            _appd = repositoryd;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        
        public MaterialDistributeTaskHeaderApp SetLoginInfo(LoginInfo loginInfo)
        {
            _app._loginInfo = loginInfo;
            _appd._loginInfo = loginInfo;
            return this;
        }
        
        public TableData Load(PageReq pageRequest, MaterialDistributeTaskHeader entity)
        {
            return _app.Load(pageRequest, entity);
        }

        public void Ins(MaterialDistributeTaskHeader entity)
        {
            _app.Add(entity);
        }

        public void Upd(MaterialDistributeTaskHeader entity)
        {
            if (entity.Status == AGVTaskState.配送完成 || entity.Status == AGVTaskState.放料车完成)
            {
                //同步更新物料呼叫状态为完成
                MaterialCallHeader materialCallHeader = _unitWork.FindSingle<MaterialCallHeader>(u => u.Id.Equals(entity.MaterialCallId));
                materialCallHeader.Status = CallStatus.完成;
                materialCallHeader.UpdateTime = DateTime.Now;
                materialCallHeader.UpdateBy = _app._loginInfo.Account;
                _unitWork.Update(materialCallHeader);
                if (entity.Status == AGVTaskState.配送完成)
                {
                    List<MaterialCallDetail> materialCallDetail = _unitWork.Find<MaterialCallDetail>(u => u.CallHeaderId.Equals(materialCallHeader.Id)).ToList();
                    foreach (MaterialCallDetail mcdetail in materialCallDetail)
                    {
                        //同步更新物料需求状态
                        List<MaterialDemand> materialDemands = _unitWork.Find<MaterialDemand>(u => u.OrderCode.Equals(mcdetail.OrderCode) && u.ProductCode == mcdetail.ProductCode).ToList();
                        foreach (MaterialDemand md in materialDemands)
                        {
                            md.Status = CallStatus.完成;
                            md.UpdateTime = DateTime.Now;
                            md.UpdateBy = _app._loginInfo.Account;
                            _unitWork.Update(md);
                        }
                    }
                }

                //更新缓存区位置存放的料框编号和状态
                Location location = _unitWork.Find<Location>(u => u.Code == entity.LocationCode).FirstOrDefault();
                location.UpdateTime = DateTime.Now;
                location.UpdateBy = _app._loginInfo.Account;
                location.ContainerCode = entity.ContainerCode;
                location.Status = LocationStatus.有货;
                _unitWork.Update(location);

                //容器
                Container container = _unitWork.Find<Container>(u => u.Code == entity.ContainerCode).FirstOrDefault();
                container.UpdateTime = DateTime.Now;
                container.UpdateBy = _app._loginInfo.Account;
                container.LocationCode = entity.LocationCode;
                container.Status = ContainerStatus.有;
                _unitWork.Update(container);

            }
            if (entity.Status== AGVTaskState.回收料框完成 || entity.Status == AGVTaskState.取工件完成)
            {
                    //更新缓存区位置存放的料框编号和状态
                    Location location = _unitWork.Find<Location>(u => u.Code == entity.LocationCode).FirstOrDefault();
                    location.UpdateTime = DateTime.Now;
                    location.UpdateBy = _app._loginInfo.Account;
                    location.ContainerCode = "";
                    location.Status = LocationStatus.空仓位;
                    _unitWork.Update(location);

                    //容器
                    Container container = _unitWork.Find<Container>(u => u.Code == entity.ContainerCode).FirstOrDefault();
                    container.UpdateTime = DateTime.Now;
                    container.UpdateBy = _app._loginInfo.Account;
                    container.LocationCode = "";
                    container.Status = ContainerStatus.空;
                    _unitWork.Update(container);
            }
            entity.Status = AGVTaskState.任务完成;
            _app.Update(entity);
        }

        public void DelByIds(int[] ids)
        {
            _app.Delete(u => ids.Contains(u.Id.Value));
        }
        
        public MaterialDistributeTaskHeader FindSingle(Expression<Func<MaterialDistributeTaskHeader, bool>> exp)
        {
            return _app.FindSingle(exp);
        }

        public IQueryable<MaterialDistributeTaskHeader> Find(Expression<Func<MaterialDistributeTaskHeader, bool>> exp)
        {
            return _app.Find(exp);
        }

        public Response ImportIn(IFormFile excelfile)
        {
            Response result = new Infrastructure.Response();
            List<MaterialDistributeTaskHeader> exp = imp.ConvertToModel<MaterialDistributeTaskHeader>(excelfile);
            string sErrorMsg = "";

            for (int i = 0; i < exp.Count; i++)
            {
                try
                {
                    MaterialDistributeTaskHeader e = exp[i];
                    e.Id = null;
                    _app.Add(e);
                }
                catch (Exception ex)
                {
                    sErrorMsg += "第" + (i + 2) + "行:" + ex.Message + "<br>";
                    result.Message = sErrorMsg;
                    break;
                }
            }
            if (sErrorMsg.Equals(string.Empty))
            {
                if (exp.Count == 0)
                {
                    sErrorMsg += "没有发现有效数据, 请确定模板是否正确， 或是否有填充数据！";
                    result.Message = sErrorMsg;
                }
                else
                {
                    result.Message = "导入完成";
                }
            }
            else
            {
                result.Status = false;
                result.Message = result.Message;
            }
            return result;
        }

        public TableData ExportData(MaterialDistributeTaskHeader entity)
        {
            return _app.ExportData(entity);
        }

        public TableData Query(MaterialDistributeTaskHeader entity)
        {
            var result = new TableData();
            var data = _app.Find(EntityToExpression<MaterialDistributeTaskHeader>.GetExpressions(entity));

            GetData(data, result);
            result.count = data.Count();

            return result;
        }

        public void GetData(IQueryable<MaterialDistributeTaskHeader> data, TableData result, PageReq pageRequest = null)
        {
            _app.GetData(data, result, pageRequest);
        }
        public TableData LoginTestApp(string url)
        {

            TableData tab = new TableData
            {
                code = 200
            };
            try
            {
               
                    string loginjson = "{\"username\":\"System\",\"password\":\"Aa123456\"}";
                    string result = httpHelper.HttpPostLogin("http://localhost:23512/api/Login/Login", loginjson, cookies);
                    //登录接口有返回TOKEN码，直接把token写入cookie
                    JObject jresult = JObject.Parse(result);
                    if (jresult["code"].ToString() == "200")
                    {
                        Cookie cookie1 = new Cookie("Token", jresult["token"].ToString());
                        cookie1.Expires = DateTime.Now.AddDays(100);
                        cookie1.Domain = "localhost";
                        cookies.Add(cookie1);
                        tab.code = 200;
                        tab.msg = "登录成功！";
                    }
                    //----适用于直接从登录接口回应头中解析读取Token码或解析loginjson = "{\"username\":\"System\",\"password\":\"Aa123456\"}"直接把账号密码写入cookie进行认证
                    //JObject jresult = JObject.Parse(result);
                    //if (!string.IsNullOrEmpty(result))
                    //{
                    //    foreach (string s in result.Split(';'))
                    //    {
                    //        string name = s.Split('=')[0].Trim();
                    //        string value = s.Contains("=") ? s.Split('=')[1].Trim() : "";
                    //        Cookie cookie1 = new Cookie(name, value);
                    //        cookie1.Domain = "localhost";
                    //        cookies.Add(cookie1);
                    //    }
                    //}
                    //--------end-------

            }
            catch (Exception ex)
            {
                tab.code = 300;
                tab.msg += ex.Message;
            }
            return tab;
        }
        public TableData SendAGVTaskApp(List<MaterialDistributeTaskHeader> mhlist,string url)
        {
            
            TableData tab = new TableData
            {
                code = 200
            };
            int hcount = 0;
            int listcount = 0;
            string sErrorMsg = "";
            listcount = mhlist.Count;
            dynamic temp = new ExpandoObject();
            try
            {
                HttpHelper httpHelper = new HttpHelper(url);
                //if (string.IsNullOrEmpty(httpHelper.GetCookie("Token", cookies)))
                if (cookies.Count == 0)
                {
                    string loginjson = "{\"username\":\"System\",\"password\":\"Aa123456\"}";
                    string result = httpHelper.HttpPostLogin("http://localhost:23512/api/Login/Login", loginjson, cookies);
                    //登录接口有返回TOKEN码，直接把token写入cookie
                    JObject jresult = JObject.Parse(result);
                    if (jresult["code"].ToString() == "200")
                    {
                        Cookie cookie1 = new Cookie("Token", jresult["token"].ToString());
                        cookie1.Expires = DateTime.Now.AddDays(100);
                        cookie1.Domain = "localhost";
                        cookies.Add(cookie1);
                    }
                    //----适用于直接从登录接口回应头中解析读取Token码或解析loginjson = "{\"username\":\"System\",\"password\":\"Aa123456\"}"直接把账号密码写入cookie进行认证
                    //JObject jresult = JObject.Parse(result);
                    //if (!string.IsNullOrEmpty(result))
                    //{
                    //    foreach (string s in result.Split(';'))
                    //    {
                    //        string name = s.Split('=')[0].Trim();
                    //        string value = s.Contains("=") ? s.Split('=')[1].Trim() : "";
                    //        Cookie cookie1 = new Cookie(name, value);
                    //        cookie1.Domain = "localhost";
                    //        cookies.Add(cookie1);
                    //    }
                    //}
                    //--------end-------
                }

                foreach (MaterialDistributeTaskHeader mdth in mhlist)
                {
                    if (mdth.Status==1)
                    {
                        //temp.postmodel = new List<dynamic>();
                        //temp.postmodel.Add(new
                        //{
                        //    OrderCode = mdth.OrderCode,
                        //    SerialNumber = "",
                        //    CarNo = "",
                        //    ContainerCode = "",
                        //    Status = "",
                        //});
                        TaskNodeModel tm = new TaskNodeModel();
                        tm.TaskNo = mdth.TaskNo;
                        var pdata = JsonHelper.Instance.Serialize(tm);
                        var responsedata = httpHelper.HttpPost(url, pdata, cookies);
                        // var responsedata = httpHelper.Post(tm, url);
                        JObject jo = JObject.Parse(responsedata);
                        JObject jr = JObject.Parse(jo["Result"].ToString());
                        if (jo["Code"].ToString() == "200")
                        {
                            //获取上料工位的缓存位置是否有料框
                            //string emptyc = _unitWork.Find<Location>(u => u.Code == mdth.LocationCode).Select(a => a.ContainerCode).FirstOrDefault();
                            MaterialDistributeTaskHeader materialDistributeTaskHeader = _unitWork.Find<MaterialDistributeTaskHeader>(u => u.TaskNo == mdth.TaskNo).FirstOrDefault();
                            materialDistributeTaskHeader.Status = AGVTaskState.任务下发;
                            //如果缓存位有空料框，任务状态为先拉空料框，否则任务为直接配送物料
                            //if (string.IsNullOrEmpty(emptyc))
                            //{
                            //    materialDistributeTaskHeader.Status = 50;
                            //}
                            //else
                            //{
                            //    materialDistributeTaskHeader.Status = 10;
                            //}
                            materialDistributeTaskHeader.CarNo = jr["CarNo"].ToString();
                            materialDistributeTaskHeader.ContainerCode = jr["ContainerCode"].ToString();
                            _app.Update(materialDistributeTaskHeader);
                            hcount = hcount + 1;
                            List<MaterialDistributeTaskDetail> materialDistributeTaskDetails = _unitWork.Find<MaterialDistributeTaskDetail>(u => u.MaterialDistributeTaskHeaderId.Equals(materialDistributeTaskHeader.Id)).ToList();
                            foreach (MaterialDistributeTaskDetail mdtDetail in materialDistributeTaskDetails)
                            {
                                mdtDetail.ContainerCode = jr["ContainerCode"].ToString();
                                _appd.Update(mdtDetail);
                            }

                        }
                        else
                        {
                            sErrorMsg += "任务号为：" + mdth.TaskNo + "接口下发失败，错误信息：" + jo["Message"].ToString() + "<br>";
                            tab.msg = sErrorMsg;
                        }
                    }
                    else {
                        sErrorMsg += "任务号为：" + mdth.TaskNo + "接口下发失败，错误信息：此任务状态不在可下发状态！<br>";
                        tab.msg = sErrorMsg;
                    }


                }

                if (listcount == hcount)
                {
                    tab.code = 200;
                    tab.msg = "任务全部成功下发！";
                }
                else
                {
                    tab.code = 200;
                    tab.msg = "下发执行完成！部分下发失败的信息如下：<br>" + tab.msg;
                }
            }
            catch (Exception ex)
            {
                tab.code = 300;
                tab.msg += ex.Message;
            }
            return tab;
        }
        public TableData TaskCancelApp(List<MaterialDistributeTaskHeader> mdthlist)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                TableData tab = new TableData();
                tab.code = 200;
                int totaltaskcount = 0;
                string sErrorMsg = "";
                try
                {
                    foreach (MaterialDistributeTaskHeader mdtaskheader in mdthlist)
                    {
                        //可以调用AGV任务取消接口，如果AGV任务还未执行，反馈可以取消则执行下面取消任务逻辑，否则无法取消任务
                        if (mdtaskheader.Status == AGVTaskState.任务生成)
                        {
                            List<MaterialDistributeTaskDetail> materialDistributeTaskDetails = _unitWork.Find<MaterialDistributeTaskDetail>(n => n.MaterialDistributeTaskHeaderId == mdtaskheader.Id).ToList();

                            foreach (MaterialDistributeTaskDetail mdtd in materialDistributeTaskDetails)
                            {
                               _unitWork.Delete(mdtd);
                            }

                            _app.Delete(mdtaskheader);
                            totaltaskcount += 1;
                        }
                        else
                        {
                            sErrorMsg += "任务号为：" + mdtaskheader.TaskNo + "任务已下发不可以取消任务！<br>";
                            tab.msg = sErrorMsg;
                        }
                    }

                    if (totaltaskcount == mdthlist.Count)
                    {
                        tab.code = 200;
                        tab.msg = "任务全部成功取消！";
                    }
                    else
                    {
                        tab.code = 200;
                        tab.msg = "取消任务已全部执行完成！部分未成功取消的信息如下：<br>" + tab.msg;
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    tab.code = 300;
                    tab.msg += ex.Message;
                }
                return tab;
            }
        }
        public TableData MaterialConfirmApp(int id, string containercode, string stockplace)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                TableData tab = new TableData();
                tab.code = 200;
                try
                {
                    MaterialDistributeTaskHeader materialDistributeTaskHeader = _unitWork.Find<MaterialDistributeTaskHeader>(u=>u.Id==id).FirstOrDefault();
                    materialDistributeTaskHeader.StartPlace = stockplace;
                    materialDistributeTaskHeader.ContainerCode = containercode;
                    materialDistributeTaskHeader.MaterialConfirm = MaterialConfirm.已确认;
                    _app.Update(materialDistributeTaskHeader);
                    tab.msg = "备料信息确认成功！";

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    tab.code = 300;
                    tab.msg += ex.Message;
                }
                return tab;
            }
        }
        public TableData UpdateDetailApp(MaterialDistributeTaskDetail materialDistributeTaskDetail)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                TableData tab = new TableData();
                tab.code = 200;
                try
                {
                    _appd.Update(materialDistributeTaskDetail);

                    MaterialDemand materialDemand = _unitWork.Find<MaterialDemand>(u=>u.OrderCode== materialDistributeTaskDetail.OrderCode && u.MaterialCode== materialDistributeTaskDetail.MaterialCode).FirstOrDefault();
                    materialDemand.DistributeQty = materialDistributeTaskDetail.Qty;
                    materialDemand.UpdateBy = "MaterialConfirm";
                    materialDemand.UpdateTime = DateTime.Now;
                    _unitWork.Update(materialDemand);
                    tab.msg = "备料信息变更成功！";

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    tab.code = 300;
                    tab.msg += ex.Message;
                }
                return tab;
            }
        }
    }
}

