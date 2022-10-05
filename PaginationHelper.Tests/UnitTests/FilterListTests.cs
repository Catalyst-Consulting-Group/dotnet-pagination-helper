using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PaginationHelper.Tests.Helpers;
using Xunit;

namespace PaginationHelper.Tests.UnitTests;

[Collection("Database Collection")]
public class FilterListTests
{
    private readonly TestDbContext _db;
    public FilterListTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task Filter_List_Default()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("List", "N1");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.List).Should().BeEquivalentTo(new List<List<string>>()
        {
           new(){ "N1", "N2" }
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_List_In()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("List__in", "A");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.List).Should().BeEquivalentTo(new List<List<string>>()
        {
           new(){ "N2", "A" }
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_List_Multiple()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("List", "N1", "A");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.List).Should().BeEquivalentTo(new List<List<string>>()
        {
           new(){ "N1", "N2" },
           new(){ "N2", "A" },
        }, opt => opt.WithStrictOrdering());
    }
}
