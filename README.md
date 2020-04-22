# MyADO
这是一个简单的数据库操作类，基于netstandard2.1，目前只支持SqlServer数据库。
类库文件说明：
INullableReader								可空数据读取器接口
NullableDataReader						可空数据读取器
IDBOperator										通用数据库操作接口
SqlDBOperator									实现SQLServer数据库操作
DBManagerFactory							数据库管理工厂类
