using System;
using System.Collections.Generic;

namespace CatConsult.PaginationHelper.Tests.Helpers;

public class TestDto
{
    public DateTimeOffset? Date { get; set; }
    public decimal? Number { get; set; }
    public string String { get; set; }
    public TestEnum? Enum { get; set; }
    public IEnumerable<string> List { get; set; } = new List<string>();
}
