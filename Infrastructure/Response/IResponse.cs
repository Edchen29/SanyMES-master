namespace Infrastructure
{
    /// <summary>
    /// Api响应实体类型
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// 操作状态码，200为正常
        /// </summary>
        int Code { get; set; }
        /// <summary>
        /// 操作消息【当Status不为 200时，显示详细的错误信息】
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// 成功/失败
        /// </summary>
        bool Status { get; set; }
    }
}
