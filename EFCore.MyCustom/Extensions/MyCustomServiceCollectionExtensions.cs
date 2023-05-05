using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using EFCore.MyCustom.Storage.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

public static class MyCustomServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkMyCustom([NotNull] this IServiceCollection serviceCollection)
    {
        var builder = new EntityFrameworkRelationalServicesBuilder(serviceCollection)
            .TryAdd<IRelationalTypeMappingSource, MyCustomTypeMappingSource>()
;

        builder.TryAddCoreServices();

        return serviceCollection;

    }
}
