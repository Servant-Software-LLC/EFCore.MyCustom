using System.Data.Common;

namespace Microsoft.EntityFrameworkCore.Storage;

/// <summary>
/// This interface used to be a part of the EF Core library (as far as I can tell in the code that ChatGPT generates).
/// ChatGPT indicates that it will be used throughout the internal classes of this provider.
/// </summary>
public interface IRelationalConnectionFactory
{
    DbConnection CreateConnection();
}
