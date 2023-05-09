using Microsoft.EntityFrameworkCore.Update;

namespace EFCore.MyCustom.Update.Internal;

internal class MyCustomModificationCommandBatchFactory : IModificationCommandBatchFactory
{
    public MyCustomModificationCommandBatchFactory(ModificationCommandBatchFactoryDependencies dependencies)
    {
        Dependencies = dependencies;
    }

    /// <summary>
    ///     Relational provider-specific dependencies for this service.
    /// </summary>
    protected virtual ModificationCommandBatchFactoryDependencies Dependencies { get; }

    //Note:  Create your own derived class, if needed for your custom provider
    public virtual ModificationCommandBatch Create()
        => new SingularModificationCommandBatch(Dependencies);
}
