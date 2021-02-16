namespace Infrastructure
{
    public class PadResponse : IResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public PadResponse()
        {
            Code = 200;
            Message = "操作成功";
            Status = true;
        }
        public dynamic Result;
    }
}
