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
                case "int":
                    return new IntTypeMapping("int", System.Data.DbType.Int32);
                case "tinyint":
                    return new ByteTypeMapping("tinyint", System.Data.DbType.Byte);
                case "smallint":
                    return new ShortTypeMapping("smallint", System.Data.DbType.Int16);
                case "bigint":
                    return new LongTypeMapping("bigint", System.Data.DbType.Int64);
                case "varchar":
                    return new StringTypeMapping(storeTypeName, System.Data.DbType.String);
                case "datetime":
                    return new DateTimeTypeMapping("datetime", System.Data.DbType.DateTime);
                case "float":
                    return new FloatTypeMapping("float", System.Data.DbType.Single);
                case "double":
                    return new DoubleTypeMapping("double", System.Data.DbType.Double);
                case "decimal":
                    return new DecimalTypeMapping("decimal", System.Data.DbType.Decimal);
            }
        }

        if (clrType != null)
        {
            if (clrType == typeof(int))
                return new IntTypeMapping("int", System.Data.DbType.Int32);
            if (clrType == typeof(short))
                return new ShortTypeMapping("smallint", System.Data.DbType.Int16);
            if (clrType == typeof(long))
                return new LongTypeMapping("bigint", System.Data.DbType.Int64);
            if (clrType == typeof(byte))
                return new ByteTypeMapping("tinyint", System.Data.DbType.Byte);
            if (clrType == typeof(string))
                return new StringTypeMapping("varchar", System.Data.DbType.String);
            if (clrType == typeof(DateTime))
                return new DateTimeTypeMapping("datetime", System.Data.DbType.DateTime);
            if (clrType == typeof(float))
                return new FloatTypeMapping("float", System.Data.DbType.Single);
            if (clrType == typeof(double))
                return new DoubleTypeMapping("double", System.Data.DbType.Double);
            if (clrType == typeof(decimal))
                return new DecimalTypeMapping("decimal", System.Data.DbType.Decimal);
        }

        return base.FindMapping(mappingInfo);
    }
}
