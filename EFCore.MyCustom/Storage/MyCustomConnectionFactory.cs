using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace EFCore.MyCustom.Storage;

internal class MyCustomConnectionFactory : IRelationalConnectionFactory
{
    private readonly string _connectionString;
    private readonly IRelationalTypeMappingSource _typeMapper;

    public MyCustomConnectionFactory(string connectionString, IRelationalTypeMappingSource typeMapper)
    {
        _connectionString = connectionString;
        _typeMapper = typeMapper;
    }

    public DbConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}
