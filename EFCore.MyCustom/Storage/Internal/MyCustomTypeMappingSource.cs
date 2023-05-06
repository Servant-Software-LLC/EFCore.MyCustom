using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.MyCustom.Storage.Internal;

public class MyCustomTypeMappingSource : RelationalTypeMappingSource
{
    public MyCustomTypeMappingSource(
        TypeMappingSourceDependencies dependencies,
        RelationalTypeMappingSourceDependencies relationalDependencies)
        : base(dependencies, relationalDependencies)
    {
    }

    protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        // You can add more mappings or customize existing ones as needed.
        var clrType = mappingInfo.ClrType;
        var storeTypeName = mappingInfo.StoreTypeName;

        if (storeTypeName != null)
        {
            switch (storeTypeName.ToLowerInvariant())
            {
                case "integer":
                    return new IntTypeMapping("INTEGER", System.Data.DbType.Int32);
                case "real":
                    return new DoubleTypeMapping("REAL", System.Data.DbType.Double);
                case "text":
                    return new StringTypeMapping("TEXT", System.Data.DbType.String);
                case "blob":
                    return new ByteArrayTypeMapping("BLOB", System.Data.DbType.Binary);
            }
        }

        if (clrType != null)
        {
            if (clrType == typeof(int))
                return new IntTypeMapping("INTEGER", System.Data.DbType.Int32);
            if (clrType == typeof(double))
                return new DoubleTypeMapping("REAL", System.Data.DbType.Double);
            if (clrType == typeof(string))
                return new StringTypeMapping("TEXT", System.Data.DbType.String);
            if (clrType == typeof(byte[]))
                return new ByteArrayTypeMapping("BLOB", System.Data.DbType.Binary);
        }

        return base.FindMapping(mappingInfo);
    }
}
