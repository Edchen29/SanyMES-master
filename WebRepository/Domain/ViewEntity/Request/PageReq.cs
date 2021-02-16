namespace WebRepository
{
    public class PageReq
    {
        public int page { get; set; }
        public int limit { get; set; }
        public string field { get; set; }
        public string order { get; set; }

        public PageReq()
        {
            page = 1;
            limit = 1000;
        }
    }
}
