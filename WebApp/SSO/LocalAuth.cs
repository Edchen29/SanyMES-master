using Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using WebRepository;

namespace WebApp
{
    /// <summary>
    /// ʹ�ñ��ص�¼�����ע��IAuthʱ��ֻ��ҪMvcһ����Ŀ���ɣ�����webapi��֧��
    /// </summary>
    public class LocalAuth : IAuth
    {
        private IHttpContextAccessor _httpContextAccessor;

        private AuthContextFactory _app;
        private LoginParse _loginParse;
        private ICacheContext _cacheContext;
        private IUnitWork _unitWork;

        public LocalAuth(IHttpContextAccessor httpContextAccessor
            , AuthContextFactory app
            , LoginParse loginParse
            , ICacheContext cacheContext
            , IUnitWork unitWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _app = app;
            _loginParse = loginParse;
            _cacheContext = cacheContext;
            _unitWork = unitWork;
        }

        private string GetToken()
        {
            string token = _httpContextAccessor.HttpContext.Request.Query["Token"];
            if (!String.IsNullOrEmpty(token)) return token;

            var cookie = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            return cookie ?? String.Empty;
        }

        public bool CheckLogin(string token = "")
        {
            if (string.IsNullOrEmpty(token))
            {
                token = GetToken();
            }

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var result = _cacheContext.Get<UserAuthSession>(token) != null;

                try
                {
                    if (result)
                    {
                        DateTime dateTime = DateTime.Now;
                        _unitWork.Update<SysUserOnline>(u => u.Token.Equals(token), u => new SysUserOnline { LastAccessTime = dateTime });
                    }
                    else
                    {
                        _unitWork.Delete<SysUserOnline>(u => u.Token.Equals(token));
                    }
                }
                catch (Exception)
                {
                }
  
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ��¼���û���Ϣ
        /// <para>ͨ��URL�е�Token������Cookie�е�Token</para>
        /// </summary>
        /// <returns>LoginUserVM.</returns>
        public AuthStrategyContext GetCurrentUser()
        {
            AuthStrategyContext context = null;
            var user = _cacheContext.Get<UserAuthSession>(GetToken());
            if (user != null)
            {
                context = _app.GetAuthStrategyContext(user.Account);
            }
            return context;
        }

        /// <summary>
        /// ��ȡ��ǰ��¼���˻����û���
        /// <para>ͨ��URL�е�Token������Cookie�е�Token</para>
        /// </summary>
        /// <returns>System.String.</returns>
        public List<string> GetUserAccountName()
        {
            var user = _cacheContext.Get<UserAuthSession>(GetToken());
            if (user != null)
            {
                return new List<string> { user.Account, user.Name };
            }

            return null;
        }

        /// <summary>
        /// ��ȡ��ǰ��¼���˻����û���
        /// <para>ͨ��URL�е�Token������Cookie�е�Token</para>
        /// </summary>
        /// <returns>System.String.</returns>
        public List<string> GetUserAccountName(string username)
        {
            var user = _app.GetAuthStrategyContext(username).User;
            if (user != null)
            {
                return new List<string> { user.Account, user.Name };
            }

            return null;
        }

        /// <summary>
        /// ��¼�ӿ�
        /// </summary>
        /// <param name="appKey">Ӧ�ó���key.</param>
        /// <param name="username">�û���</param>
        /// <param name="pwd">����</param>
        /// <returns>System.String.</returns>
        public LoginResult Login(string appKey, string username, string pwd)
        {
            return _loginParse.Do(new PassportLoginRequest
            {
                AppKey = appKey,
                Account = username,
                Password = pwd,
            });
        }

        /// <summary>
        /// ע��
        /// </summary>
        public bool Logout()
        {
            var token = GetToken();
            if (String.IsNullOrEmpty(token)) return true;

            try
            {
                _unitWork.Delete<SysUserOnline>(u => u.Token.Equals(token));
            }
            catch (Exception)
            {
            }

            try
            {
                _cacheContext.Remove(token);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}