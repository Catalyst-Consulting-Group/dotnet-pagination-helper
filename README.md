<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->

<a name="readme-top"></a>

<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

[![Contributors][contributors-shield]][contributors-url]
[![Issues][issues-shield]][issues-url]

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
        <a href="#config-options">Config Options</a>
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

- Dynamic paginate, sort, filter, and search options
- Easy to use on top of existing EF code
- **Flexible**. Lots of built-in options to perform general filters
- **Fast**. No in-memory operation, everything translated into SQL
- **Short**. Just one line of extension method
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

^^^ the above example request will return all rows with `StrCol` contains text `hello`, order by `DateCol` in `descending` order

```
HTTP GET /paginated?page=1&rowsPerPage=10&strCol=filter me&dateCol__gte=2000-1-1
```

^^^ the above example request will return

- second page
- 10 rows per page
- Any rows that contains `something` text in `StrCol` or `OtherCol`
- `DateCol` greater than or equal to `2000-1-1` date
- `StrCol` contains `filter me` text

# Config Options

Todo

# Local Development

Todo

[contributors-shield]: https://img.shields.io/github/contributors/othneildrew/Best-README-Template.svg?style=for-the-badge
[contributors-url]: https://github.com/Catalyst-Consulting-Group/dotnet-pagination-helper/graphs/contributors
[issues-shield]: https://img.shields.io/github/issues/othneildrew/Best-README-Template.svg?style=for-the-badge
[issues-url]: https://github.com/Catalyst-Consulting-Group/dotnet-pagination-helper/issues
