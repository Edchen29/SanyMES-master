using Infrastructure;

namespace WebApp
{
    public class LoginResult :Response<string>
    {
        public string ReturnUrl;
        public string Token;
    }
}