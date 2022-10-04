using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace PaginationHelper.Tests.Helpers;

public class TestDbContextMigrationFactory : IDesignTimeDbContextFactory<TestDbContext>
{
    public TestDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<TestDbContext>();
        builder = builder.UseNpgsql((NpgsqlDbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.MigrationsAssembly(GetType().Assembly.FullName));

        return new TestDbContext(builder.Options);
    }

}
