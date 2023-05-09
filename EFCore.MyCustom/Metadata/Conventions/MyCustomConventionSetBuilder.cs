using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
