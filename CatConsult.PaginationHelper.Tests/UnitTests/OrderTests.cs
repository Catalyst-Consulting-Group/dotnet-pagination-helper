using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using CatConsult.PaginationHelper.Tests.Helpers;
using Xunit;

namespace CatConsult.PaginationHelper.Tests.UnitTests;

[Collection("Database Collection")]
public class OrderTests
{
    private readonly TestDbContext _db;
    public OrderTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task Order_By_String_ASC()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("orderBy", "string")
            .Add("orderDirection", "asc");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<string>()
        {
           "AAAA", "AABB", "ABCD", "BBBB", "CCCC", null
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Order_By_String_DESC()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("orderBy", "string")
            .Add("orderDirection", "desc");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<string>()
        {
           null, "CCCC", "BBBB", "ABCD", "AABB", "AAAA"
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Order_By_Number_ASC()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("orderBy", "number")
            .Add("orderDirection", "asc");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal?>()
        {
           1,
           1.1m,
           1.5m,
           100,
           200,
           null,
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Order_By_Number_DESC()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("orderBy", "number")
            .Add("orderDirection", "desc");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal?>()
        {
           null,
           200,
           100,
           1.5m,
           1.1m,
           1
        }, opt => opt.WithStrictOrdering());
    }
}
