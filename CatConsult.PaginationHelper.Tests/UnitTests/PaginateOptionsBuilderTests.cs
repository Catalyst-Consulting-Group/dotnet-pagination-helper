using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CatConsult.PaginationHelper.Tests.UnitTests;

public class PaginateOptionsBuilderTests
{
    [Fact]
    public void BuildNone()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder();
        var options = paginateOptionBuilder.Build();
        options.Should().BeEquivalentTo(new PaginateOptions());
    }

    [Fact]
    public void BuildRemoved()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder();
        var options = paginateOptionBuilder
            .Add("orderBy" , "t")
            .Add("orderDirection" , "asc")
            .Remove("orderBy", "orderDirection")
            .Build();
        options.Should().BeEquivalentTo(new PaginateOptions());
    }

    [Fact]
    public void BuildAll()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("page", "1")
            .Add("rowsPerPage", "10")
            .Add("orderBy", "tOrderBy")
            .Add("orderDirection", "asc")
            .Add("search", "tSearch")
            .Add("columns", "col1", "col2", "col3")
            .Add("s", "s1", "s2")
            .Add("s__in", "s3")
            .Add("s__eq", "s4")
            .Add("s__start", "s5")
            .Add("s__end", "s6")
            .Add("n__lt", "1")
            .Add("n__gt", "2")
            .Add("d__lte", "3")
            .Add("d2__gte", "4");

        var options = paginateOptionBuilder.Build();

        options.Should().BeEquivalentTo(new PaginateOptions()
        {
            Page = 1,
            RowsPerPage = 10,
            OrderBy = "tOrderBy",
            OrderDirection = "asc",
            Search = "tSearch",
            Columns = new HashSet<string>() { "col1", "col2", "col3" },
            Filters = new Dictionary<string, List<PaginateFilterValue>>()
            {
                { "s", new()
                    {
                        new PaginateFilterValue()
                        {
                            Value = "s1"
                        },
                        new PaginateFilterValue()
                        {
                            Value = "s2"
                        },
                        new PaginateFilterValue()
                        {
                            Value = "s3",
                            FilterType = PaginateFilterType.In
                        },
                        new PaginateFilterValue()
                        {
                            Value = "s4",
                            FilterType = PaginateFilterType.Equal
                        },
                        new PaginateFilterValue()
                        {
                            Value = "s5",
                            FilterType = PaginateFilterType.StartWith
                        },
                        new PaginateFilterValue()
                        {
                            Value = "s6",
                            FilterType = PaginateFilterType.EndWith
                        },
                    }
                },
                { "n", new()
                    {
                        new PaginateFilterValue()
                        {
                            Value = "1",
                            FilterType = PaginateFilterType.LessThan
                        },
                        new PaginateFilterValue()
                        {
                            Value = "2",
                            FilterType = PaginateFilterType.GreaterThan
                        },
                    }
                },
                { "d", new()
                    {
                        new PaginateFilterValue()
                        {
                            Value = "3",
                            FilterType = PaginateFilterType.LessThanOrEqual
                        },
                    }
                },
                { "d2", new()
                    {
                        new PaginateFilterValue()
                        {
                            Value = "4",
                            FilterType = PaginateFilterType.GreaterThanOrEqual
                        },
                    }
                }
            }
        });
    }

    [Fact]
    public void Excluding()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("columns", "col1", "col2", "col3")
            .Add("col1", "a")
            .Add("col2__eq", "b")
            .Add("col3", "c");

        var options = paginateOptionBuilder
            .ExcludeColumns("col1", "col2")
            .Build();

        options.Should().BeEquivalentTo(new PaginateOptions()
        {
            Columns = new HashSet<string>() { "col3" },
            Filters = new Dictionary<string, List<PaginateFilterValue>>()
            {
                {
                    "col3",
                    new()
                    {
                        new PaginateFilterValue()
                        {
                            Value = "c"
                        },
                    }
                },
            }
        });
    }

    [Fact]
    public void Including()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("columns", "col1", "col2", "col3")
            .Add("col1", "a")
            .Add("col2__eq", "b")
            .Add("col3", "c");

        var options = paginateOptionBuilder
            .IncludeColumns("col3")
            .Build();

        options.Should().BeEquivalentTo(new PaginateOptions()
        {
            Columns = new HashSet<string>() { "col3" },
            Filters = new Dictionary<string, List<PaginateFilterValue>>()
            {
                {
                    "col3",
                    new()
                    {
                        new PaginateFilterValue()
                        {
                            Value = "c"
                        },
                    }
                },
            }
        });
    }  
    
    [Fact]
    public void Multiple_With_Square_Bracket()
    {
        var paginateOptionBuilder = new PaginateOptionsBuilder()
            .Add("columns", "col1", "col2")
            .Add("columns[]", "col3", "col4")
            .Add("columns[5]", "col5")
            .Add("t[0]", "t1")
            .Add("t[1]", "t2")
            .Add("x[]", "x1")
            .Add("x[]", "x2");

        var options = paginateOptionBuilder
          .Build();

        options.Should().BeEquivalentTo(new PaginateOptions()
        {
            Columns = new HashSet<string>() { "col1", "col2", "col3", "col4", "col5" },
            Filters = new Dictionary<string, List<PaginateFilterValue>>()
            {
                {
                    "t",
                    new()
                    {
                        new PaginateFilterValue()
                        {
                            Value = "t1"
                        }, 
                        new PaginateFilterValue()
                        {
                            Value = "t2"
                        },
                    }
                }, 
                {
                    "x",
                    new()
                    {
                        new PaginateFilterValue()
                        {
                            Value = "x1"
                        }, 
                        new PaginateFilterValue()
                        {
                            Value = "x2"
                        },
                    }
                },
            }
        });
    }
}
