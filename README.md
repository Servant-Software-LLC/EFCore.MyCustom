# Tutorial - Create an EF Core Provider  

This tutorial makes the following assumptions:

*   Create an EF Core Provider that is built on top of an existing ADO.NET Provider.
    
*   The backed database is relational.
    
*   You are using EF Core 6.x
    
*   Your provider needs to do only CRUD operations and have support for [EnsureCreated()](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.storage.idatabasecreator.ensurecreated#microsoft-entityframeworkcore-storage-idatabasecreator-ensurecreated) and [EnsureDeleted()](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.storage.idatabasecreator.ensuredeleted#microsoft-entityframeworkcore-storage-idatabasecreator-ensuredeleted). (i.e. no [Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/) nor [Scaffolding](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli) support yet)
    

Overview
--------

In this tutorial, we will look at what it takes to create a basic EF Core Provider that merely can handle all the operations demostrated in the [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli). By the end of this tutorial, your provider will have unit tests to exerise each of these operations.

In this example provider, we will use the **SQLite** ADO.NET Provider as the basis to build upon. This ADO.NET Provider has an [in-memory database ability](https://www.sqlite.org/inmemorydb.html) that will make it easy to setup our unit tests.

To implement a basic custom Entity Framework Core Data Provider, you'll need to create and configure the following main classes and components:

1.  `MyCustomTypeMappingSource`: A custom implementation of [RelationalTypeMappingSource](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.storage.relationaltypemappingsource?view=efcore-6.0) that maps CLR types to the corresponding SQLite data types and vice versa.
    
2.  `MyCustomConnectionFactory`: A custom implementation of `IRelationalConnectionFactory` that creates and returns instances of `MySqlConnection`, which are used to connect to the SQLite database.
    
3.  DbContext extension method: An extension method for [DbContextOptionsBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontextoptionsbuilder?view=efcore-6.0) that allows users to configure the custom SQLite provider when setting up their `DbContext`.
    
4.  `MyCustomOptionsExtension`: A custom implementation of [RelationalOptionsExtension](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.infrastructure.relationaloptionsextension?view=efcore-6.0) that configures the provider with the necessary services and settings specific to SQLite. This class is responsible for registering custom services like `MyCustomConnectionFactory` and `MyCustomTypeMappingSource`.
    

There will also be 2 extension classes that are used to wire-up the dependency injection required. They are:

1.  `MyCustomDbContextOptionsExtensions`: A static class that contains extension methods for configuring a custom EF Core data provider within a `DbContext`. These extension methods provide an easy-to-use API for configuring the custom provider and its options. The primary purpose of the class is to define a `UseMyCustom` (which you will rename) extension method. This method accepts a connection string and optional configuration options as parameters. It is used to configure the custom provider for a `DbContext` by adding an instance of the `MyCustomOptionsExtension` to the `DbContextOptionsBuilder`.
    
2.  `MyCustomServiceCollectionExtensions`: A static class that contains extension methods for registering the services required by the custom EF Core data provider with the dependency injection container. These extension methods extend the `IServiceCollection` interface, making it easy for developers to configure the custom provider's services within the application's startup code or other initialization logic.
    
    The primary purpose of the class is to define a `AddEntityFrameworkMyCustom` (which you will rename) extension method. This method accepts an `IServiceCollection` instance and registers the necessary services for the custom SQLite provider, such as `IDatabaseProvider`, `IRelationalTypeMappingSource`, `IRelationalConnectionFactory`, and `IRelationalConnection`.
    

Detailed Implementations
------------------------

This section provides sub-sections that walk you through what is required for a basic EF Core Provider. By the end of each sub-section, you should be able to compile your provider. Successful complitation (and passing unit tests, in the last sub-section), helps serve as checkpoints to indicate that you are properly implementing your provider. The result of completing each sub-section is represented in [branches](https://github.com/Servant-Software-LLC/EFCore.MyCustom/branches) of the [this repo](https://github.com/Servant-Software-LLC/EFCore.MyCustom).

### Options Extension Class (Step #1)

As a starting point, we’ll add references to the libraries that we need, get a few classes (that `MyCustomOptionsExtension` needs) in place and implement the `MyCustomOptionsExtension` class:

1.  Add the necessary NuGet packages for EF Core and your ADO.NET Provider:
    
    *   `Microsoft.EntityFrameworkCore`
        
    *   `Microsoft.EntityFrameworkCore.Relational`
        
    *   `<Your ADO.NET Provider>` in this example, `MySql.Data.EntityFrameworkCore`
        
2.  Implement [**MyCustomTypeMappingSource**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%231_of_4/EFCore.MyCustom/Storage/Internal/MyCustomTypeMappingSource.cs) and define all CLR to database types.
    
3.  Implement [**IRelationalConnectionFactory**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%231_of_4/EFCore.MyCustom/Storage/IRelationalConnectionFactory.cs) and [**MyCustomConnectionFactory**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%231_of_4/EFCore.MyCustom/Storage/MyCustomConnectionFactory.cs) that merely constructs an instance of your ADO.NET Connection class by providing it with the current connection string.
    
4.  Finally, we can implement [**MyCustomOptionsExtension**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%231_of_4/EFCore.MyCustom/Infrastructure/Internal/MyCustomOptionsExtension.cs). Add the necessary services and settings that are specific to your ADO.NET Provider.
    

At this point, you should be able to build your provider. Nothing is wired up, so you will not yet be able to do anything with it.

### Boilerplate Classes (Step #2)

As mentioned before, these classes are necessary for registering the services required by the custom EF Core data provider. After completing this sub-section, you should be able to create your own `DbContext` to make use of your provider.

1.  The EF Core Framework, expects that your provider has registered a [LoggingDefinitions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.diagnostics.loggingdefinitions?view=efcore-6.0) service. Unfortunately, `LoggingDefinitions` and even [RelationalLoggingDefinitions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.diagnostics.relationalloggingdefinitions?view=efcore-6.0) are abstract. Therefore, we need to implement [**MyCustomLoggingDefinitions**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%232_of_4/EFCore.MyCustom/Diagnostics/Internal/MyCustomLoggingDefinitions.cs).
    
2.  The Framework also needs to be able to create and manage instances of the underlying ADO.NET database connection, such as in our case, `SqliteConnection` for SQLite. Therefore, we must implement [**MyCustomConnection**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%232_of_4/EFCore.MyCustom/Diagnostics/Internal/MyCustomConnection.cs) which inherits the abstract class RelationalConnection.
    
3.  For updates, EF Core needs to be able to create batch instances. There is no default implementation, so we need to create our own. Implement [**MyCustomModificationCommandBatchFactory**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%232_of_4/EFCore.MyCustom/Update/Internal/MyCustomModificationCommandBatchFactory.cs). The factory creates instances of `SingularModificationCommandBatch`. That class creates a single command batch for each operation, effectively disabling command batching. If you need batching, then implement your own custom class that inherits `ModificationCommandBatch` and create an instance of it in the factory’s Create() method.
    
4.  Although, it isn’t absolutely necessary, the default convention for your EF Core provider to automatically determine on a model if a column a primary key and auto-incrementing if its name is ‘Id’ or ends with ‘Id’ wasn’t working until our provider derived a class off of `RelationalConventionSetBuilder` and registered `IProviderConventionSetBuilder`. This derived class is called [**MyCustomConventionSetBuilder**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%232_of_4/EFCore.MyCustom/Metadata/Conventions/MyCustomConventionSetBuilder.cs).
    

### Provider Specific Classes (Step #3)

So far, all the classes implemented, have been fairly boiler plate, only using a few of the major ADO.NET provider classes. This section will address classes have very specific logic of your database technology that isn’t available through its ADO.NET provider.

1.  The EF Core Framework needs to be able to generate SQL for queries that are compatible with the SQLite database system, based on the Entity Framework Core's internal representation of LINQ queries. In our case, when a user uses the [First()](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.first?view=net-7.0) linq method, the EF Core Framework creates SQL with the [FETCH](https://learn.microsoft.com/en-us/sql/t-sql/language-elements/fetch-transact-sql?view=sql-server-ver16) command in it. For this example though, the SQLite database does not know the FETCH command (among other things) and hence we must create our own derived class [**MyCustomQuerySqlGenerator**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%233_of_4/EFCore.MyCustom/Query/Internal/MyCustomQuerySqlGenerator.cs) which inherits [QuerySqlGenerator](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.query.querysqlgenerator?view=efcore-6.0).
    
2.  The `MyCustomQuerySqlGenerator` must be created by a factory and therefore, we must create [**MyCustomQuerySqlGeneratorFactory**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%233_of_4/EFCore.MyCustom/Query/Internal/MyCustomQuerySqlGeneratorFactory.cs) to simply inherit IQuerySqlGeneratorFactory and have its Create() method instantiate an instance of our `MyCustomQuerySqlGenerator` class.
    
3.  The EF Core Framework needs to be able to generate SQL for update operations, such as INSERT, UPDATE, and DELETE. The `IUpdateSqlGenerator` interface must be implemented to provide this provider-specific SQL commands. Typically, a provider will derive their implementation from the `UpdateSqlGenerator` abstract class. For our case, in order to avoid adding a dependency on SQLite EF Core provider, we have just copied the code for this class that is called [**MyCustomUpdateSqlGenerator**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%233_of_4/EFCore.MyCustom/Update/Internal/SqliteUpdateSqlGenerator.cs).
    
4.  The Framework also needs to be able to handle the creation, deletion, and existence check of relational databases. This is done by it offering the interface `IRelationalDatabaseCreator` and abstract class `RelationalDatabaseCreator`. Again, a derived class must be created for your provider. In this tutorial, the example implements [**MyCustomDatabaseCreator**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%233_of_4/EFCore.MyCustom/Storage/Internal/MyCustomDatabaseCreator.cs).
    

### Extension Classes (Step #4)

Now that all the essential classes for CRUD operations in your provider have been implemented, we need to expose them for both the EF Core Framework and consumers to take advantage of. These classes are typical C# static extension classes and follow EF Core’s standard pattern for wiring everything up.

1.  Implement [**MyCustomDbContextOptionsExtensions**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%234_of_4/EFCore.MyCustom/Extensions/MyCustomDbContextOptionsExtensions.cs).
    
2.  Implement [**MyCustomServiceCollectionExtensions**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/blob/BasicProvider_Step_%234_of_4/EFCore.MyCustom/Extensions/MyCustomServiceCollectionExtensions.cs).
    
3.  Add our the [**Getting Started unit tests**](https://github.com/Servant-Software-LLC/EFCore.MyCustom/tree/BasicProvider_Step_%234_of_4/EFCore.MyCustom.Tests) to exercise our provider. Of course, these unit tests are very minimal and you’ll need to build unit tests that are specialized for your specific provider.
