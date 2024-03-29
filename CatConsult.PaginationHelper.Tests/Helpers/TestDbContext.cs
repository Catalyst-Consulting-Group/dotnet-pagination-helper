﻿using Microsoft.EntityFrameworkCore;

namespace CatConsult.PaginationHelper.Tests.Helpers;

public class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }
    public TestDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities { get; set; }
}

