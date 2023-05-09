using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace EFCore.MyCustom.Diagnostics.Internal;

public class CustomMySqlConnection : RelationalConnection
{
    public CustomMySqlConnection(RelationalConnectionDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override DbConnection CreateDbConnection() => new SqliteConnection(ConnectionString);
}
