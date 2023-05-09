using EFCore.MyCustom.Tests.Models;
using System.Linq;
using Xunit;

namespace EFCore.MyCustom.Tests;

public class GettingStartedTests
{
    [Fact]
    public void Create_AddBlog()
    {
        using var db = new BloggingContext();
        db.Database.EnsureCreated();

        db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
        db.SaveChanges();
    }

    [Fact]
    public void Read_FirstBlog()
    {
        using var db = new BloggingContext();
        db.Database.EnsureCreated();

        var firstBlog = new Blog { Url = "http://blogs.msdn.com/adonet" };
        db.Add(firstBlog);
        db.Add(new Blog { Url = "https://www.billboard.com/" });
        db.Add(new Blog { Url = "https://www.wired.com/" });
        db.SaveChanges();

        var blog = db.Blogs
            .OrderBy(b => b.BlogId)
            .First();

        Assert.Equal(firstBlog.Url, blog.Url);
    }

    [Fact]
    public void Update_UpdateBlogAddPost() 
    {
        using var db = new BloggingContext();
        db.Database.EnsureCreated();

        db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
        db.SaveChanges();

        var blog = db.Blogs
            .OrderBy(b => b.BlogId)
            .First();
        var updatedUrl = "https://devblogs.microsoft.com/dotnet";
        blog.Url = updatedUrl;
        blog.Posts.Add(
            new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
        db.SaveChanges();

        blog = db.Blogs
            .OrderBy(b => b.BlogId)
            .First();

        Assert.Equal(updatedUrl, blog.Url);
        Assert.True(blog.Posts.Any());
    }

    [Fact]
    public void Delete_DeleteBlog()
    {
        using var db = new BloggingContext();
        db.Database.EnsureCreated();

        var firstBlog = new Blog { Url = "http://blogs.msdn.com/adonet" };
        db.Add(firstBlog);
        var secondBlog = "https://www.billboard.com/";
        db.Add(new Blog { Url = secondBlog });
        var thirdBlog = "https://www.wired.com/";
        db.Add(new Blog { Url = thirdBlog });
        db.SaveChanges();

        var blog = db.Blogs
            .OrderBy(b => b.BlogId)
            .First();

        Assert.Equal(firstBlog, blog);
        db.Remove(blog);
        db.SaveChanges();

        var blogs = db.Blogs.OrderBy(b => b.BlogId).ToList();
        Assert.Equal(2, db.Blogs.Count());
        Assert.Equal(secondBlog, blogs[0].Url);
        Assert.Equal(thirdBlog, blogs[1].Url);
    }
}
