using System;

namespace MyADO
{
	/// <summary>
	/// 可空数据读取器接口
	/// </summary>
	public interface INullableReader
	{
		/// <summary>
		/// 获取指定列的布尔值形式的值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		bool GetBoolean(string name);
		/// <summary>
		/// 获取指定列的 8 位无符号整数值。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		byte GetByte(string name);
		/// <summary>
		/// 获取指定列的字符值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		char GetChar(string name);
		/// <summary>
		/// 获取指定字段的日期和时间数据值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		DateTime GetDateTime(string name);
		/// <summary>
		/// 获取指定字段的固定位置的数值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		decimal GetDecimal(string name);
		/// <summary>
		/// 获取指定字段的双精度浮点数
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		double GetDouble(string name);
		/// <summary>
		/// 获取指定字段的单精度浮点数
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		float GetFloat(string name);
		/// <summary>
		/// 返回指定字段的 GUID 值。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		Guid GetGuid(string name);
		/// <summary>
		/// 获取指定字段的 16 位有符号整数值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		short GetInt16(string name);
		/// <summary>
		/// 获取指定字段的 32 位有符号整数值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		int GetInt32(string name);
		/// <summary>
		/// 获取指定字段的 64 位有符号整数值。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		long GetInt64(string name);
		/// <summary>
		/// 获取指定字段的字符串值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		string GetString(string name);
		/// <summary>
		/// 返回指定字段的值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		object GetValue(string name);
		/// <summary>
		/// 返回是否将指定字段设置为空
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		bool IsDBNull(string name);
	}
}
