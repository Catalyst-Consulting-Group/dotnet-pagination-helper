![Test Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/wztech0192/33a4dd1f54e8b9cf42cb49328e0c282a/raw/code-coverage.json)
[![NuGet stable version](https://badgen.net/nuget/v/CatConsult.PaginationHelper)](https://nuget.org/packages/CatConsult.PaginationHelper)

<!-- TABLE OF CONTENTS -->

- [Pagination Helper](#pagination-helper)
- [Features](#Features)
- [Getting Started](#getting-started)
- [Quick Example](#quick-example)
- [Documentations](#documentations)
  - [Query Parameters](#query-parameters)
  - [Developing](#filters)
  - [Property Types](#property-types)
  - [PaginateOptionsBuilder](#paginateoptionsbuilder)
  - [ToPaginatedAsync](#topaginatedasync)
  - [IPaginateResult](#ipaginateresult)
- [Local Development](#local-development)

# Pagination Helper

A dotnet entity framework extension class to perform server side table processing (**paginate, sort, search, and filter**) via generic options. The extension method extend on top of EntityFramework IQueryable. Try this out if you're tired of duplicated dotnet server side processing boilerplate code!

# Features

- Dynamically paginate, sort, filter, and search data
- Easy to use on top of existing EF code
- **Flexible**. Lots of built-in options to perform general filters
- **Fast**. No in-memory operation, everything translated into SQL
- **Secured**. Built with Dynamic LinQ, say no to SQL Injection

# Getting Started

1. Install NuGet Package

```ps
> dotnet add package CatConsult.PaginationHelper
```

2. Import Package

```C#
using CatConsult.PaginationHelper;
```

3. Use ToPaginateAsync

```C#
DbContext.FooEntities.ToPaginatedAsync(paginateOptionsBuilder)
```

# Quick Example

> _For more examples, checkout [integration tests class](./PaginationHelper.Tests/IntegrationTests.cs)_

## ASP.Net API Project

```C#
...

// example entity
public class FooEntity
{
    public string StrCol { get; set; }
    public string OtherCol { get; set; }
    public DateTimeOffset DateCol { get; set; }
}

...
using CatConsult.PaginationHelper;
...
// A random controller method. Please don't put business logic in controllers when building serious project :D
[HttpGet("paginated")]
public Task<IPaginateResult<FooDto>> GetPaginatedData([FromQuery] PaginateOptionsBuilder paginateOptionsBuilder)
{
    return await _db.FooEntities
        .Where(...) // pre filter data if needed
        .Select(...)// recommend project into a dto first, better for the performance.
        .ToPaginatedAsync(paginateOptionsBuilder);
}
...
```

## Returned Data Type (IPaginateResult)

```ts
{
  data: any[]; // list of paginated data
  count: number; // total matched records in the database
}
```

## Example HTTP Requests

```
HTTP GET /paginated?order=dateCol&orderDirection=desk&strCol__eq=hello
```

^^^ the above example request will return all rows with `StrCol` contains `"hello"`, order by `DateCol` in `descending` order.

```
HTTP GET /paginated?page=1&rowsPerPage=10&strCol=filter me&dateCol__gte=2000-1-1
```

^^^ the above example request will return `second` page, `10` rows per page, rows that contains `"something"` in `StrCol` or `OtherCol`, `StrCol` contains `"filter me"`, `DateCol` greater than or equal to `2000-1-1` date,

# Documentations

## Query Parameters

| Name           | Description                                                                                      |
| -------------- | ------------------------------------------------------------------------------------------------ |
| page           | Page number start with 0                                                                         |
| rowsPerPage    | Maximum rows to return for a page                                                                |
| orderBy        | The property name of the item to order/sort by                                                   |
| orderDirection | order/sort by direction. Support `asc` or `desc`                                                 |
| search         | Search value. Return items with matching `columns` values                                        |
| columns        | Columns to search for. \*Required if provide `search`                                            |
| other keys     | All other keys will be treated as `filter`. Checkout [Filters Section](#filters) for more detail |

## Filters

We included some built-in filter keywords to provide more flexible filtering. Filter keyword can be append to any filter key and they are case-insensitive. All filter keywords start with `two underscore, __XX`. For example, if you want any strCol start with `"A"`, instead of `strCol=A` you will do `strCol__start=A`.

| Keyword   | Filter Type              | Applicable Property Types          | Description                           | Note                                                                                                                                                                                                                       |
| --------- | ------------------------ | ---------------------------------- | ------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| \_\_in    | Contains                 | `String` and `List` type only      | item value `contains` filter value    | Default filter type for `String` and `List` type. <br/> `String` type contains is case-insensitive.<br/> `String` and `List` type only `List` type contains is case-sensitive                                              |
| \_\_eq    | Equal                    | `All` except `List / Object` type  | filter value `==` item value          | Default filter type for others except `String` and `List` type<br/> `String` type equal is case-insensitive<br/>`Date` type only compare `Date` part. If need to narrow down by time, use greater/less than filter keyword |
| \_\_gt    | Greater Than             | `All`, except `List / Object` type | filter value `>` item value           |
| \_\_gte   | Greater Than or Equal to | `All`, except `List / Object` type | filter value `>=` item value          |
| \_\_lt    | Less Than or Equal to    | `All`, except `List / Object` type | filter value `<` item value           |
| \_\_lte   | Less Than or Equal to    | `All`, except `List / Object` type | filter value `<=` item value          |
| \_\_start | Starts With              | `String` type only                 | item value `starts with` filter value |
| \_\_end   | Ends With                | `String` type only                 | item value `ends with` filter value   |

All filter keys accept multiple values.

- Range keywords (`gt`, `gte`, `lt`, and `lte`) will join using `&&` operation.
- All other keywords will join using `||` operation.

For example, `str=A&str=B&num_gte=1&num_lt=10` will translate into `(str == A || str == B) || (num >= 1 && num < 10)`

## Property Types

Some filtering behavior differs based on the item property type.

- `String` and `List` type will use `Contains` filter by default. For example, `str=A` will translate into `str contains 'A'`
- Other types will use `Equal` filter by default. For example, `number=1` will translate into `number equal 1`

When perform `search`, all types will use their default filter type.

## PaginateOptionsBuilder

A helper class to build paginate option. The class can be use to bind query params in the controller.

```c#
PaginateOptionsBuilder builder = new PaginateOptionsBuilder();

// to add new keys
builder
    .Add("orderBy", "date")
    .Add("orderDirection", "desc")
    .Add("strCol", "A", "B", "C")

// to exclude some column from search or filter
builder.ExcludeColumns("strCol", "fooCol")

// to only include defined column for search or filter
builder.IncludeColumns("strCol", "fooCol")
```

## ToPaginatedAsync

Extension method of `IQueryable<T>`. Will return `IPaginateResult<T>`

```c#

// first parameter take PaginateOptionsBuiler
// optional second parameter to transform IQueryable after apply filter, paginate, and sort
DbContext.Entities.ToPaginated(paginateOptionsBuilder, paginatedQuery => paginatedQuery.Where(c => c.FooBool))
```

## IPaginateResult

```c#
public interface IPaginateResult<T>
{
    IEnumerable<T> Data { get; set; } // paginated and filtered item list
    int Count { get; set; } // total matched records in the database
}
```

# Local Development

## Prerequisites

- Windows or macOS
- .NET 6
- Docker Desktop
- Editor/IDE that support C#

## Running the Test

Use `dotnet test` command in project directory. The test will be running against real PostgresSQL database using Docker Fixture.
