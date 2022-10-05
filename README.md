<a name="readme-top"></a>

[![Deploy to NuGet](https://github.com/Catalyst-Consulting-Group/dotnet-pagination-helper/actions/workflows/deploy.yml/badge.svg)](https://github.com/Catalyst-Consulting-Group/dotnet-pagination-helper/actions/workflows/deploy.yml)

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#pagination-helper">Pagination Helper</a>
    </li>
    <li>
      <a href="#features">Features</a>
    </li>
    <li>
      <a href="#getting-started">Features</a>
    </li>
    <li>
        <a href="#quick-example">Quick Example</a>
    </li>
    <li>
        <a href="#documentations">Documentations</a>
        <ul>
            <li><a href="#query-parameters">Query Parameters</a></li>
            <li><a href="#filters">Filters</a></li>
            <li><a href="#property-types">Property Types</a></li>
            <li><a href="#paginateoptionsbuilder">PaginateOptionsBuilder</a></li>
            <li><a href="#topaginatedasync">ToPaginatedAsync</a></li>
            <li><a href="#ipaginateresult">IPaginateResult</a></li>
        </ul>
    </li>
    <li>
        <a href="#local-development">Local Development</a>
    </li>
  
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

# Pagination Helper

A dotnet entity framework extension class to perform server side table processing (**paginate, sort, search, and filter**) via generic options. The extension method extend on top of EntityFramework IQueryable. Try this out if you're tired of duplicated dotnet server side processing boilerplate code!

# Features

- Dynamically paginate, sort, filter, and search data
- Easy to use on top of existing EF code
- **Flexible**. Lots of built-in options to perform general filters
- **Fast**. No in-memory operation, everything translated into SQL
- **Secured**. Built with Dynamic LinQ, say no to SQL Injection

# Getting Started

TODO: Add Nuget link

# Quick Example

> <i>For more examples, checkout [integration tests class](./PaginationHelper.Tests/IntegrationTests.cs)</i>

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

<table>
    <thead>
        <tr>
            <th>Keyword</th>
            <th>Filter Type</th>
            <th>Applicable Types</th>
            <th>Description</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td valign="top">
                __in
            </td>
            <td valign="top">
                Contains
            </td>
            <td valign="top">
                <b>String</b> and <b>List</b> type only
            </td>
            <td valign="top">
                item value contains filter value
                <ul>
                    <li>
                        Default filter type for <b>String</b> and <b>List</b> type
                    </li>
                    <li>
                        <b>String</b> type contains is case-insensitive
                    </li>
                    <li>
                        <b>List</b> type contains is case-sensitive 
                    </li>
                </ul>
            </td>
        </tr>
        <tr>
            <td valign="top">
                __eq
            </td>
            <td valign="top">
                Equal
            </td>
            <td valign="top">
                <b>All</b> except <b>List / Object</b> type
            </td>
            <td valign="top">
                filter value == item value
                <ul>
                    <li>
                        Default filter type for others except <b>String</b> and <b>List</b> type
                    </li>
                    <li>
                        <b>String</b> type equal is case-insensitive
                    </li>
                    <li>
                        <b>Date</b> type only compare <b>Date</b> part. If need to narrow down by time, use greater/less than filter keyword 
                    </li>
                </ul>
            </td>
        </tr>
        <tr>
            <td valign="top">
                __gt
            </td>
            <td valign="top">
                Greater Than
            </td>
            <td valign="top">
                <b>All</b>, except <b>List / Object</b> type
            </td>
            <td valign="top">
                filter value > item value
            </td>
        </tr>
        <tr>
            <td valign="top">
                __gte
            </td>
            <td valign="top">
                Greater Than or Equal to
            </td>
            <td valign="top">
                <b>All</b>, except <b>List / Object</b> type
            </td>
            <td valign="top">
                filter value >= item value
            </td>
        </tr>
        <tr>
            <td valign="top">
                __lt
            </td>
            <td valign="top">
                Less Than
            </td>
            <td valign="top">
                <b>All</b>, except <b>List / Object</b> type
            </td>
            <td valign="top">
                filter value < item value
            </td>
        </tr>
        <tr>
            <td valign="top">
                __gt
            </td>
            <td valign="top">
                Less Than or Equal to
            </td>
            <td valign="top">
                <b>All</b>, except <b>List / Object</b> type
            </td>
            <td valign="top">
                filter value <= item value
            </td>
        </tr>
        <tr>
            <td valign="top">
                __start
            </td>
            <td valign="top">
                Starts With
            </td>
            <td valign="top">
                <b>String</b> type only
            </td>
            <td valign="top">
                item value starts with filter value
            </td>
        </tr>
        <tr>
            <td valign="top">
                __end
            </td>
            <td valign="top">
                Ends With
            </td>
            <td valign="top">
                <b>String</b> type only
            </td>
            <td valign="top">
                item value ends with filter value
            </td>
        </tr>
    </tbody>
</table>

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
