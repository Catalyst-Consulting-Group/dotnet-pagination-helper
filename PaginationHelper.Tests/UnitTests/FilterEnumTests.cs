using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PaginationHelper.Tests.Helpers;
using Xunit;

namespace PaginationHelper.Tests.UnitTests;

[Collection("Database Collection")]
public class FilterEnumTests
{
    private readonly TestDbContext _db;
    public FilterEnumTests()
    {
        _db = new TestDbContextFixture().Context;
    }

    [Fact]
    public async Task Filter_Enum_Default()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Enum", ((int)TestEnum.CaseA).ToString());

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Enum).Should().BeEquivalentTo(new List<TestEnum>()
        {
           TestEnum.CaseA
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Enum_Equal()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Enum__eq", ((int)TestEnum.CaseB).ToString());

        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Enum).Should().BeEquivalentTo(new List<TestEnum>()
        {
           TestEnum.CaseB, TestEnum.CaseB
        }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task Filter_Enum_Multiple()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("Enum", ((int)TestEnum.CaseA).ToString(), ((int)TestEnum.CaseB).ToString());


        var actual = await _db.TestEntities
            .Select(ATestData.Projection)
            .ToPaginatedAsync(paginateOptionBuilder);

        actual.Data.Select(d => d.Enum).Should().BeEquivalentTo(new List<TestEnum>()
        {
            TestEnum.CaseA, TestEnum.CaseB, TestEnum.CaseB
        }, opt => opt.WithStrictOrdering());
    }
}
