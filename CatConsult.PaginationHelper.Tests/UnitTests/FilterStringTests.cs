using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using CatConsult.PaginationHelper.Tests.Helpers;
using Xunit;

namespace CatConsult.PaginationHelper.Tests.UnitTests;

[Collection("Database Collection")]
public class FilterStringTests
{
    private readonly TestDbContext _db;
    public FilterStringTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task Filter_String_Default()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("string", "BC");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<string>()
        {
           "ABCD"
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_String_In()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("string__in", "BC");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<string>()
        {
           "ABCD"
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_String_Equal()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("string__eq", "ABCD");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<string>()
        {
           "ABCD"
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_String_Start_With()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("string__start", "AB");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<string>()
        {
           "ABCD"
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_String_End_With()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("string__end", "CD");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<string>()
        {
           "ABCD"
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_String_Multiple()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("string", "AAAA", "aabb", "cd");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<String>()
        {
           "AAAA", "AABB", "ABCD"
        }, opt => opt.WithStrictOrdering());
    }
}
