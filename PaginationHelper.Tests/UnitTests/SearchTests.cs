using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PaginationHelper.Tests.Helpers;
using Xunit;

namespace PaginationHelper.Tests.UnitTests;

[Collection("Database Collection")]
public class SearchTests
{
    private readonly TestDbContext _db;
    public SearchTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task Search_By_String()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("search", "AA")
            .Add("columns", "string");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.String).Should().BeEquivalentTo(new List<string>()
        {
           "AAAA", "AABB"
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Search_By_Date()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("search", "2000-01-15")
            .Add("columns", "date");

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
    public async Task Search_By_Number()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("search", "1.5")
            .Add("columns", "number");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Number).Should().BeEquivalentTo(new List<decimal>()
        {
            1.5m
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Search_By_List()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("search", "N2")
            .Add("columns", "list");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.List).Should().BeEquivalentTo(new List<List<string>>()
        {
            new(){ "N1", "N2" },
            new(){ "N2", "A" }
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Search_In_Multiple_Columns()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("search", "A")
            .Add("columns", "list", "string");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<TestDto>()
        {
            Data = new List<TestDto>()
            {
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 1, 15, 1, 2, 3),
                    Enum = TestEnum.CaseA,
                    Number = 1,
                    String = "AAAA"
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 1, 15),
                    Enum = TestEnum.CaseB,
                    Number = 1.1m,
                    String = "AABB"
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 3, 15),
                    Enum = TestEnum.CaseC,
                    Number = 100,
                    String = "ABCD",
                    List= new List<string>() { "N1" , "N2" }
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2001, 4, 15),
                    Enum = TestEnum.CaseC,
                    Number = 200,
                    String = "CCCC",
                    List= new List<string>() { "N2" , "A" }
                },
            },
            Count = 4
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Search_Without_Columns()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("search", "N2");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Should().HaveCount(6);
    }

}
