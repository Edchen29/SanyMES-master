namespace Infrastructure
{
    public class Response : IResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public Response()
        {
            Code = 200;
            Message = "操作成功";
            Status = true;
        }
    }

    public class Response<T> : Response
    {
        /// <summary>
        /// 回传的结果
        /// </summary>
        public T Result { get; set; }
    }
}
