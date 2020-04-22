using System;
using System.Collections.Generic;
using System.Text;

namespace MyADO
{
	/// <summary>
	/// 数据库管理工厂类
	/// </summary>
	public class DBManagerFactory
	{
		private DBManagerFactory()
		{
		}

		/// <summary>
		/// 得到sqlserver数据库
		/// </summary>
		/// <param name="connString">数据库连接字符串</param>
		/// <returns></returns>
		public static IDBOperator GetSqlDB(string connString)
		{
			return GetDatabase(connString, "sqlserver");
		}
		/// <summary>
		/// 得到mysql数据库
		/// </summary>
		/// <param name="connString">数据库连接字符串</param>
		/// <returns></returns>
		public static IDBOperator GetMySqlDB(string connString)
		{
			return GetDatabase(connString, "mysql");
		}
		/// <summary>
		/// 得到指定的数据库
		/// </summary>
		/// <param name="connString">数据库连接字符串</param>
		/// <param name="providerName">数据库提供者（sqlserver、mysql）</param>
		/// <returns></returns>
		public static IDBOperator GetDatabase(string connString, string providerName = "sqlserver")
		{
			IDBOperator db;
			switch (providerName.ToLower())
			{
				case "sqlserver":
					db = new SqlDBOperator(connString);
					break;
				default:
					db = new SqlDBOperator(connString);
					break;
			}
			return db;
		}

	}
}
