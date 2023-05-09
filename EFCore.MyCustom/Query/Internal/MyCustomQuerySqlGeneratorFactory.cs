using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.MyCustom.Query.Internal;

public class MyCustomQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
{
    public MyCustomQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies)
    {
        Dependencies = dependencies;
    }

    /// <summary>
    ///     Relational provider-specific dependencies for this service.
    /// </summary>
    protected virtual QuerySqlGeneratorDependencies Dependencies { get; }

    public virtual QuerySqlGenerator Create()
        => new MyCustomQuerySqlGenerator(Dependencies);
}
