using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaginationHelper.Tests.Helpers;

public class TestEntity
{
    [Key]
    public int Id { get; set; }
    public DateTimeOffset? Date { get; set; }
    public decimal? Number { get; set; }
    public string String { get; set; }
    public TestEnum? Enum { get; set; }

    public virtual ICollection<TestNestedEntity> List { get; set; } = new List<TestNestedEntity>();
}


public class TestNestedEntity
{
    [Key]
    public int Id { get; set; }
    public string String { get; set; }

    public virtual TestEntity Entity { get; set; }

}