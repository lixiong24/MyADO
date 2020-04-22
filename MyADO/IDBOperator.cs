using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyADO
{
	/// <summary>
	/// 通用数据库操作接口
	/// </summary>
	public interface IDBOperator
	{
		#region 属性
		/// <summary>
		/// 得到数据库连接
		/// </summary>
		IDbConnection GetConnection
		{
			get;
		}

		/// <summary>
		/// 获取当前是否处于事务处理中
		/// </summary>
		bool IsTransaction
		{
			get;
		}
		#endregion

		#region 方法

		#region 连接
		/// <summary>
		/// 打开数据库连接
		/// </summary>
		void Open();

		/// <summary>
		/// 关闭连接
		/// </summary>
		void Close();

		/// <summary>
		/// 关闭连接,释放资源
		/// </summary>
		void Dispose();
		#endregion

		#region 事务处理
		/// <summary>
		/// 开始一个事务
		/// </summary>
		void BeginTrans();

		/// <summary>
		/// 提交一个事务
		/// </summary>
		void CommitTrans();

		/// <summary>
		/// 回滚一个事务
		/// </summary>
		void RollbackTrans();
		#endregion

		#region 生成Command对象
		/// <summary>
		/// 生成Command对象
		/// </summary>
		/// <returns>Command对象</returns>
		IDbCommand CreateCmd();
		#endregion

		#region 生成DataAdapter对象
		/// <summary>
		/// 生成DataAdapter对象
		/// </summary>
		/// <param name="cmd">Command对象</param>
		/// <returns></returns>
		IDbDataAdapter CreateDataAdapter(IDbCommand cmd=null);
		#endregion

		#region 运行SQL语句,返回受影响的行数
		/// <summary>
		/// 运行SQL语句,返回受影响的行数
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>受影响的行数</returns>
		int ExeSQL(string sql);
		/// <summary>
		/// 运行带参数的SQL语句,返回受影响的行数
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>受影响的行数</returns>
		int ExeSQL(string sql, IDataParameter[] parameterArray);
		/// <summary>
		/// 运行带参数的SQL语句,返回受影响的行数
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>受影响的行数</returns>
		int ExeSQL(string sql, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行带参数的SQL语句,返回受影响的行数
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		int ExeSQL(string sql, Dictionary<string, object> dict);
		#endregion

		#region 运行存储过程,返回受影响的行数
		/// <summary>
		/// 运行存储过程,返回受影响的行数
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <returns>受影响的行数</returns>
		int ExeProc(string procName);
		/// <summary>
		/// 运行带参数的存储过程,返回受影响的行数
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="parameterArray">存储过程参数</param>
		/// <returns>受影响的行数</returns>
		int ExeProc(string procName, IDataParameter[] parameterArray);
		/// <summary>
		/// 运行带参数的存储过程,返回受影响的行数
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>受影响的行数</returns>
		int ExeProc(string procName, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行带参数的存储过程,返回受影响的行数
		/// </summary>
		/// <param name="procName">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		int ExeProc(string procName, Dictionary<string, object> dict);
		#endregion

		#region 运行insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// <summary>
		/// 运行insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="sql">insert,update,delete语句</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		bool ExeSQLResult(string sql);
		/// <summary>
		/// 运行带参数的insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="sql">带参数的insert,update,delete语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		bool ExeSQLResult(string sql, IDataParameter[] parameterArray);
		/// <summary>
		/// 运行带参数的insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="sql">带参数的insert,update,delete语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		bool ExeSQLResult(string sql, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行带参数的insert,update,delete语句,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="sql">带参数的insert,update,delete语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		bool ExeSQLResult(string sql, Dictionary<string, object> dict);
		#endregion

		#region 运行insert,update,delete类型的存储过程,返回执行结果。其他的一律返回false
		/// <summary>
		/// 运行insert,update,delete类型的存储过程,返回执行结果。其他的一律返回false
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		bool ExeProcResult(string procName);
		/// <summary>
		/// 运行带参数的insert,update,delete类型的存储过程,返回执行结果。其他的一律返回false
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="parameterArray">存储过程参数</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		bool ExeProcResult(string procName, IDataParameter[] parameterArray);
		/// <summary>
		/// 运行带参数的insert,update,delete类型的存储过程,返回执行结果。其他的一律返回false
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>返回执行结果,成功为true,失败为false</returns>
		bool ExeProcResult(string procName, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行带参数的insert,update,delete类型的存储过程,返回执行结果。其他的SQL语句一律返回false
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		bool ExeProcResult(string procName, Dictionary<string, object> dict);
		#endregion

		#region 执行Insert SQL语句返回当前ID
		/// <summary>
		/// 执行Insert SQL语句返回当前ID
		/// </summary>
		/// <param name="strInsert">Insert SQL语句</param>
		/// <returns>当前ID,失败返回-1</returns>
		int ReturnID(string strInsert);
		/// <summary>
		/// 执行带参数的Insert SQL语句返回当前ID
		/// </summary>
		/// <param name="strInsert">带参数的Insert SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>当前ID,失败返回-1</returns>
		int ReturnID(string strInsert, IDataParameter[] parameterArray);
		/// <summary>
		/// 执行带参数的Insert SQL语句返回当前ID
		/// </summary>
		/// <param name="strInsert">带参数的Insert SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>当前ID,失败返回-1</returns>
		int ReturnID(string strInsert, string[] paraName, object[] paraValue);
		/// <summary>
		/// 执行带参数的Insert SQL语句返回当前ID
		/// </summary>
		/// <param name="strInsert">带参数的Insert SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		int ReturnID(string strInsert, Dictionary<string, object> dict);
		#endregion

		#region 执行SQL语句返回第一行第一列的值,没有数据返回空值
		/// <summary>
		/// 执行SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>第一行第一列的值</returns>
		string ExeSQLScalar(string sql);
		/// <summary>
		/// 执行带参数的SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>第一行第一列的值</returns>
		string ExeSQLScalar(string sql, IDataParameter[] parameterArray);
		/// <summary>
		/// 执行带参数的SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>第一行第一列的值</returns>
		string ExeSQLScalar(string sql, string[] paraName, object[] paraValue);
		/// <summary>
		/// 执行带参数的SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		string ExeSQLScalar(string sql, Dictionary<string, object> dict);
		/// <summary>
		/// 返回SQL语句执行结果的第一行第ColumnI列,没有数据返回空值
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="ColumnI">第几列</param>
		/// <returns>第一行第ColumnI列的值</returns>
		string ExeSQLScalar(string sql, int ColumnI);
		/// <summary>
		/// 返回带参数的SQL语句执行结果的第一行第ColumnI列,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="ColumnI">第几列</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>第一行第ColumnI列的值</returns>
		string ExeSQLScalar(string sql, int ColumnI, IDataParameter[] parameterArray);
		/// <summary>
		/// 返回带参数的SQL语句执行结果的第一行第ColumnI列,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="ColumnI">第几列</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>第一行第ColumnI列的值</returns>
		string ExeSQLScalar(string sql, int ColumnI, string[] paraName, object[] paraValue);
		/// <summary>
		/// 执行带参数的SQL语句返回第一行第一列的值,没有数据返回空值
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="ColumnI">第几列</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		string ExeSQLScalar(string sql, int ColumnI, Dictionary<string, object> dict);
		#endregion

		#region 检验是否存在数据
		/// <summary>
		/// 检验是否存在数据
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>有返回true,没有返回false</returns>
		bool ExistData(string sql);
		/// <summary>
		/// 检验是否存在数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>有返回true,没有返回false</returns>
		bool ExistData(string sql, IDataParameter[] parameterArray);
		/// <summary>
		/// 检验是否存在数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>有返回true,没有返回false</returns>
		bool ExistData(string sql, string[] paraName, object[] paraValue);
		/// <summary>
		/// 检验是否存在数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		bool ExistData(string sql, Dictionary<string, object> dict);
		#endregion

		#region 运行SQL语句,得到DataReader对象
		/// <summary>
		/// 运行SQL语句,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>DataReader对象</returns>
		NullableDataReader GetDataReader(string sql);
		/// <summary>
		/// 运行带参数的SQL语句,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataReader对象</returns>
		NullableDataReader GetDataReader(string sql, IDataParameter[] parameterArray);
		/// <summary>
		/// 运行带参数的SQL语句,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataReader对象</returns>
		NullableDataReader GetDataReader(string sql, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行带参数的SQL语句,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		NullableDataReader GetDataReader(string sql, Dictionary<string, object> dict);
		#endregion

		#region 运行存储过程,得到DataReader对象
		/// <summary>
		/// 运行存储过程,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <returns>DataReader对象</returns>
		NullableDataReader GetDataReaderByProc(string procName);
		/// <summary>
		/// 运行带参数的存储过程,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataReader对象</returns>
		NullableDataReader GetDataReaderByProc(string procName, IDataParameter[] parameterArray);
		/// <summary>
		/// 运行带参数的存储过程,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataReader对象</returns>
		NullableDataReader GetDataReaderByProc(string procName, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行带参数的存储过程,得到DataReader对象,使用完后显示调用DataReader的Close()方法.
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		NullableDataReader GetDataReaderByProc(string procName, Dictionary<string, object> dict);
		#endregion

		#region 运行SQL语句,得到DataRow
		/// <summary>
		/// 得到第一行数据
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRow(string sql, Dictionary<string, object> dict);
		/// <summary>
		/// 得到第一行数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRow(string sql, IDataParameter[] parameterArray = null);
		/// <summary>
		/// 得到第一行数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRow(string sql, string[] paraName, object[] paraValue);
		/// <summary>
		/// 得到第rowIndex行数据
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRow(string sql, int rowIndex, Dictionary<string, object> dict);
		/// <summary>
		/// 得到第rowIndex行数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRow(string sql, int rowIndex, IDataParameter[] parameterArray = null);
		/// <summary>
		/// 得到第rowIndex行数据
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRow(string sql, int rowIndex, string[] paraName, object[] paraValue);
		#endregion

		#region 运行存储过程,得到DataRow
		/// <summary>
		/// 运行存储过程,得到第一行数据
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRowByProc(string procName, Dictionary<string, object> dict);
		/// <summary>
		/// 运行存储过程,得到第一行数据
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRowByProc(string procName, IDataParameter[] parameterArray=null);
		/// <summary>
		/// 运行存储过程,得到第一行数据
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRowByProc(string procName, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行存储过程,得到第rowIndex行数据
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRowByProc(string procName, int rowIndex, Dictionary<string, object> dict);
		/// <summary>
		/// 运行存储过程,得到第rowIndex行数据
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRowByProc(string procName, int rowIndex, IDataParameter[] parameterArray = null);
		/// <summary>
		/// 运行存储过程,得到第rowIndex行数据
		/// </summary>
		/// <param name="procName">带参数的存储过程名</param>
		/// <param name="rowIndex">行索引</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataRow对象</returns>
		DataRow GetDataRowByProc(string procName, int rowIndex, string[] paraName, object[] paraValue);
		#endregion

		#region 运行SQL语句,得到DataTable
		/// <summary>
		/// 得到DataTable
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataTable对象</returns>
		DataTable GetDataTable(string sql, IDataParameter[] parameterArray=null);
		/// <summary>
		/// 得到DataTable
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataTable对象</returns>
		DataTable GetDataTable(string sql, string[] paraName, object[] paraValue);
		/// <summary>
		/// 得到DataTable
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataTable对象</returns>
		DataTable GetDataTable(string sql, Dictionary<string, object> dict);
		#endregion

		#region 运行存储过程,获取DataTable
		/// <summary>
		/// 运行存储过程,获取DataTable
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataTable对象</returns>
		DataTable GetDataTableByProc(string procName, IDataParameter[] parameterArray = null);
		/// <summary>
		/// 运行存储过程,获取DataTable
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataTable对象</returns>
		DataTable GetDataTableByProc(string procName, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行存储过程,获取DataTable
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataTable对象</returns>
		DataTable GetDataTableByProc(string procName, Dictionary<string, object> dict);
		#endregion

		#region 运行SQL语句,得到DataSet
		/// <summary>
		/// 得到DataSet
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="parameterArray">参数数组</param>
		/// <returns>DataSet对象</returns>
		DataSet GetDataSet(string sql, IDataParameter[] parameterArray=null);
		/// <summary>
		/// 得到DataSet
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataSet对象</returns>
		DataSet GetDataSet(string sql, string[] paraName, object[] paraValue);
		/// <summary>
		/// 得到DataSet
		/// </summary>
		/// <param name="sql">带参数的SQL语句</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns>DataSet对象</returns>
		DataSet GetDataSet(string sql, Dictionary<string, object> dict);
		#endregion

		#region 运行存储过程,得到DataSet对象
		/// <summary>
		/// 运行带参数的存储过程,得到DataSet对象
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="parameterArray">存储过程参数</param>
		/// <returns>DataSet对象</returns>
		DataSet GetDataSetByProc(string procName, IDataParameter[] parameterArray=null);
		/// <summary>
		/// 运行带参数的存储过程,得到DataSet对象
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="paraName">参数名数组</param>
		/// <param name="paraValue">参数值数组</param>
		/// <returns>DataSet对象</returns>
		DataSet GetDataSetByProc(string procName, string[] paraName, object[] paraValue);
		/// <summary>
		/// 运行带参数的存储过程,得到DataSet对象
		/// </summary>
		/// <param name="procName">存储过程名</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		DataSet GetDataSetByProc(string procName, Dictionary<string, object> dict);
		#endregion

		#region 得到最大ID
		/// <summary>
		/// 得到指定表中的、指定字段的最大数值
		/// </summary>
		/// <param name="tableName">数据表名</param>
		/// <param name="fieldName">字段名</param>
		/// <returns></returns>
		int GetMaxID(string tableName, string fieldName);
		/// <summary>
		/// 得到指定表中的、指定字段的最大数值
		/// </summary>
		/// <param name="tableName">数据表名</param>
		/// <param name="fieldName">字段名</param>
		/// <param name="whereSQL">参数化查询条件(例如: and Name = @Name )</param>
		/// <param name="dict">参数的名/值集合</param>
		/// <returns></returns>
		int GetMaxID(string tableName, string fieldName, string whereSQL, Dictionary<string, object> dict = null);
		#endregion

		#endregion
	}
}
