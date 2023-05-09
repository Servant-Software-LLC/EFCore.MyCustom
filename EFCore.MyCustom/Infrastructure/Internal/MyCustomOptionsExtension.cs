using EFCore.MyCustom.Storage;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.MyCustom.Infrastructure.Internal;

public class MyCustomOptionsExtension : RelationalOptionsExtension
{
    private MyCustomOptionsExtensionInfo? _info;

    public MyCustomOptionsExtension() { }
    protected MyCustomOptionsExtension(MyCustomOptionsExtension copyFrom)
        : base(copyFrom)
    {
        
    }

    public override DbContextOptionsExtensionInfo Info => _info ??= new MyCustomOptionsExtensionInfo(this);

    public override void ApplyServices(IServiceCollection services)
    {
        //Will be uncommented in the last step when the Extension classes are created.
        //services.AddEntityFrameworkMyCustom();
        
        services.AddSingleton<IRelationalConnectionFactory>(provider =>
        {
            var typeMapper = provider.GetService<IRelationalTypeMappingSource>();
            return new MyCustomConnectionFactory(ConnectionString, typeMapper);
        });

        
    }
       


    public override void Validate(IDbContextOptions options)
    {
        // You can add any validation logic here, if necessary.
    }

    protected override RelationalOptionsExtension Clone() => new MyCustomOptionsExtension(this);


    public class MyCustomOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public MyCustomOptionsExtensionInfo(MyCustomOptionsExtension extension)
            : base(extension)
        {
        }

        public override bool IsDatabaseProvider => true;

        public override string LogFragment => $"Using Custom SQLite Provider - ConnectionString: {Extension.ConnectionString}";

        public override int GetServiceProviderHashCode() => Extension.ConnectionString.GetHashCode();

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => other is MyCustomOptionsExtensionInfo;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["MyCustom:ConnectionString"] = Extension.ConnectionString;
        }

        public override MyCustomOptionsExtension Extension => (MyCustomOptionsExtension)base.Extension;
    }

}
