using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MyADO
{
	/// <summary>
	/// 实现SQL Server数据库操作
	/// </summary>
	public class SqlDBOperator : IDBOperator, IDisposable
	{
		#region 属性

		/// <summary>
		/// 数据库连接
		/// </summary>
		private IDbConnection _conn;
		/// <summary>
		/// 获取当前SQL Server连接
		/// </summary>
		public IDbConnection GetConnection
		{
			get
			{
				return _conn;
			}
		}

		/// <summary>
		/// 获取当前是否处于事务处理中，默认值false
		/// </summary>
		private bool isTransaction = false;
		/// <summary>
		/// 获取当前是否处于事务处理中
		/// </summary>
		public bool IsTransaction
		{
			get
			{
				return isTransaction;
			}
		}

		/// <summary>
		/// 事务处理类
		/// </summary>
		private IDbTransaction _trans;

		#endregion

		#region 构造函数
		/// <summary>
		/// 初始化Connection对象
		/// </summary>
		/// <param name="strConnection">数据库连接字符串</param>
		public SqlDBOperator(string strConnection)
		{
			try
			{
				if (_conn == null)
				{
					_conn = new SqlConnection(strConnection);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region 方法

		#region 连接
		/// <summary>
		/// 打开连接
		/// </summary>
		public void Open()
		{
			if (_conn.State != ConnectionState.Open)
			{
				try
				{
					_conn.Open();
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		/// <summary>
		/// 关闭连接
		/// </summary>
		public void Close()
		{
			if (_conn.State != ConnectionState.Closed)
			{
				try
				{
					_conn.Close();
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		/// <summary>
		/// 关闭连接,释放资源
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Close();
			_trans = null;
			_conn = null;
		}
		#endregion

		#region 事务处理
		/// <summary>
		/// 开始一个事务
		/// </summary>
		public void BeginTrans()
		{
			if (!isTransaction)
			{
				_trans = _conn.BeginTransaction();
				isTransaction = true;
			}
		}

		/// <summary>
		/// 提交一个事务
		/// </summary>
		public void CommitTrans()
		{
			try
			{
				if (isTransaction)
				{
					_trans.Commit();
					isTransaction = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 回滚一个事务
		/// </summary>
		public void RollbackTrans()
		{
			_trans.Rollback();
			isTransaction = false;
		}
		#endregion

		#region 生成Command对象
		/// <summary>
		/// 生成Command对象
		/// </summary>
		/// <returns>Command对象</returns>
		public IDbCommand CreateCmd()
		{
			IDbCommand cmd = new SqlCommand();
			return cmd;
		}
		/// <summary>
		/// 生成Command对象
		/// </summary>
		/// <param name="sql">SQL语句或存储过程名</param>
		/// <param name="isProc">是否存储过程</param>
		/// <returns>Command对象</returns>
		private IDbCommand CreateCmd(string sql, bool isProc)
		{
			return CreateCmd(sql, null, null, isProc);
		}
		/// <summary>
		/// 生成Command对象
		/// </summary>
		/// <param name="sql">SQL语句或存储过程名</param>
		/// <param name="parameterArray">参数数组</param>
		/// <param name="isProc">是否存储过程</param>
		/// <returns>Command对象</returns>
		private IDbCommand CreateCmd(string sql, IDataParameter[] parameterArray, bool isProc)
		{
			//BeginTrans();
			IDbCommand cmd = CreateCmd();
			cmd.CommandText = sql;
			cmd.Connection = _conn;
			//cmd.Transaction = _trans;
			if (isProc)
			{
				cmd.CommandType = CommandType.StoredProcedure;
			}
			AttachParameters(cmd, parameterArray);
			return cmd;
		}
		/// <summary>
		/// 生成Command对象
		/// </summary>
		/// <param name="sql">SQL语句或存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <param name="isProc">是否存储过程</param>
		/// <returns>Command对象</returns>
		private IDbCommand CreateCmd(string sql, string[] paraName, object[] paraValue, bool isProc)
		{
			//BeginTrans();
			IDbCommand cmd = CreateCmd();
			cmd.CommandText = sql;
			cmd.Connection = _conn;
			//cmd.Transaction = _trans;
			if (isProc)
			{
				cmd.CommandType = CommandType.StoredProcedure;
			}
			AttachParameters(cmd, paraName, paraValue);
			return cmd;
		}
		/// <summary>
		/// 生成Command对象
		/// </summary>
		/// <param name="sql">SQL语句或存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <param name="isProc">是否存储过程</param>
		/// <returns></returns>
		private IDbCommand CreateCmd(string sql, Dictionary<string, object> dict, bool isProc)
		{
			//BeginTrans();
			IDbCommand cmd = CreateCmd();
			cmd.CommandText = sql;
			cmd.Connection = _conn;
			//cmd.Transaction = _trans;
			if (isProc)
			{
				cmd.CommandType = CommandType.StoredProcedure;
			}
			AttachParameters(cmd, dict);
			return cmd;
		}
		#endregion

		#region 生成DataAdapter对象
		/// <summary>
		/// 生成DataAdapter对象
		/// </summary>
		/// <param name="cmd">Command对象</param>
		/// <returns>DataAdapter对象</returns>
		public IDbDataAdapter CreateDataAdapter(IDbCommand cmd=null)
		{
			IDbDataAdapter dap = new SqlDataAdapter();
			if(cmd != null)
			{
				dap.SelectCommand = cmd;
			}
			return dap;
		}
		#endregion

		#region 运行SQL语句,返回受影响的行数
		/// <summary>
		/// 运行SQL语句,返回受影响的行数
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>受影响的行数</returns>
		public int ExeSQL(string sql)
		{
			return ExeSQL(sql, null, null);
		}
		/// <summary>
		/// 运行带参数的SQL语句,返回受影响的行数
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>受影响的行数</returns>
		public int ExeSQL(string sql, IDataParameter[] parameterArray)
		{
			this.Open();
			int i = -2;
			try
			{
				IDbCommand cmd = CreateCmd(sql, parameterArray, false);
				BeginTrans();
				cmd.Transaction = _trans;
				i = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				this.Close();
			}
			return i;
		}
		/// <summary>
		/// 运行带参数的SQL语句,返回受影响的行数
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>受影响的行数</returns>
		public int ExeSQL(string sql, string[] paraName, object[] paraValue)
		{
			this.Open();
			int i = -2;
			try
			{
				IDbCommand cmd = CreateCmd(sql, paraName, paraValue, false);
				BeginTrans();
				cmd.Transaction = _trans;
				i = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				this.Close();
			}
			return i;
		}
		/// <summary>
		/// 运行带参数的SQL语句,返回受影响的行数
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public int ExeSQL(string sql, Dictionary<string, object> dict)
		{
			this.Open();
			int i = -2;
			try
			{
				IDbCommand cmd = CreateCmd(sql, dict, false);
				BeginTrans();
				cmd.Transaction = _trans;
				i = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				this.Close();
			}
			return i;
		}
		#endregion

		#region 运行存储过程,返回受影响的行数
		/// <summary>
		/// 运行存储过程,返回受影响的行数
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <returns>受影响的行数</returns>
		public int ExeProc(string procName)
		{
			return ExeProc(procName, null, null);
		}
		/// <summary>
		/// 运行带参数的存储过程,返回受影响的行数
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="parameterArray">存储过程参数</param>
		/// <returns>受影响的行数</returns>
		public int ExeProc(string procName, IDataParameter[] parameterArray)
		{
			this.Open();
			int i = -2;
			try
			{
				IDbCommand cmd = CreateCmd(procName, parameterArray, true);
				BeginTrans();
				cmd.Transaction = _trans;
				i = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				this.Close();
			}
			return i;
		}
		/// <summary>
		/// 运行带参数的存储过程,返回受影响的行数
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>受影响的行数</returns>
		public int ExeProc(string procName, string[] paraName, object[] paraValue)
		{
			this.Open();
			int i = -2;
			try
			{
				IDbCommand cmd = CreateCmd(procName, paraName, paraValue, true);
				BeginTrans();
				cmd.Transaction = _trans;
				i = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				this.Close();
			}
			return i;
		}
		/// <summary>
		/// 运行带参数的存储过程,返回受影响的行数
		/// </summary>
		/// <param name="procName">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public int ExeProc(string procName, Dictionary<string, object> dict)
		{
			this.Open();
			int i = -2;
			try
			{
				IDbCommand cmd = CreateCmd(procName, dict, true);
				BeginTrans();
				cmd.Transaction = _trans;
				i = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				this.Close();
			}
			return i;
		}
		#endregion

		#region 运行insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// <summary>
		/// 运行insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="sql">insert,update,delete语句</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		public bool ExeSQLResult(string sql)
		{
			return ExeSQLResult(sql, null, null);
		}
		/// <summary>
		/// 运行带参数的insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="sql">带参数的insert,update,delete语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		public bool ExeSQLResult(string sql, IDataParameter[] parameterArray)
		{
			bool bResult = false;
			int i = ExeSQL(sql, parameterArray);
			if (i >= 1)
			{
				bResult = true;
			}
			return bResult;
		}
		/// <summary>
		/// 运行带参数的insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="sql">带参数的insert,update,delete语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		public bool ExeSQLResult(string sql, string[] paraName, object[] paraValue)
		{
			bool bResult = false;
			int i = ExeSQL(sql, paraName, paraValue);
			if (i >= 1)
			{
				bResult = true;
			}
			return bResult;
		}
		/// <summary>
		/// 运行带参数的insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="sql">带参数的insert,update,delete语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public bool ExeSQLResult(string sql, Dictionary<string, object> dict)
		{
			bool bResult = false;
			int i = ExeSQL(sql, dict);
			if (i >= 1)
			{
				bResult = true;
			}
			return bResult;
		}
		#endregion

		#region 运行insert,update,delete类型的存储过程,返回执行结果。其他的一律返回false
		/// <summary>
		/// 运行insert,update,delete类型的存储过程,返回执行结果。其他的一律返回false
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		public bool ExeProcResult(string procName)
		{
			return ExeProcResult(procName, null, null);
		}
		/// <summary>
		/// 运行带参数的insert,update,delete类型的存储过程,返回执行结果。其他的一律返回false
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="parameterArray">存储过程参数</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		public bool ExeProcResult(string procName, IDataParameter[] parameterArray)
		{
			bool bResult = false;
			int i = ExeProc(procName, parameterArray);
			if (i >= 1)
			{
				bResult = true;
			}
			return bResult;
		}
		/// <summary>
		/// 运行带参数的insert,update,delete类型的存储过程,返回执行结果。其他的一律返回false
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		public bool ExeProcResult(string procName, string[] paraName, object[] paraValue)
		{
			bool bResult = false;
			int i = ExeProc(procName, paraName, paraValue);
			if (i >= 1)
			{
				bResult = true;
			}
			return bResult;
		}
		/// <summary>
		/// 运行带参数的insert,update,delete类型的存储过程,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public bool ExeProcResult(string procName, Dictionary<string, object> dict)
		{
			bool bResult = false;
			int i = ExeProc(procName, dict);
			if (i >= 1)
			{
				bResult = true;
			}
			return bResult;
		}
		#endregion

		#region 执行Insert SQL语句返回当前ID
		/// <summary>
		/// 执行Insert SQL语句返回当前ID
		/// </summary>
		/// <param name="strInsert">Insert SQL语句</param>
		/// <returns>当前ID,失败返回-1</returns>
		public int ReturnID(string strInsert)
		{
			return ReturnID(strInsert, null, null);
		}
		/// <summary>
		/// 执行带参数的Insert SQL语句返回当前ID
		/// </summary>
		/// <param name="strInsert">带参数的Insert SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>当前ID,失败返回-1</returns>
		public int ReturnID(string strInsert, IDataParameter[] parameterArray)
		{
			this.Open();
			int identity = -1;
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(strInsert + "; select SCOPE_IDENTITY() as 'identity'", parameterArray, false);
				BeginTrans();
				cmd.Transaction = _trans;
				// 第一行第一列的值为当前ID
				dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					identity = int.Parse(dr[0].ToString());
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
			}
			return identity;
		}
		/// <summary>
		/// 执行带参数的Insert SQL语句返回当前ID
		/// </summary>
		/// <param name="strInsert">带参数的Insert SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>当前ID,失败返回-1</returns>
		public int ReturnID(string strInsert, string[] paraName, object[] paraValue)
		{
			this.Open();
			int identity = -1;
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(strInsert + "; select SCOPE_IDENTITY() as 'identity'", paraName, paraValue, false);
				BeginTrans();
				cmd.Transaction = _trans;
				// 第一行第一列的值为当前ID
				dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					identity = int.Parse(dr[0].ToString());
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
			}
			return identity;
		}
		/// <summary>
		/// 执行带参数的Insert SQL语句返回当前ID
		/// </summary>
		/// <param name="strInsert">带参数的Insert SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public int ReturnID(string strInsert, Dictionary<string, object> dict)
		{
			this.Open();
			int identity = -1;
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(strInsert + "; select SCOPE_IDENTITY() as 'identity'", dict, false);
				BeginTrans();
				cmd.Transaction = _trans;
				// 第一行第一列的值为当前ID
				dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					identity = int.Parse(dr[0].ToString());
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				RollbackTrans();
				throw ex;
			}
			finally
			{
				if (isTransaction == true)
				{
					CommitTrans();
				}
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
			}
			return identity;
		}
		#endregion

		#region 执行SQL语句返回第一行第一列的值,没有数据返回空值
		/// <summary>
		/// 执行SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>第一行第一列的值</returns>
		public string ExeSQLScalar(string sql)
		{
			return ExeSQLScalar(sql, 0);
		}
		/// <summary>
		/// 执行带参数的SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>第一行第一列的值</returns>
		public string ExeSQLScalar(string sql, IDataParameter[] parameterArray)
		{
			return ExeSQLScalar(sql, 0, parameterArray);
		}
		/// <summary>
		/// 执行带参数的SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>第一行第一列的值</returns>
		public string ExeSQLScalar(string sql, string[] paraName, object[] paraValue)
		{
			return ExeSQLScalar(sql, 0, paraName, paraValue);
		}
		/// <summary>
		/// 执行带参数的SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public string ExeSQLScalar(string sql, Dictionary<string, object> dict)
		{
			return ExeSQLScalar(sql, 0, dict);
		}
		/// <summary>
		/// 返回SQL语句执行结果的第一行第ColumnI列,没有数据返回空值
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="ColumnI">第几列</param>
		/// <returns>第一行第ColumnI列的值</returns>
		public string ExeSQLScalar(string sql, int ColumnI)
		{
			return ExeSQLScalar(sql, ColumnI, null, null);
		}
		/// <summary>
		/// 返回带参数的SQL语句执行结果的第一行第ColumnI列,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="ColumnI">第几列</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>第一行第ColumnI列的值</returns>
		public string ExeSQLScalar(string sql, int ColumnI, IDataParameter[] parameterArray)
		{
			string strValue = "";
			IDataReader dr = GetDataReader(sql, parameterArray);
			try
			{
				if (dr.Read())
				{
					strValue = dr[ColumnI].ToString();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
			return strValue;
		}
		/// <summary>
		/// 返回带参数的SQL语句执行结果的第一行第ColumnI列,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="ColumnI">第几列</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>第一行第ColumnI列的值</returns>
		public string ExeSQLScalar(string sql, int ColumnI, string[] paraName, object[] paraValue)
		{
			string strValue = "";
			IDataReader dr = GetDataReader(sql, paraName, paraValue);
			try
			{
				if (dr.Read())
				{
					strValue = dr[ColumnI].ToString();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
			return strValue;
		}
		/// <summary>
		/// 返回带参数的SQL语句执行结果的第一行第ColumnI列,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="ColumnI">第几列</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public string ExeSQLScalar(string sql, int ColumnI, Dictionary<string, object> dict)
		{
			string strValue = "";
			IDataReader dr = GetDataReader(sql, dict);
			try
			{
				if (dr.Read())
				{
					strValue = dr[ColumnI].ToString();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
			return strValue;
		}
		#endregion

		#region 检验是否存在数据
		/// <summary>
		/// 检验是否存在数据
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>有返回true,没有返回false</returns>
		public bool ExistData(string sql)
		{
			return ExistData(sql, null, null);
		}
		/// <summary>
		/// 检验是否存在数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>有返回true,没有返回false</returns>
		public bool ExistData(string sql, IDataParameter[] parameterArray)
		{
			bool bExist = false;
			IDataReader dr = GetDataReader(sql, parameterArray);
			try
			{
				if (dr.Read())
				{
					bExist = true;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
			return bExist;
		}
		/// <summary>
		/// 检验是否存在数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>有返回true,没有返回false</returns>
		public bool ExistData(string sql, string[] paraName, object[] paraValue)
		{
			bool bExist = false;
			IDataReader dr = GetDataReader(sql, paraName, paraValue);
			try
			{
				if (dr.Read())
				{
					bExist = true;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
			return bExist;
		}
		/// <summary>
		/// 检验是否存在数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public bool ExistData(string sql, Dictionary<string, object> dict)
		{
			bool bExist = false;
			IDataReader dr = GetDataReader(sql, dict);
			try
			{
				if (dr.Read())
				{
					bExist = true;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
			}
			return bExist;
		}
		#endregion

		#region 运行SQL语句,得到DataReader对象
		/// <summary>
		/// 运行SQL语句,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>DataReader对象</returns>
		public NullableDataReader GetDataReader(string sql)
		{
			return GetDataReader(sql, null, null);
		}
		/// <summary>
		/// 运行带参数的SQL语句,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataReader对象</returns>
		public NullableDataReader GetDataReader(string sql, IDataParameter[] parameterArray)
		{
			this.Open();
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(sql, parameterArray, false);
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
				throw ex;
			}
			return new NullableDataReader(dr);
		}
		/// <summary>
		/// 运行带参数的SQL语句,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataReader对象</returns>
		public NullableDataReader GetDataReader(string sql, string[] paraName, object[] paraValue)
		{
			this.Open();
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(sql, paraName, paraValue, false);
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
				throw ex;
			}
			return new NullableDataReader(dr);
		}
		/// <summary>
		/// 运行带参数的SQL语句,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public NullableDataReader GetDataReader(string sql, Dictionary<string, object> dict)
		{
			this.Open();
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(sql, dict, false);
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
				throw ex;
			}
			return new NullableDataReader(dr);
		}
		#endregion

		#region 运行存储过程,得到DataReader对象
		/// <summary>
		/// 运行存储过程,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <returns>DataReader对象</returns>
		public NullableDataReader GetDataReaderByProc(string procName)
		{
			return GetDataReaderByProc(procName, null, null);
		}
		/// <summary>
		/// 运行带参数的存储过程,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataReader对象</returns>
		public NullableDataReader GetDataReaderByProc(string procName, IDataParameter[] parameterArray)
		{
			this.Open();
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(procName, parameterArray, true);
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
				throw ex;
			}
			return new NullableDataReader(dr);
		}
		/// <summary>
		/// 运行带参数的存储过程,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataReader对象</returns>
		public NullableDataReader GetDataReaderByProc(string procName, string[] paraName, object[] paraValue)
		{
			this.Open();
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(procName, paraName, paraValue, true);
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
				throw ex;
			}
			return new NullableDataReader(dr);
		}
		/// <summary>
		/// 运行带参数的存储过程,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public NullableDataReader GetDataReaderByProc(string procName, Dictionary<string, object> dict)
		{
			this.Open();
			IDataReader dr = null;
			try
			{
				IDbCommand cmd = CreateCmd(procName, dict, true);
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}
				this.Close();
				throw ex;
			}
			return new NullableDataReader(dr);
		}
		#endregion

		#region 运行SQL语句,得到DataRow
		/// <summary>
		/// 得到第一行数据
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRow(string sql, Dictionary<string, object> dict)
		{
			return GetDataRow(sql,0, dict);
		}
		/// <summary>
		/// 得到第一行数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRow(string sql, IDataParameter[] parameterArray=null)
		{
			return GetDataRow(sql, 0, parameterArray);
		}
		/// <summary>
		/// 得到第一行数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRow(string sql, string[] paraName, object[] paraValue)
		{
			return GetDataRow(sql, 0, paraName, paraValue);
		}
		/// <summary>
		/// 得到第rowIndex行数据
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRow(string sql, int rowIndex, Dictionary<string, object> dict)
		{
			return GetDataRow(sql, rowIndex, dict);
		}
		/// <summary>
		/// 得到第rowIndex行数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRow(string sql, int rowIndex, IDataParameter[] parameterArray=null)
		{
			DataRow row = null;
			DataTable dt = GetDataTable(sql, parameterArray);
			if (dt.Rows.Count > 0 && dt.Rows.Count > rowIndex)
			{
				row = dt.Rows[rowIndex];
			}
			return row;
		}
		/// <summary>
		/// 得到第rowIndex行数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRow(string sql, int rowIndex, string[] paraName, object[] paraValue)
		{
			DataRow row = null;
			DataTable dt = GetDataTable(sql, paraName, paraValue);
			if (dt.Rows.Count > 0 && dt.Rows.Count > rowIndex)
			{
				row = dt.Rows[rowIndex];
			}
			return row;
		}
		#endregion

		#region 运行存储过程,得到DataRow
		/// <summary>
		/// 运行存储过程,得到第一行数据
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRowByProc(string procName, Dictionary<string, object> dict)
		{
			return GetDataRowByProc(procName, 0, dict);
		}
		/// <summary>
		/// 运行存储过程,得到第一行数据
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRowByProc(string procName, IDataParameter[] parameterArray=null)
		{
			return GetDataRowByProc(procName, 0, parameterArray);
		}
		/// <summary>
		/// 运行存储过程,得到第一行数据
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRowByProc(string procName, string[] paraName, object[] paraValue)
		{
			return GetDataRowByProc(procName, 0, paraName, paraValue);
		}
		/// <summary>
		/// 运行存储过程,得到第rowIndex行数据
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRowByProc(string procName, int rowIndex, Dictionary<string, object> dict)
		{
			return GetDataRowByProc(procName, rowIndex, dict);
		}
		/// <summary>
		/// 运行存储过程,得到第rowIndex行数据
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRowByProc(string procName, int rowIndex, IDataParameter[] parameterArray=null)
		{
			DataRow row = null;
			DataTable dt = GetDataTableByProc(procName, parameterArray);
			if (dt.Rows.Count > 0 && dt.Rows.Count > rowIndex)
			{
				row = dt.Rows[rowIndex];
			}
			return row;
		}
		/// <summary>
		/// 运行存储过程,得到第rowIndex行数据
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataRow对象</returns>
		public DataRow GetDataRowByProc(string procName, int rowIndex, string[] paraName, object[] paraValue)
		{
			DataRow row = null;
			DataTable dt = GetDataTableByProc(procName, paraName, paraValue);
			if (dt.Rows.Count > 0 && dt.Rows.Count > rowIndex)
			{
				row = dt.Rows[rowIndex];
			}
			return row;
		}
		#endregion

		#region 运行SQL语句,得到DataTable
		/// <summary>
		/// 得到DataTable
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataTable对象</returns>
		public DataTable GetDataTable(string sql, IDataParameter[] parameterArray)
		{
			DataSet ds = GetDataSet(sql, parameterArray);
			return ds.Tables[0];
		}
		/// <summary>
		/// 得到DataTable
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataTable对象</returns>
		public DataTable GetDataTable(string sql, string[] paraName, object[] paraValue)
		{
			DataSet ds = GetDataSet(sql, paraName, paraValue);
			return ds.Tables[0];
		}
		/// <summary>
		/// 得到DataTable
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataTable对象</returns>
		public DataTable GetDataTable(string sql, Dictionary<string, object> dict)
		{
			DataSet ds = GetDataSet(sql, dict);
			return ds.Tables[0];
		}
		#endregion

		#region 运行存储过程,获取DataTable
		/// <summary>
		/// 运行存储过程,获取DataTable
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataTable对象</returns>
		public DataTable GetDataTableByProc(string procName, IDataParameter[] parameterArray=null)
		{
			DataSet ds = GetDataSetByProc(procName, parameterArray);
			return ds.Tables[0];
		}
		/// <summary>
		/// 运行存储过程,获取DataTable
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataTable对象</returns>
		public DataTable GetDataTableByProc(string procName, string[] paraName, object[] paraValue)
		{
			DataSet ds = GetDataSetByProc(procName, paraName, paraValue);
			return ds.Tables[0];
		}
		/// <summary>
		/// 运行存储过程,获取DataTable
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataTable对象</returns>
		public DataTable GetDataTableByProc(string procName, Dictionary<string, object> dict)
		{
			DataSet ds = GetDataSetByProc(procName, dict);
			return ds.Tables[0];
		}
		#endregion

		#region 运行SQL语句,得到DataSet
		/// <summary>
		/// 得到DataSet
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataSet对象</returns>
		public DataSet GetDataSet(string sql, IDataParameter[] parameterArray=null)
		{
			this.Open();
			DataSet ds = new DataSet();
			try
			{
				IDbDataAdapter dap = CreateDataAdapter(CreateCmd(sql, parameterArray, false));
				dap.Fill(ds);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.Close();
			}
			return ds;
		}
		/// <summary>
		/// 得到DataSet
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataSet对象</returns>
		public DataSet GetDataSet(string sql, string[] paraName, object[] paraValue)
		{
			this.Open();
			DataSet ds = new DataSet();
			try
			{
				IDbDataAdapter dap = CreateDataAdapter(CreateCmd(sql, paraName, paraValue, false));
				dap.Fill(ds);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.Close();
			}
			return ds;
		}
		/// <summary>
		/// 得到DataSet
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataSet对象</returns>
		public DataSet GetDataSet(string sql, Dictionary<string, object> dict)
		{
			this.Open();
			DataSet ds = new DataSet();
			try
			{
				IDbDataAdapter dap = CreateDataAdapter(CreateCmd(sql, dict, false));
				dap.Fill(ds);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.Close();
			}
			return ds;
		}
		#endregion

		#region 运行存储过程,得到DataSet对象
		/// <summary>
		/// 运行带参数的存储过程,得到DataSet对象
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="parameterArray">存储过程参数</param>
		/// <returns>DataSet对象</returns>
		public DataSet GetDataSetByProc(string procName, IDataParameter[] parameterArray=null)
		{
			this.Open();
			DataSet ds = new DataSet();
			try
			{
				IDbDataAdapter dap = CreateDataAdapter(CreateCmd(procName, parameterArray, true));
				dap.Fill(ds);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.Close();
			}
			return ds;
		}
		/// <summary>
		/// 运行带参数的存储过程,得到DataSet对象
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataSet对象</returns>
		public DataSet GetDataSetByProc(string procName, string[] paraName, object[] paraValue)
		{
			this.Open();
			DataSet ds = new DataSet();
			try
			{
				IDbDataAdapter dap = CreateDataAdapter(CreateCmd(procName, paraName, paraValue, true));
				dap.Fill(ds);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.Close();
			}
			return ds;
		}
		/// <summary>
		/// 运行带参数的存储过程,得到DataSet对象
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public DataSet GetDataSetByProc(string procName, Dictionary<string, object> dict)
		{
			this.Open();
			DataSet ds = new DataSet();
			try
			{
				IDbDataAdapter dap = CreateDataAdapter(CreateCmd(procName, dict, true));
				dap.Fill(ds);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.Close();
			}
			return ds;
		}
		#endregion

		#region 得到最大ID
		/// <summary>
		/// 得到指定表中的、指定字段的最大数值
		/// </summary>
		/// <param name="tableName">数据表名</param>
		/// <param name="fieldName">字段名</param>
		/// <returns></returns>
		public int GetMaxID(string tableName, string fieldName)
		{
			return GetMaxID(tableName, fieldName, string.Empty);
		}
		/// <summary>
		/// 得到指定表中的、指定字段的最大数值
		/// </summary>
		/// <param name="tableName">数据表名</param>
		/// <param name="fieldName">字段名</param>
		/// <param name="whereSQL">参数化查询条件(例如: and Name = @Name )</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		public int GetMaxID(string tableName, string fieldName, string whereSQL, Dictionary<string, object> dict = null)
		{
			string strSQL = "select MAX(" + fieldName + ") from " + tableName + " where 1=1 ";
			if (!string.IsNullOrEmpty(whereSQL))
			{
				strSQL += whereSQL;
			}
			string result = ExeSQLScalar(strSQL, dict);
			return ObjectToInt32(result);
		}
		#endregion

		#endregion

		#region 私有方法
		/// <summary>
		/// 配属参数到IDbCommand对象
		/// </summary>
		/// <param name="command">IDbCommand对象</param>
		/// <param name="parameterArray">参数数组</param>
		private void AttachParameters(IDbCommand command, IDataParameter[] parameterArray)
		{
			if (parameterArray != null)
			{
				foreach (IDataParameter parameter in parameterArray)
				{
					if (parameter.Direction == ParameterDirection.InputOutput && parameter.Value == null)
					{
						parameter.Value = DBNull.Value;
					}
					command.Parameters.Add(parameter);
				}
			}
		}
		/// <summary>
		/// 配属参数到IDbCommand对象
		/// </summary>
		/// <param name="command">IDbCommand对象</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		private void AttachParameters(IDbCommand command, string[] paraName, object[] paraValue)
		{
			if (paraName == null || paraValue == null)
			{
				return;
			}
			IDataParameter[] parameterArray = new IDataParameter[paraName.Length];
			for (int j = 0; j < paraName.Length; j++)
			{
				parameterArray[j] = new SqlParameter()
				{
					ParameterName = paraName[j].ToString(),
					Value = paraValue[j]
				};
			}
			if (parameterArray != null)
			{
				foreach (IDataParameter parameter in parameterArray)
				{
					if (parameter.Direction == ParameterDirection.InputOutput && parameter.Value == null)
					{
						parameter.Value = DBNull.Value;
					}
					command.Parameters.Add(parameter);
				}
			}
		}
		/// <summary>
		/// 配属参数到IDbCommand对象
		/// </summary>
		/// <param name="command">IDbCommand对象</param>
		/// <param name="dict">参数的名/值集合</param>
		private void AttachParameters(IDbCommand command, Dictionary<string, object> dict)
		{
			if (dict == null)
			{
				return;
			}
			foreach (KeyValuePair<string, object> kvp in dict)
			{
				SqlParameter sqlPara = new SqlParameter(kvp.Key, kvp.Value);
				if (sqlPara.Value == null)
				{
					sqlPara.Value = DBNull.Value;
				}
				command.Parameters.Add(sqlPara);
			}
		}
		/// <summary>
		/// 转换对象类型为INT类型，失败返回0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private int ObjectToInt32(object value)
		{
			int result = 0;
			if ((!object.Equals(value, null) && !object.Equals(value, DBNull.Value)) && !int.TryParse(value.ToString(), out result))
			{
				result = 0;
			}
			return result;
		}
		#endregion
	}
}
