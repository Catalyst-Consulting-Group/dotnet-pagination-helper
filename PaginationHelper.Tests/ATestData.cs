using PaginationHelper.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PaginationHelper.Tests
{
    internal class ATestData
    {
        public static Expression<Func<TestEntity, TestDto>> Projection = entity => new TestDto()
        {
            Date = entity.Date,
            Enum = entity.Enum,
            Number = entity.Number,
            String = entity.String,
            List = entity.List.Select(e => e.String)
        };

        public static IEnumerable<TestEntity> SeedTestEntities()
        {
            return new List<TestEntity>()
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
                    Date = Utilities.CreateDateTime(2000, 2, 15),
                    Enum = TestEnum.CaseB,
                    Number = 1.5m,
                    String = "BBBB"
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2000, 3, 15),
                    Enum = TestEnum.CaseC,
                    Number = 100,
                    String = "ABCD",
                    List = new List<TestNestedEntity>()
                    {
                        new()
                        {
                            String = "N1"
                        },
                        new()
                        {
                            String = "N2"
                        }
                    }
                },
                new()
                {
                    Date = Utilities.CreateDateTime(2001, 4, 15),
                    Enum = TestEnum.CaseC,
                    Number = 200,
                    String = "CCCC",
                    List= new List<TestNestedEntity>()
                    {
                        new()
                        {
                            String = "N2"
                        },
                        new()
                        {
                            String = "A"
                        }
                    }
                },
                new(),
            };
        }
    }
}
