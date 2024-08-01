![Test Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/wztech0192/33a4dd1f54e8b9cf42cb49328e0c282a/raw/code-coverage.json)
[![NuGet stable version](https://badgen.net/nuget/v/CatConsult.PaginationHelper)](https://nuget.org/packages/CatConsult.PaginationHelper)

<!-- TABLE OF CONTENTS -->

- [Pagination Helper](#pagination-helper)
- [Features](#features)
- [Getting Started](#getting-started)
- [Quick Example](#quick-example)
- [Documentation](#documentation)
  - [Query Parameters](#query-parameters)
  - [Filters](#filters)
  - [Property Types](#property-types)
  - [PaginateOptionsBuilder](#paginateoptionsbuilder)
  - [ToPaginatedAsync](#topaginatedasync)
  - [IPaginateResult](#ipaginateresult)
- [Local Development](#local-development)

# Pagination Helper

A .NET Entity Framework extension class to dynamically perform server-side data processing (**paging, sorting, searching, and filtering**). This extension method is built on top of EntityFramework's IQueryable type. Try it out if you're tired of duplicating server-side processing boilerplate code!

# Features

- Dynamically paginate, sort, filter, and search data
- Easy to use on top of existing EF code
- **Flexible**: Lots of built-in options to perform general filters
- **Fast**: No in-memory operation, everything is translated into SQL
- **Secure**: Built with Dynamic LinQ, protecting against SQL Injection

# Getting Started

1. Install NuGet Package:

   ```ps
   > dotnet add package CatConsult.PaginationHelper
   ```

2. Import Package:

   ```csharp
   using CatConsult.PaginationHelper;
   ```

3. Use ToPaginatedAsync:

   ```csharp
   DbContext.FooEntities.ToPaginatedAsync(paginateOptionsBuilder)
   ```

# Quick Example

_For more examples, check out the [integration tests class](./CatConsult.PaginationHelper.Tests/IntegrationTests.cs)._

## ASP.Net API Project

```csharp
...

// Example entity
public class FooEntity
{
    public string StrCol { get; set; }
    public string OtherCol { get; set; }
    public DateTimeOffset DateCol { get; set; }
}

...
using CatConsult.PaginationHelper;
...

// A random controller method
public async Task<IPaginateResult<FooDto>> GetPaginatedData([FromQuery] PaginateOptionsBuilder paginateOptionsBuilder)
{
    return await _db.FooEntities
        .Where(...) // Pre-filter data if needed
        .Select(...) // Recommend projecting into a DTO first, better for performance
        .ToPaginatedAsync(paginateOptionsBuilder);
}
...
```

## Returned Data Type (IPaginateResult)

```typescript
{
  data: any[]; // List of paginated data
  count: number; // Total matched records in the database
  currentPage: number;
  rowsPerPage: number;
  totalPages: number;
  previousPage: number | null; // Null if no previous page
  nextPage: number | null; // Null if no next page
}
```

## Example HTTP Requests

```
HTTP GET /paginated?order=dateCol&orderDirection=desc&strCol__eq=hello
```

The above example request will return all rows where `StrCol` equals `"hello"`, ordered by `DateCol` in descending order.

```
HTTP GET /paginated?page=1&rowsPerPage=10&strCol=filter me&dateCol__gte=2000-1-1
```

The above example request will return the second page, with 10 rows per page, where rows contain `"filter me"` in `StrCol` and `DateCol` is greater than or equal to `2000-1-1`.

# Documentation

## Query Parameters

| Name           | Description                                                                                             |
| -------------- | ------------------------------------------------------------------------------------------------------- |
| page           | Page number starting from 0                                                                             |
| rowsPerPage    | Maximum rows to return per page                                                                         |
| orderBy        | The property name of the item to order/sort by                                                          |
| orderDirection | Order/sort direction. Supports `asc` or `desc`                                                          |
| search         | Search value. Returns items with matching `columns` values                                              |
| columns        | Columns to search in. \*Required if `search` is provided                                                |
| other keys     | All other keys will be treated as `filters`. Check out the [Filters Section](#filters) for more details |

## Filters

Built-in filter keywords provide more flexible filtering. Filter keywords can be appended to any filter key and are case-insensitive. All filter keywords start with `__XX`. For example, to filter for any `strCol` starting with `"A"`, use `strCol__start=A` instead of `strCol=A`.

| Keyword   | Filter Type              | Applicable Property Types              | Description                           | Note                                                                                                                                                                                                                |
| --------- | ------------------------ | -------------------------------------- | ------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| \_\_in    | Contains                 | `String` and `List` type only          | Item value `contains` filter value    | Default filter type for `String` and `List` types.<br/> `String` type contains is case-insensitive.<br/> `List` type contains is case-sensitive                                                                     |
| \_\_eq    | Equal                    | `All` except `List / Object` type      | Filter value `==` item value          | Default filter type for others except `String` and `List` types<br/> `String` type equal is case-insensitive<br/> `Date` type compares only `Date` part. For time comparison, use greater/less than filter keywords |
| \_\_gt    | Greater Than             | `All`, except `List / Object` type     | Filter value `>` item value           |
| \_\_gte   | Greater Than or Equal to | `All`, except `List / Object` type     | Filter value `>=` item value          |
| \_\_lt    | Less Than or Equal to    | `All`, except `List / Object` type     | Filter value `<` item value           |
| \_\_lte   | Less Than or Equal to    | `All`, except `List / Object` type     | Filter value `<=` item value          |
| \_\_start | Starts With              | `String` or `List of string` type only | Item value `starts with` filter value |
| \_\_end   | Ends With                | `String` or `List of string` type only | Item value `ends with` filter value   |

All filter keys accept multiple values.

- Range keywords (`gt`, `gte`, `lt`, and `lte`) join using `&&` operation.
- All other keywords join using `||` operation.

For example, `str=A&str=B&num_gte=1&num_lt=10` translates into `(str == A || str == B) && (num >= 1 && num < 10)`.

## Property Types

Filtering behavior differs based on the item property type.

- `String` and `List` types use `Contains` filter by default. For example, `str=A` translates into `str contains 'A'`.
- Other types use `Equal` filter by default. For example, `number=1` translates into `number equals 1`.

When performing a `search`, all types use their default filter type.

## PaginateOptionsBuilder

A helper class to build paginate options. The class can be used to bind query params in the controller.

```csharp
PaginateOptionsBuilder builder = new PaginateOptionsBuilder();

// To add new keys
builder
    .Add("orderBy", "date")
    .Add("orderDirection", "desc")
    .Add("strCol", "A", "B", "C")

// To exclude some columns from search or filter
builder.ExcludeColumns("strCol", "fooCol")

// To only include defined columns for search or filter
builder.IncludeColumns("strCol", "fooCol")

// Override default filters by type
builder.OverrideDefaultFilterType(FilterPropertyType.String, FilterPropertyType.StartWith);
```

## ToPaginatedAsync

Extension method of `IQueryable<T>`. Returns `IPaginateResult<T>`.

```csharp
// First parameter takes PaginateOptionsBuilder
// Optional second parameter to transform IQueryable after applying filter, paginate, and sort
DbContext.Entities.ToPaginated(paginateOptionsBuilder, paginatedQuery => paginatedQuery.Where(c => c.FooBool))
```

## IPaginateResult

```csharp
public interface IPaginateResult<T>
{
    IEnumerable<T> Data { get; set; } // Paginated and filtered item list
    int Count { get; set; } // Total matched records in the database
    int CurrentPage { get; set; }
    int RowsPerPage { get; set; }
    int TotalPages { get; }
    int? PreviousPage { get; }
    int? NextPage { get; }
}
```

# Local Development

## Prerequisites

- Windows or macOS
- .NET >6
- Docker Desktop
- Editor/IDE that supports C#

## Running the Tests

Use the `dotnet test` command in the project directory. The tests will run against a real PostgreSQL database using Docker Fixture.
