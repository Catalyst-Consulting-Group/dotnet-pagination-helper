using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PaginationHelper.Tests.Helpers;
using Xunit;

namespace PaginationHelper.Tests;

[Collection("Database Collection")]
public class IntegrationTests
{
    private readonly TestDbContext _db;

    public IntegrationTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task Case0()
    {
        // no filters
        // order by number, desc
        // last page, 2 rows per page
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("orderBy", "number")
            .Add("orderDirection", "desc")
            .Add("rowsPerPage", "2")
            .Add("page", "2");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<TestDto>()
        {
            Data = new List<TestDto>()
            {
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 1, 15),
                    Enum = TestEnum.CaseB,
                    Number = 1.1m,
                    String = "AABB"
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 1, 15, 1, 2, 3),
                    Enum = TestEnum.CaseA,
                    Number = 1,
                    String = "AAAA"
                },
            },
            Count = 6
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Case1()
    {
        // filter by 'string' column that contains 'a' or 'c'
        // order by number dec
        // first page, 3 rows per page
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("string", "a")
            .Add("string", "c")
            .Add("orderBy", "number")
            .Add("orderDirection", "desc")
            .Add("rowsPerPage", "3")
            .Add("page", "0");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<TestDto>()
        {
            Data = new List<TestDto>()
            {
                new()
                {
                    Date = Utilities.CreateDateTime(2001, 4, 15),
                    Enum = TestEnum.CaseC,
                    Number = 200,
                    String = "CCCC",
                    List= new List<string>() { "N2" , "A" }
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
                    Date = Utilities.CreateDateTime(2000, 1, 15),
                    Enum = TestEnum.CaseB,
                    Number = 1.1m,
                    String = "AABB"
                }
            },
            Count = 4
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Case2()
    {
        // search for any 'list,string' columns that contains 'A'
        // filter by 'list' that contains N2
        // order by date dec
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("columns", "list", "string")
            .Add("list", "N2")
            .Add("orderBy", "date")
            .Add("orderDirection", "desc");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<TestDto>()
        {
            Data = new List<TestDto>()
            {
                new()
                {
                    Date = Utilities.CreateDateTime(2001, 4, 15),
                    Enum = TestEnum.CaseC,
                    Number = 200,
                    String = "CCCC",
                    List= new List<string>() { "N2" , "A" }
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 3, 15),
                    Enum = TestEnum.CaseC,
                    Number = 100,
                    String = "ABCD",
                    List= new List<string>() { "N1" , "N2" }
                }
            },
            Count = 2
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Case3()
    {
        // filter by 'string' that start with 'C' or end with 'B'
        // filter by 'date' that is less than 2001/1/1 and greater or equal to 2000/1/1
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("string__start", "C")
            .Add("string__end", "B")
            .Add("date__lt", "2001/1/1")
            .Add("date__gte", "2000/1/1");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<TestDto>()
        {
            Data = new List<TestDto>()
            {
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 1, 15),
                    Enum = TestEnum.CaseB,
                    Number = 1.1m,
                    String = "AABB"
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 2, 15),
                    Enum = TestEnum.CaseB,
                    Number = 1.5m,
                    String = "BBBB"
                },
            },
            Count = 2
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Case4()
    {
        // filter by 'enum' is 2
        // filter by string contains 'B'
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("enum__eq", ((int)TestEnum.CaseB).ToString())
            .Add("string__in", "b");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<TestDto>()
        {
            Data = new List<TestDto>()
            {
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 1, 15),
                    Enum = TestEnum.CaseB,
                    Number = 1.1m,
                    String = "AABB"
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 2, 15),
                    Enum = TestEnum.CaseB,
                    Number = 1.5m,
                    String = "BBBB"
                },
            },
            Count = 2
        }, opt => opt.WithStrictOrdering());
    }


    [Fact]
    public async Task NoOptions()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder();

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Count.Should().Be(6);
        actual.Data.Should().HaveCount(6);
    }

    [Fact]
    public async Task DifferentProjectionKeys()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("somethingElse__end", "1");

        var actual = await _db.TestEntities
            .Select(e => new
            {
                SomethingElse = e.String + "_" + e.Number
            })
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<dynamic>()
        {
            Data = new List<dynamic>()
            {
                new
                {
                    SomethingElse = "AAAA_1"
                },
                new
                {
                    SomethingElse = "AABB_1.1"
                },
            },
            Count = 2
        }, opt => opt.WithStrictOrdering());
    }
}
