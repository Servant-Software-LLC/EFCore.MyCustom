using Microsoft.EntityFrameworkCore.Infrastructure;
using EFCore.MyCustom.Infrastructure.Internal;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using EFCore.MyCustom.Storage;

namespace Microsoft.EntityFrameworkCore;

public static class MyCustomDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseMyCustom(this DbContextOptionsBuilder optionsBuilder, 
                                                      string connectionString)
    {
        var factory = new MyCustomConnectionFactory(connectionString);
        return UseMyCustomInternal(optionsBuilder, factory.CreateConnection());
    }

    public static DbContextOptionsBuilder UseMyCustom(this DbContextOptionsBuilder optionsBuilder,
                                                      SqliteConnection connection)
        => UseMyCustomInternal(optionsBuilder, (DbConnection)connection);

    private static DbContextOptionsBuilder UseMyCustomInternal(this DbContextOptionsBuilder optionsBuilder,
                                                      DbConnection connection)
    {
        var extension = optionsBuilder.Options.FindExtension<MyCustomOptionsExtension>()
                         ?? new MyCustomOptionsExtension().WithConnection(connection);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

}
