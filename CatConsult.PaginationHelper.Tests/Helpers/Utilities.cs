using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CatConsult.PaginationHelper.Tests.Helpers
{
    internal class Utilities
    {
        public static DateTimeOffset CreateDateTime(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            return new DateTimeOffset(new DateTime(year, month, day, hour, minute, second)).ToUniversalTime();
        }

    }
}
