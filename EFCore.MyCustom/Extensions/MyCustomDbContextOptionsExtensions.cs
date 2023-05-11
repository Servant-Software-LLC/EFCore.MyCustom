using Microsoft.EntityFrameworkCore.Infrastructure;
using EFCore.MyCustom.Infrastructure.Internal;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Microsoft.EntityFrameworkCore;

public static class MyCustomDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseMyCustom(this DbContextOptionsBuilder optionsBuilder,
                                                      string connectionString)
    {
        if (optionsBuilder == null)
            throw new ArgumentNullException(nameof(optionsBuilder));
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        var extension = (MyCustomOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        ConfigureWarnings(optionsBuilder);

        return optionsBuilder;
    }

    public static DbContextOptionsBuilder UseMyCustom(this DbContextOptionsBuilder optionsBuilder,
                                                      SqliteConnection connection)
    {
        if (optionsBuilder == null)
            throw new ArgumentNullException(nameof(optionsBuilder));
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        var extension = (MyCustomOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnection(connection);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        ConfigureWarnings(optionsBuilder);

        return optionsBuilder;
    }

    /// <summary>
    /// Returns an existing instance of <see cref="NpgsqlOptionsExtension"/>, or a new instance if one does not exist.
    /// </summary>
    /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> to search.</param>
    /// <returns>
    /// An existing instance of <see cref="NpgsqlOptionsExtension"/>, or a new instance if one does not exist.
    /// </returns>
    private static MyCustomOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.Options.FindExtension<MyCustomOptionsExtension>() is MyCustomOptionsExtension existing
            ? new MyCustomOptionsExtension(existing)
            : new MyCustomOptionsExtension();

    private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
    {
        var coreOptionsExtension = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()
            ?? new CoreOptionsExtension();

        coreOptionsExtension = coreOptionsExtension.WithWarningsConfiguration(
            coreOptionsExtension.WarningsConfiguration.TryWithExplicit(
                RelationalEventId.AmbientTransactionWarning, WarningBehavior.Throw));

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
    }

}
