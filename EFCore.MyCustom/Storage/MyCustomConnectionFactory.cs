using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace EFCore.MyCustom.Storage;

internal class MyCustomConnectionFactory : IRelationalConnectionFactory
{
    private readonly string _connectionString;
    //private readonly IRelationalTypeMappingSource _typeMapper;

    public MyCustomConnectionFactory(string connectionString /*, IRelationalTypeMappingSource typeMapper */)
    {
        _connectionString = connectionString;
        //_typeMapper = typeMapper;
    }

    public DbConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}
