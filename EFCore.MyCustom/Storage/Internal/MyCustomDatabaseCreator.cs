using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.MyCustom.Storage.Internal;

public class MyCustomDatabaseCreator : RelationalDatabaseCreator
{
    private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

    public MyCustomDatabaseCreator(
        RelationalDatabaseCreatorDependencies dependencies,
        IRawSqlCommandBuilder rawSqlCommandBuilder)
        : base(dependencies)
    {
        _rawSqlCommandBuilder = rawSqlCommandBuilder;
    }

    public override void Create()
    {
        Dependencies.Connection.Open();

        _rawSqlCommandBuilder.Build("PRAGMA journal_mode = 'wal';")
            .ExecuteNonQuery(
                new RelationalCommandParameterObject(
                    Dependencies.Connection,
                    null,
                    null,
                    null,
                    Dependencies.CommandLogger, CommandSource.Migrations));

        Dependencies.Connection.Close();
    }

    public override void Delete()
    {
        string? path = null;

        Dependencies.Connection.Open();
        try
        {
            path = Dependencies.Connection.DbConnection.DataSource;
        }
        catch
        {
            // any exceptions here can be ignored
        }
        finally
        {
            Dependencies.Connection.Close();
        }

        if (!string.IsNullOrEmpty(path))
        {
            SqliteConnection.ClearPool(new SqliteConnection(Dependencies.Connection.ConnectionString));
            // See issues #25797 and #26016
            // SqliteConnection.ClearAllPools();
            File.Delete(path);
        }
    }

    public override bool Exists()
    {
        var connectionOptions = new SqliteConnectionStringBuilder(Dependencies.Connection.ConnectionString);
        if (connectionOptions.DataSource.Equals(":memory:", StringComparison.OrdinalIgnoreCase)
            || connectionOptions.Mode == SqliteOpenMode.Memory)
        {
            return true;
        }

        throw new Exception("For this sample provider, we only want to support in-memory mode of the SQLite");
    }

    public override bool HasTables()
    {
        var count = (long)_rawSqlCommandBuilder
            .Build("SELECT COUNT(*) FROM \"sqlite_master\" WHERE \"type\" = 'table' AND \"rootpage\" IS NOT NULL;")
            .ExecuteScalar(
                new RelationalCommandParameterObject(
                    Dependencies.Connection,
                    null,
                    null,
                    null,
                    Dependencies.CommandLogger, CommandSource.Migrations))!;

        return count != 0;
    }
}
