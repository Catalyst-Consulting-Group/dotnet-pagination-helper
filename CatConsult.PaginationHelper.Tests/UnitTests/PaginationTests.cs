using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using CatConsult.PaginationHelper.Tests.Helpers;
using Xunit;

namespace CatConsult.PaginationHelper.Tests.UnitTests;

[Collection("Database Collection")]
public class PaginationTests
{
    private readonly TestDbContext _db;
    public PaginationTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task First_Page_2_Rows()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("page", "0")
            .Add("rowsPerPage", "2");

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
            },
            Count = 6
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Last_Page_2_Rows()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("page", "2")
            .Add("rowsPerPage", "2");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<TestDto>()
        {
            Data = new List<TestDto>()
            {
                new TestDto()
                {
                    Date = Utilities.CreateDateTime(2001, 4, 15),
                    Enum = TestEnum.CaseC,
                    Number = 200,
                    String = "CCCC",
                    List = new List<string>() { "N2", "A" }
                },
                new()
            },
            Count = 6
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Page_Over()
    {
        // filter by 'string' that start with 'C' or end with 'B'
        // filter by 'date' that is less than 2001/1/1 and greater or equal to 2000/1/1
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("page", "1000")
            .Add("rowsPerPage", "2");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Should().BeEquivalentTo(new PaginateResult<TestDto>()
        {
            Data = new List<TestDto>(),
            Count = 6
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Over_Rows()
    {
        // filter by 'string' that start with 'C' or end with 'B'
        // filter by 'date' that is less than 2001/1/1 and greater or equal to 2000/1/1
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("page", "0")
            .Add("rowsPerPage", "1000");

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Should().HaveCount(6);
        actual.Count.Should().Be(6);
    }
}
