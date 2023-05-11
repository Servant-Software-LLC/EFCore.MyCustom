using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace EFCore.MyCustom.Metadata.Conventions;

public class MyCustomConventionSetBuilder : RelationalConventionSetBuilder
{
    public MyCustomConventionSetBuilder(
    ProviderConventionSetBuilderDependencies dependencies,
    RelationalConventionSetBuilderDependencies relationalDependencies)
    : base(dependencies, relationalDependencies)
    {
    }

    public override ConventionSet CreateConventionSet() => base.CreateConventionSet();

}
