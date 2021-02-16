namespace Infrastructure
{
    public class WCSResponse : IResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public WCSResponse()
        {
            Code = 200;
            Message = "操作成功";
            Status = true;
        }
    }

    public class WCSResponse<T> : WCSResponse
    {
        /// <summary>
        /// 回传的结果
        /// </summary>
        public T Result { get; set; }
    }
}
