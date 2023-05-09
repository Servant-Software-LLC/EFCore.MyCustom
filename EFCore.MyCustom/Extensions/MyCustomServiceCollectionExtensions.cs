using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using EFCore.MyCustom.Storage.Internal;
using System.Diagnostics.CodeAnalysis;
using EFCore.MyCustom.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EFCore.MyCustom.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Update;
using EFCore.MyCustom.Update.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using EFCore.MyCustom.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query;
using EFCore.MyCustom.Query.Internal;

namespace Microsoft.Extensions.DependencyInjection;

public static class MyCustomServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkMyCustom([NotNull] this IServiceCollection serviceCollection)
    {
        var builder = new EntityFrameworkRelationalServicesBuilder(serviceCollection)
            .TryAdd<IRelationalTypeMappingSource, MyCustomTypeMappingSource>()
            .TryAdd<IDatabaseProvider, DatabaseProvider<MyCustomOptionsExtension>>()
            .TryAdd<LoggingDefinitions, MyCustomLoggingDefinitions>()
            .TryAdd<IModificationCommandBatchFactory, MyCustomModificationCommandBatchFactory>()
            .TryAdd<IUpdateSqlGenerator, MyCustomUpdateSqlGenerator>()

            //Found that this was necessary, because the default convension of determining a
            //Model's primary key automatically based off of properties that have 'Id' in their
            //name was getting ignored.
            .TryAdd<IProviderConventionSetBuilder, MyCustomConventionSetBuilder>()

            .TryAdd<IQuerySqlGeneratorFactory, MyCustomQuerySqlGeneratorFactory>()

        ;

        builder.TryAddCoreServices();

        serviceCollection
            .AddScoped<IRelationalConnection, CustomMySqlConnection>()
            .AddSingleton<ISqlGenerationHelper, RelationalSqlGenerationHelper>()
            .AddScoped<IRelationalDatabaseCreator, MyCustomDatabaseCreator>();

        ;

        return serviceCollection;

    }
}
