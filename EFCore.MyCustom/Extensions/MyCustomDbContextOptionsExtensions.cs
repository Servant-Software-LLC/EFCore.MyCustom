using Microsoft.EntityFrameworkCore.Infrastructure;
using EFCore.MyCustom.Infrastructure.Internal;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Microsoft.EntityFrameworkCore;

public static class MyCustomDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseMyCustom(this DbContextOptionsBuilder optionsBuilder, 
                                                      string connectionString)
    {
        var extension = optionsBuilder.Options.FindExtension<MyCustomOptionsExtension>()
                         ?? new MyCustomOptionsExtension().WithConnectionString(connectionString);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

    public static DbContextOptionsBuilder UseMyCustom(this DbContextOptionsBuilder optionsBuilder,
                                                      SqliteConnection connection)
    {
        var extension = optionsBuilder.Options.FindExtension<MyCustomOptionsExtension>()
                         ?? new MyCustomOptionsExtension().WithConnection(connection);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

}
