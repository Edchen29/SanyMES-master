namespace Infrastructure
{
    public class ICSResponse : IResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public ICSResponse()
        {
            Code = 200;
            Message = "操作成功";
            Status = true;
        }
    }

    public class ICSResponse<T> : ICSResponse
    {
        /// <summary>
        /// 回传的结果
        /// </summary>
        public T Result { get; set; }
    }
}
