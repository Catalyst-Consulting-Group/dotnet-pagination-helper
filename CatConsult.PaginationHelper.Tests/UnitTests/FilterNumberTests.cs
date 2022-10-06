using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using CatConsult.PaginationHelper.Tests.Helpers;
using Xunit;

namespace CatConsult.PaginationHelper.Tests.UnitTests;

[Collection("Database Collection")]
public class FilterNumberTests
{
    private readonly TestDbContext _db;
    public FilterNumberTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task Filter_Number_Default()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("number", "1");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal>()
        {
           1
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Number_Equal()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("number__eq", "1");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal>()
        {
           1
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Number_Greater_Than()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("number__gt", "1.5");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal>()
        {
           100, 200
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Number_Greater_Than_Or_Equal()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("number__gte", "1.5");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal>()
        {
           1.5m, 100, 200
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Number_Less_Than()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("number__lt", "1.5");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal>()
        {
           1, 1.1m
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Number_Less_Than_Or_Equal()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("number__lte", "1.5");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal>()
        {
           1, 1.1m, 1.5m
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Number_Multiple()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Number__gte", "1.1")
            .Add("Number__lt", "100");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<Decimal>()
        {
            1.1m, 1.5m
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Number_Invalid()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Number__gte", "adwad")
            .Add("Number__gt", "adwad")
            .Add("Number__lte", "adwad")
            .Add("Number__lt", "adwad");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Should().BeEmpty();
        actual.Count.Should().Be(0);
    }
}
