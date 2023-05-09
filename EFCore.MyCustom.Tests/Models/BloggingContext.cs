using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EFCore.MyCustom.Tests.Models;


public class BloggingContext : DbContext
{
    //Since this is an in-memory database, we need to ensures that a single connection to the in-memory
    //(i.e. there is no persistent between connections) SQLite database is used and keep it open for the
    //entire duration of the test.
    private SqliteConnection connection;

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public BloggingContext()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseMyCustom(connection);
}
