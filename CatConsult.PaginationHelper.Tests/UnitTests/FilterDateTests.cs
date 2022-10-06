using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using CatConsult.PaginationHelper.Tests.Helpers;
using Xunit;

namespace CatConsult.PaginationHelper.Tests.UnitTests;

[Collection("Database Collection")]
public class FilterDateTests
{
    private readonly TestDbContext _db;
    public FilterDateTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task Filter_Date_Default()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("date", "2000-1-15 01:02:03");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Date).Should().BeEquivalentTo(new List<DateTimeOffset>()
        {
           Utilities.CreateDateTime(2000, 1, 15, 1, 2, 3),
           Utilities.CreateDateTime(2000, 1, 15)
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Date_Equal()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Date__eq", "2000-1-15 01:02:03");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Date).Should().BeEquivalentTo(new List<DateTimeOffset>()
        {
            Utilities.CreateDateTime(2000, 1, 15, 1, 2, 3),
            Utilities.CreateDateTime(2000, 1, 15)
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Date_Greater_Than()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Date__gt", "2000-3-15");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Date).Should().BeEquivalentTo(new List<DateTimeOffset>()
        {
           Utilities.CreateDateTime(2001, 4, 15)
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Date_Greater_Than_Or_Equal()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Date__gte", "2000-3-15");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Date).Should().BeEquivalentTo(new List<DateTimeOffset>()
        {
            Utilities.CreateDateTime(2000, 3, 15),
            Utilities.CreateDateTime(2001, 4, 15)
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Date_Less_Than()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Date__lt", "2000-01-15 01:02:03");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Date).Should().BeEquivalentTo(new List<DateTimeOffset>()
        {
            Utilities.CreateDateTime(2000, 1, 15)
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Date_Less_Than_Or_Equal()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Date__lte", "2000-01-15 01:02:03");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Date).Should().BeEquivalentTo(new List<DateTimeOffset>()
        {
            Utilities.CreateDateTime(2000, 1, 15, 1, 2, 3),
            Utilities.CreateDateTime(2000, 1, 15)
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Date_Multiple()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Date__gte", "2000-02-15")
            .Add("Date__lt", "2000-03-15");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Date).Should().BeEquivalentTo(new List<DateTimeOffset>()
        {
            Utilities.CreateDateTime(2000, 2, 15)
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Date_Invalid_Value()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Date__gte", "asdbad")
            .Add("Date__gt", "asdbad")
            .Add("Date__lt", "asdbad")
            .Add("Date__lte", "asdbad");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Should().BeEmpty();
        actual.Count.Should().Be(0);
    }
}
