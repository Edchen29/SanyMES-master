using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;

namespace WebRepository
{
    public partial class SqlWork : ISqlWork
    {
        private BaseDBContext _context;

        private string CnString;

        public SqlWork(BaseDBContext context)
        {
            _context = context;
        }

        public BaseDBContext GetDbContext()
        {
            return _context;
        }

        /// <summary>
        /// 数据操作
        /// </summary>
        public int DataOperator(string Cmstring)
        {
            CnString = _context.Database.GetDbConnection().ConnectionString;

            int Line = 0;
            SqlCommand sqlCm = null;
            SqlConnection sqlCn = null;

            try
            {
                using (sqlCn = new SqlConnection(CnString))
                {
                    sqlCn.Open();
                    sqlCm = new SqlCommand(Cmstring, sqlCn);
                    Line = sqlCm.ExecuteNonQuery();
                    sqlCn.Close();
                }
            }
            catch (Exception ex)
            {
                //SEmessage = ex.Message.Replace(Environment.NewLine, "");
                Line = -1;
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));

            }
            finally
            {
                sqlCn.Dispose();
                if (sqlCm != null)
                    sqlCm.Dispose();
            }
            return Line;
        }

        /// <summary>
        /// 查询信息
        /// </summary>
        public DataSet SelectGet(string CmString)
        {
            CnString = _context.Database.GetDbConnection().ConnectionString;

            DataSet ds = new System.Data.DataSet();
            SqlConnection sqlCn = null;
            SqlDataAdapter sqlDa = null;
            try
            {
                using (sqlCn = new SqlConnection(CnString))
                {
                    sqlCn.Open();
                    sqlDa = new SqlDataAdapter(CmString, sqlCn);
                    sqlDa.Fill(ds);
                    sqlCn.Close();
                }
            }
            catch (Exception ex)
            {
                ds = null;
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
                //App.ExFile.MessageLog(App.ExFile.Path_Emessage, "", ex.ToString());

            }
            finally
            {
                sqlCn.Dispose();
                if (sqlDa != null)
                    sqlDa.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数 
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public int RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            CnString = _context.Database.GetDbConnection().ConnectionString;

            using (SqlConnection connection = new SqlConnection(CnString))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                int rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        public int ProcOutput(SqlCommand cmd, string procName, SqlParameter[] paras)
        {

            int result;
            cmd.CommandType = CommandType.StoredProcedure;//设置此类型是存储过程类型
            cmd.CommandText = procName;//设置执行的存储过程//添加para参数
            if (paras != null)
            {
                foreach (SqlParameter parameter in paras)
                    cmd.Parameters.Add(parameter);
            }
            cmd.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            int n = cmd.ExecuteNonQuery();//执行语句并返回受影响的行数
            result = (int)cmd.Parameters["ReturnValue"].Value;
            return result;
        }

        public int ProcOutput(string procName, SqlParameter[] paras)
        {
            CnString = _context.Database.GetDbConnection().ConnectionString;

            SqlConnection conn = new SqlConnection(CnString);//创建数据库连接对象
            conn.Open();//打开数据库连接

            //SqlCommand cmd = conn.CreateCommand();//创建并执行与此连接关联的T-Sql语句命令的对象，可以不用这段用下面两段
            SqlCommand cmd = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.StoredProcedure,//设置此类型是存储过程类型
                CommandText = procName//设置执行的存储过程//添加para参数
            };
            int result;
            if (paras != null)
            {
                foreach (SqlParameter parameter in paras)
                    cmd.Parameters.Add(parameter);
            }
            cmd.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            int n = cmd.ExecuteNonQuery();//执行语句并返回受影响的行数
            result = (int)cmd.Parameters["ReturnValue"].Value;
            conn.Close();//关闭连接
            return result;
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值) 
        /// </summary>
        /// <param name="connection">SQl连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        /// <summary>
        /// 调用存储过程Output参数方法
        /// </summary>
        /// <param name="procName">执行的存储过程语句</param>
        /// <param name="paras">数据库的参数</param>
        /// <param name="OutName">定义Output参数</param>
        /// <returns>返回值</returns>
        public object ProcOutput(string procName, SqlParameter[] paras, string OutName)
        {
            CnString = _context.Database.GetDbConnection().ConnectionString;

            SqlConnection conn = new SqlConnection(CnString);//创建数据库连接对象
            conn.Open();//打开数据库连接

            //SqlCommand cmd = conn.CreateCommand();//创建并执行与此连接关联的T-Sql语句命令的对象，可以不用这段用下面两段
            SqlCommand cmd = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.StoredProcedure,//设置此类型是存储过程类型
                CommandText = procName//设置执行的存储过程//添加para参数
            };
            if (paras != null)
            {
                foreach (SqlParameter parameter in paras)
                    cmd.Parameters.Add(parameter);
            }
            int n = cmd.ExecuteNonQuery();//执行语句并返回受影响的行数
            object o = cmd.Parameters[OutName].Value;//获取@CardID的值
            conn.Close();//关闭连接
            return o;
        }
    }
}
