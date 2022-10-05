using DockerComposeFixture;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CatConsult.PaginationHelper.Tests.Helpers;

[CollectionDefinition("Database Collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

public class DatabaseFixture : IAsyncLifetime
{
    private readonly DockerFixture _dockerFixture;

    public DatabaseFixture(IMessageSink output)
    {
        _dockerFixture = new DockerFixture(output);
        _dockerFixture.InitOnce(() => new DockerFixtureOptions
        {
            DockerComposeFiles = new[]
            {
                "docker-compose.test.yml"
            },
            CustomUpTest = logs => logs.Any(l => l.Contains("database system is ready to accept connections")),
            DockerComposeArgs = $"--project-name pagination_helper_tests_{Guid.NewGuid():N}"
        });
    }

    public async Task InitializeAsync()
    {
        using var fixture = new TestDbContextFixture();
        var context = fixture.Context;

        await context.Database.MigrateAsync();

        context.AddRange(ATestData.SeedTestEntities());
        await context.SaveChangesAsync();
    }

    public Task DisposeAsync()
    {
        _dockerFixture.Dispose();

        return Task.CompletedTask;
    }


}

