using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace PaginationHelper.Tests.Helpers;

public class TestDbContextFixture : IDisposable
{
    public TestDbContextFixture()
    {
        var connectionString = BuildConnectionString();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(
                connectionString,
                o => o.MigrationsAssembly(GetType().Assembly.FullName)
            )
            .Options;
        Context = new TestDbContext(options);
    }

    private static string BuildConnectionString()
    {
        var host = "127.0.0.1";
        var port = "5433";
        var database = "pagination_helper_tests";
        var user = "pagination_helper_tests";
        var password = "pagination_helper_tests";
        return $"Host={host};Port={port};Database={database};Username={user};Password={password};Include Error Detail=true";
    }

    public TestDbContext Context { get; }

    public void Dispose() => Context.Dispose();
}
