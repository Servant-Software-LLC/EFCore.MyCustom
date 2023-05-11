using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace EFCore.MyCustom.Storage.Internal;

public class MyCustomRelationalConnection : RelationalConnection, IMyCustomRelationalConnection
{
    public MyCustomRelationalConnection(RelationalConnectionDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override DbConnection CreateDbConnection() => new SqliteConnection(ConnectionString);
}
