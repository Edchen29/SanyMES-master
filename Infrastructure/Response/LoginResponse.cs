namespace Infrastructure
{
    public class LoginResponse : IResponse
    {
        public int Code { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public LoginResponse()
        {
            Code = 200;
            Message = "操作成功";
            Status = true;
        }
    }

    public class LoginResponse<T> : LoginResponse
    {
        /// <summary>
        /// 回传的结果
        /// </summary>
        public T Result { get; set; }
    }
}
