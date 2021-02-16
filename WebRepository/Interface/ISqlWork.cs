using System.Data;
using System.Data.SqlClient;

namespace WebRepository
{
    /// <summary>
    /// Sql语句操作接口
    /// </summary>
    public partial interface ISqlWork
    {
        BaseDBContext GetDbContext();

        /// <summary>
        /// 数据操作
        /// </summary>
        int DataOperator(string Cmstring);

        /// <summary>
        /// 查询信息
        /// </summary>
        DataSet SelectGet(string CmString);

        /// <summary>
        /// 执行存储过程，返回影响的行数 
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        int RunProcedure(string storedProcName, IDataParameter[] parameters);

        int ProcOutput(SqlCommand cmd, string procName, SqlParameter[] paras);

        int ProcOutput(string procName, SqlParameter[] paras);

        /// <summary>
        /// 调用存储过程Output参数方法
        /// </summary>
        /// <param name="procName">执行的存储过程语句</param>
        /// <param name="paras">数据库的参数</param>
        /// <param name="OutName">定义Output参数</param>
        /// <returns>返回值</returns>
        object ProcOutput(string procName, SqlParameter[] paras, string OutName);
    }
}
