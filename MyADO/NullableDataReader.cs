using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyADO
{
	/// <summary>
	/// 可空数据读取器
	/// </summary>
	public sealed class NullableDataReader : IDataReader, IDisposable, IDataRecord, INullableReader
	{
		#region 构造器注入
		private IDataReader reader;
		private NullableDataReader()
		{
		}
		/// <summary>
		/// 构造器注入
		/// </summary>
		/// <param name="dataReader"></param>
		public NullableDataReader(IDataReader dataReader)
		{
			reader = dataReader;
		}
		#endregion

		#region 属性
		/// <summary>
		/// 获取一个值，该值指示当前行的嵌套深度。
		/// </summary>
		public int Depth => reader.Depth;
		/// <summary>
		/// 获取一个值，该值指示数据读取器是否已关闭。如果数据读取器已关闭，则为 true；否则为 false。
		/// </summary>
		public bool IsClosed => reader.IsClosed;
		/// <summary>
		/// 通过执行 SQL 语句获取更改、插入或删除的行数。
		/// 如果没有任何行受到影响或语句失败，则为 0；-1 表示 SELECT 语句。
		/// </summary>
		public int RecordsAffected => reader.RecordsAffected;
		/// <summary>
		/// 获取当前行中的列数。如果未放在有效的记录集中，则为 0；
		/// </summary>
		public int FieldCount => reader.FieldCount;
		/// <summary>
		/// 获取具有指定名称的列。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object this[string name] => reader[name];
		/// <summary>
		/// 获取位于指定索引处的列。
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public object this[int i] => reader[i];
		#endregion

		#region 释放资源
		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			if (this.reader != null)
			{
				this.reader.Dispose();
			}
		}

		/// <summary>
		/// 关闭IDataReader对象
		/// </summary>
		public void Close()
		{
			this.reader.Close();
		}
		#endregion

		#region 读取内容
		/// <summary>
		/// 当读取批处理 SQL 语句的结果时，使数据读取器前进到下一个结果。
		/// 如果存在多个行，则为 true；否则为 false。
		/// </summary>
		/// <returns></returns>
		public bool NextResult()
		{
			return reader.NextResult();
		}
		/// <summary>
		/// 使 System.Data.IDataReader 前进到下一条记录。
		/// 如果存在多个行，则为 true；否则为 false。
		/// </summary>
		/// <returns></returns>
		public bool Read()
		{
			return reader.Read();
		}
		#endregion

		#region 获取字段值
		/// <summary>
		/// 获取指定列的布尔值形式的值。
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public bool GetBoolean(int i)
		{
			return reader.GetBoolean(i);
		}
		/// <summary>
		/// 获取指定列的布尔值形式的值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool GetBoolean(string name)
		{
			bool boolean = false;
			if (!this.IsDBNull(name))
			{
				boolean = this.GetBoolean(this.reader.GetOrdinal(name));
			}
			return boolean;
		}

		/// <summary>
		/// 获取指定列的 8 位无符号整数值。
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public byte GetByte(int i)
		{
			return this.reader.GetByte(i);
		}
		/// <summary>
		/// 获取指定列的 8 位无符号整数值。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public byte GetByte(string name)
		{
			return this.GetByte(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 从指定的列偏移量将字节流作为数组从给定的缓冲区偏移量开始读入缓冲区。
		/// 返回结果:读取的实际字节数。
		/// </summary>
		/// <param name="i">从零开始的列序号</param>
		/// <param name="fieldOffset">字段中的索引，从该索引位置开始读取操作</param>
		/// <param name="buffer">要将字节流读入的缓冲区。</param>
		/// <param name="bufferoffset">开始读取操作的 buffer 索引</param>
		/// <param name="length">要读取的字节数</param>
		/// <returns></returns>
		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return this.reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}
		/// <summary>
		/// 获取指定列的字符值
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public char GetChar(int i)
		{
			return this.reader.GetChar(i);
		}
		/// <summary>
		/// 获取指定列的字符值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public char GetChar(string name)
		{
			return this.GetChar(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 从指定的列偏移量将字符流作为数组从给定的缓冲区偏移量开始读入缓冲区
		/// </summary>
		/// <param name="i"></param>
		/// <param name="fieldoffset"></param>
		/// <param name="buffer"></param>
		/// <param name="bufferoffset"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return this.reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}
		/// <summary>
		/// 返回指定的列序号的 System.Data.IDataReader。
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public IDataReader GetData(int i)
		{
			return this.reader.GetData(i);
		}
		/// <summary>
		/// 获取指定字段的数据类型信息
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public string GetDataTypeName(int i)
		{
			return this.reader.GetDataTypeName(i);
		}
		/// <summary>
		/// 获取指定字段的数据类型信息
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetDataTypeName(string name)
		{
			return this.reader.GetDataTypeName(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 获取指定字段的日期和时间数据值
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public DateTime GetDateTime(int i)
		{
			return this.reader.GetDateTime(i);
		}
		/// <summary>
		/// 获取指定字段的日期和时间数据值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public DateTime GetDateTime(string name)
		{
			if (this.IsDBNull(name))
			{
				return DateTime.Now;
			}
			return this.reader.GetDateTime(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 获取指定字段的日期和时间数据值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public DateTime? GetNullableDateTime(string name)
		{
			if (this.IsDBNull(name))
			{
				return null;
			}
			return new DateTime?(this.reader.GetDateTime(this.reader.GetOrdinal(name)));
		}
		/// <summary>
		/// 获取指定字段的固定位置的数值
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public decimal GetDecimal(int i)
		{
			return this.reader.GetDecimal(i);
		}
		/// <summary>
		/// 获取指定字段的固定位置的数值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public decimal GetDecimal(string name)
		{
			decimal @decimal = 0M;
			if (!this.IsDBNull(name))
			{
				@decimal = this.reader.GetDecimal(this.reader.GetOrdinal(name));
			}
			return @decimal;
		}
		/// <summary>
		/// 获取指定字段的双精度浮点数
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public double GetDouble(int i)
		{
			return this.reader.GetDouble(i);
		}
		/// <summary>
		/// 获取指定字段的双精度浮点数
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public double GetDouble(string name)
		{
			double num = 0.0;
			if (!this.IsDBNull(name))
			{
				num = this.reader.GetDouble(this.reader.GetOrdinal(name));
			}
			return num;
		}
		/// <summary>
		/// 获取与从 System.Data.IDataRecord.GetValue(System.Int32) 返回的 System.Object 类型对应的System.Type 信息。
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Type GetFieldType(int i)
		{
			return this.reader.GetFieldType(i);
		}
		/// <summary>
		/// 获取与从 System.Data.IDataRecord.GetValue(System.Int32) 返回的 System.Object 类型对应的System.Type 信息。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Type GetFieldType(string name)
		{
			return this.reader.GetFieldType(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 获取指定字段的单精度浮点数
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public float GetFloat(int i)
		{
			return this.reader.GetFloat(i);
		}
		/// <summary>
		/// 获取指定字段的单精度浮点数
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public float GetFloat(string name)
		{
			float @float = 0f;
			if (!this.IsDBNull(name))
			{
				@float = this.reader.GetFloat(this.reader.GetOrdinal(name));
			}
			return @float;
		}
		/// <summary>
		/// 返回指定字段的 GUID 值。
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Guid GetGuid(int i)
		{
			return this.reader.GetGuid(i);
		}
		/// <summary>
		/// 返回指定字段的 GUID 值。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Guid GetGuid(string name)
		{
			return this.reader.GetGuid(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 获取指定字段的 16 位有符号整数值
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public short GetInt16(int i)
		{
			return this.reader.GetInt16(i);
		}
		/// <summary>
		/// 获取指定字段的 16 位有符号整数值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public short GetInt16(string name)
		{
			if (this.IsDBNull(name))
			{
				return 0;
			}
			return this.reader.GetInt16(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 获取指定字段的 32 位有符号整数值
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public int GetInt32(int i)
		{
			return this.reader.GetInt32(i);
		}
		/// <summary>
		/// 获取指定字段的 32 位有符号整数值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int GetInt32(string name)
		{
			if (this.IsDBNull(name))
			{
				return 0;
			}
			return this.reader.GetInt32(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 获取指定字段的 64 位有符号整数值。
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public long GetInt64(int i)
		{
			return this.reader.GetInt64(i);
		}
		/// <summary>
		/// 获取指定字段的 64 位有符号整数值。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public long GetInt64(string name)
		{
			if (this.IsDBNull(name))
			{
				return 0L;
			}
			return this.reader.GetInt64(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 获取要查找的字段的名称
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public string GetName(int i)
		{
			return this.reader.GetName(i);
		}
		/// <summary>
		/// 返回命名字段的索引
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int GetOrdinal(string name)
		{
			return this.reader.GetOrdinal(name);
		}
		/// <summary>
		/// 获取指定字段的字符串值
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public string GetString(int i)
		{
			return this.reader.GetString(i);
		}
		/// <summary>
		/// 获取指定字段的字符串值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetString(string name)
		{
			string str = string.Empty;
			if (!this.IsDBNull(name))
			{
				str = this.reader.GetString(this.reader.GetOrdinal(name));
			}
			return str;
		}
		/// <summary>
		/// 返回指定字段的值
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public object GetValue(int i)
		{
			return this.reader.GetValue(i);
		}
		/// <summary>
		/// 返回指定字段的值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object GetValue(string name)
		{
			return this.reader.GetValue(this.reader.GetOrdinal(name));
		}
		/// <summary>
		/// 使用当前记录的列值来填充对象数组
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public int GetValues(object[] values)
		{
			return this.reader.GetValues(values);
		}
		#endregion

		#region 检查是否为空
		/// <summary>
		/// 返回是否将指定字段设置为空
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public bool IsDBNull(int i)
		{
			return reader.IsDBNull(i);
		}
		/// <summary>
		/// 返回是否将指定字段设置为空
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool IsDBNull(string name)
		{
			return reader.IsDBNull(reader.GetOrdinal(name));
		}
		#endregion

		/// <summary>
		/// 返回一个 System.Data.DataTable，它描述 System.Data.IDataReader 的列元数据。
		/// </summary>
		/// <returns></returns>
		public DataTable GetSchemaTable()
		{
			return reader.GetSchemaTable();
		}
	}
}
