using EFCore.MyCustom.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.MyCustom.Infrastructure.Internal;

public class MyCustomOptionsExtension : RelationalOptionsExtension
{
    private string _connectionString;

    public MyCustomOptionsExtension(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override DbContextOptionsExtensionInfo Info => throw new NotImplementedException();

    public override void ApplyServices(IServiceCollection services)
    {
        services.AddEntityFrameworkMyCustom();
        services.AddSingleton<IRelationalConnectionFactory>(provider =>
        {
            var typeMapper = provider.GetService<IRelationalTypeMappingSource>();
            return new MyCustomConnectionFactory(_connectionString, typeMapper);
        });
    }
       


    public override void Validate(IDbContextOptions options)
    {
        // You can add any validation logic here, if necessary.
    }

    protected override RelationalOptionsExtension Clone()
    {
        return new MyCustomOptionsExtension(_connectionString);
    }

}
