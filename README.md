# PGroonga.EntityFrameworkCore

PGroonga extension for Npgsql.EntityFrameworkCore.PostgreSQL

[![AppVeyor](https://img.shields.io/appveyor/ci/JoyMoe/pgroonga-entityframeworkcore.svg)](https://ci.appveyor.com/project/JoyMoe/pgroonga-entityframeworkcore)
[![license](https://img.shields.io/github/license/JoyMoe/Base62.Net.svg)](https://github.com/JoyMoe/Base62.Net/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PGroonga.EntityFrameworkCore.svg)](https://www.nuget.org/packages/PGroonga.EntityFrameworkCore)
[![NuGet](https://img.shields.io/nuget/vpre/PGroonga.EntityFrameworkCore.svg)](https://www.nuget.org/packages/PGroonga.EntityFrameworkCore/absoluteLatest)
![netstandard2.0](https://img.shields.io/badge/.Net-netstandard2.0-brightgreen.svg)

## Attention

`EF.Functions.Pgroonga*` won't work due to [aspnet/EntityFrameworkCore#13454](https://github.com/aspnet/EntityFrameworkCore/issues/13454).

## Usage

```csharp
public class ApplicationDbContext : DbContext
{
    // ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ...

        modelBuilder.Entity<Post>()
            .HasIndex(g => g.Content)
            .ForNpgsqlHasMethod("pgroonga")
            .ForNpgsqlHasOperators("pgroonga_text_full_text_search_ops_v2");

    }
}

public class Startup
{
    // ...

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
            builder => builder.UsePGroonga()));

        // ...
    }
}

var posts = await _dbContext.Posts.Where(g => g.Content.SimilarSearch(q))
    .OrderByDescending(g => EF.Functions.PgroongaScore())
    .ToArrayAsync();
```

For more information, see `PGroongaLinqExtensions` and `PGroongaDbFunctionsExtensions`.

## Features

* [x] Operators
  * [x] ~`LIKE`~ same as PostgreSQL standard `LIKE`
  * [x] ~`ILIKE`~ same as PostgreSQL standard `ILIKE`
  * [x] ==== pgroonga_xxx_full_text_search_ops[_v2] ====
  * [x] `&@`
  * [x] `&@~`
  * [x] `&@*`
  * [x] `` &` ``
  * [x] `&@|`
  * [x] `&@~|`
  * [x] ==== pgroonga_xxx_term_search_ops[_v2] ====
  * [x] `&^`
  * [x] `&^~`
  * [x] `&^|`
  * [x] `&^~|`
  * [x] ==== pgroonga_xxx_regexp_ops[_v2] ====
  * [x] `&~`
* [x] Functions
  * [x] `pgroonga_command`
  * [x] `pgroonga_command_escape_value`
  * [x] `pgroonga_escape`
  * [x] `pgroonga_flush`
  * [x] `pgroonga_highlight_html`
  * [x] `pgroonga_is_writable`
  * [x] `pgroonga_match_positions_byte`
  * [x] `pgroonga_match_positions_character`
  * [x] `pgroonga_normalize`
  * [x] `pgroonga_query_escape`
  * [x] `pgroonga_query_expand`
  * [x] `pgroonga_query_extract_keywords`
  * [x] `pgroonga_set_writable`
  * [x] `pgroonga_score`
  * [x] `pgroonga_snippet_html`
  * [x] `pgroonga_table_name`
  * [x] `pgroonga_wal_apply`
  * [x] `pgroonga_wal_truncate`

## License

The MIT License

More info see [LICENSE](LICENSE)
