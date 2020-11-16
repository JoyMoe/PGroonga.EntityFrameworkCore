using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PGroongaTests.Supports;
using Xunit;

namespace PGroongaTests
{
    public class PGroongaQueryTest : IClassFixture<PGroongaFixture>
    {
        public PGroongaQueryTest(PGroongaFixture fixture)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            Fixture.TestSqlLoggerFactory.Clear();
        }

        PGroongaFixture Fixture { get; }

        #region Functions

        [Fact]
        public void FunctionPgroongaCommand()
        {
            using var ctx = CreateContext();

            var rows = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaCommand("status"))
                .Single();
            Assert.Contains("uptime", rows, StringComparison.InvariantCulture);
            Assert.Contains(@"SELECT pgroonga_command('status')", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaCommandEscapeValue()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaCommandEscapeValue("(PostgreSQL"))
                .Single();
            Assert.Equal("\"(PostgreSQL\"", row);
            Assert.Contains(@"SELECT pgroonga_command_escape_value('(PostgreSQL')", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaEscape()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaEscape(100))
                .Single();
            Assert.Equal("100", row);
            Assert.Contains(@"SELECT pgroonga_escape(100)", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaFlush()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaFlush("ix_pgroongatypes_id_content"))
                .Single();
            Assert.True(row);
            Assert.Contains(@"SELECT pgroonga_flush('ix_pgroongatypes_id_content')", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaHighlightHtml()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaHighlightHtml("PGroonga is a PostgreSQL extension.", new[] {"PostgreSQL"}))
                .Single();
            Assert.Contains("<span class=\"keyword\">PostgreSQL</span>", row, StringComparison.InvariantCulture);
            Assert.Contains(@"SELECT pgroonga_highlight_html('PGroonga is a PostgreSQL extension.', ARRAY['PostgreSQL']", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaIsWritable()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaIsWritable())
                .Single();
            Assert.True(row);
            Assert.Contains(@"SELECT pgroonga_is_writable()", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaMatchPositionsByte()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaMatchPositionsByte("PGroonga is a PostgreSQL extension.", new[] { "PostgreSQL" }))
                .Single();
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
            Assert.Equal(new[,] { { 14, 10 } }, row);
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional
            Assert.Contains(@"SELECT pgroonga_match_positions_byte('PGroonga is a PostgreSQL extension.', ARRAY['PostgreSQL']", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaMatchPositionsCharacter()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaMatchPositionsCharacter("PGroonga is a PostgreSQL extension.", new[] { "PostgreSQL" }))
                .Single();
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
            Assert.Equal(new[,] { { 14, 10 } }, row);
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional
            Assert.Contains(@"SELECT pgroonga_match_positions_character('PGroonga is a PostgreSQL extension.', ARRAY['PostgreSQL']", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaNormalize()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes.Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaNormalize("aBcDe 123")).Single();
            Assert.Equal("abcde 123", row);
            Assert.Contains(@"SELECT pgroonga_normalize('aBcDe 123')", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaQueryEscape()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaQueryEscape("(PostgreSQL"))
                .Single();
            Assert.Equal("\\(PostgreSQL", row);
            Assert.Contains(@"SELECT pgroonga_query_escape('(PostgreSQL')", Sql, StringComparison.InvariantCulture);
        }

        /*
        [Fact]
        public void FunctionPgroongaQueryExpand()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaQueryExpand("synonyms", "term", "synonyms", "PGroonga OR Mroonga"))
                .Single();
            Assert.Equal("((PGroonga) OR (Groonga PostgreSQL)) OR Mroonga", row);
            Assert.Contains(@"SELECT pgroonga_query_expand('synonyms', 'term', 'synonyms',", Sql);
        }
        */

        [Fact]
        public void FunctionPgroongaQueryExtractKeywords()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaQueryExtractKeywords("Groonga PostgreSQL"))
                .Single();
            Assert.Equal(new[] { "PostgreSQL", "Groonga" }, row);
            Assert.Contains(@"SELECT pgroonga_query_extract_keywords('Groonga PostgreSQL')", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaSetWritable()
        {
            using var ctx = CreateContext();
            var row1 = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaSetWritable(false))
                .Single();
            Assert.True(row1);
            Assert.Contains(@"SELECT pgroonga_set_writable(FALSE)", Sql, StringComparison.InvariantCulture);

            var row2 = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaSetWritable(true))
                .Single();
            Assert.False(row2);
            Assert.Contains(@"SELECT pgroonga_set_writable(TRUE)", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaScore()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(r => r.Content.Match("engine"))
                .Select(r => EF.Functions.PgroongaScore())
                .Single();
            Assert.Equal(1, row);
            Assert.Contains(@"SELECT pgroonga_score(tableoid, ctid)", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaSnippetHtml()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(t => t.Id == 1)
                .Select(x => EF.Functions.PgroongaSnippetHtml(
                        @"Groonga is a fast and accurate full text search engine based on
inverted index. One of the characteristics of Groonga is that a
newly registered document instantly appears in search results.
Also, Groonga allows updates without read locks. These characteristics
result in superior performance on real-time applications.
\n
\n
Groonga is also a column-oriented database management system (DBMS).
Compared with well-known row-oriented systems, such as MySQL and
PostgreSQL, column-oriented systems are more suited for aggregate
queries. Due to this advantage, Groonga can cover weakness of
row-oriented systems.", new[] { "fast", "PostgreSQL" }))
                .First();
            Assert.Equal(2, row.Length);
            Assert.Contains(@"SELECT pgroonga_snippet_html(", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void FunctionPgroongaTableName()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(r => r.Content.Match("engine"))
                .Select(r => EF.Functions.PgroongaTableName("ix_pgroongatypes_id_content"))
                .Single();
            Assert.Contains("Sources", row, StringComparison.InvariantCulture);
            Assert.Contains(@"SELECT pgroonga_table_name('ix_pgroongatypes_id_content')", Sql, StringComparison.InvariantCulture);
        }

        /*
        [Fact]
        public void FunctionPgroongaWalApply()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(r => r.Content.Match("engine"))
                .Select(r => EF.Functions.PgroongaWalApply("ix_pgroongatypes_id_content"))
                .Single();
            Assert.Contains(@"SELECT pgroonga_wal_apply('ix_pgroongatypes_id_content')", Sql);
        }

        [Fact]
        public void FunctionPgroongaWalTruncate()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Where(r => r.Content.Match("engine"))
                .Select(r => EF.Functions.PgroongaWalTruncate("ix_pgroongatypes_id_content"))
                .Single();
            Assert.Contains(@"SELECT pgroonga_wal_truncate('ix_pgroongatypes_id_content')", Sql);
        }
        */

        #endregion Functions

        #region Operators pgroonga_text_full_text_search_ops_v2

        [Fact]
        public void OperatorMatchV2()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Single(r => r.Content.Match("engine"));
            Assert.Equal(2, row.Id);
            Assert.Contains(@"""Content"" &@ 'engine'", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void OperatorQueryV2()
        {
            using var ctx = CreateContext();
            var rows = ctx.PGroongaTypes
                .Count(r => r.Content.Query("PGroonga OR PostgreSQL"));
            Assert.Equal(2, rows);
            Assert.Contains(@"""Content"" &@~ 'PGroonga OR PostgreSQL'", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void OperatorSimilarSearchV2()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Single(r => r.Content.SimilarSearch("Mroonga is a MySQL extension taht uses Groonga"));
            Assert.Equal(3, row.Id);
            Assert.Contains(@"""Content"" &@* 'Mroonga is a MySQL extension taht uses Groonga'", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void OperatorScriptQueryV2()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Single(r => r.Content.ScriptQuery("Id >= 2 && (Content @ 'engine' || Content @ 'rdbms')"));
            Assert.Equal(2, row.Id);
            Assert.Contains(@"""Content"" &` 'Id >= 2 && (Content @ ''engine'' || Content @ ''rdbms'')'", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void OperatorMatchInV2()
        {
            using var ctx = CreateContext();
            var rows = ctx.PGroongaTypes
                .Count(r => r.Content.MatchIn(new[] { "engine", "database" }));
            Assert.Equal(2, rows);
            Assert.Contains(@"""Content"" &@| ARRAY['engine','database']::text[]", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void OperatorQueryInV2()
        {
            using var ctx = CreateContext();
            var rows = ctx.PGroongaTypes
                .Count(r => r.Content.QueryIn(new[] { "Groonga engine", "PostgreSQL -PGroonga" }));
            Assert.Equal(2, rows);
            Assert.Contains(@"""Content"" &@~| ARRAY['Groonga engine','PostgreSQL -PGroonga']::text[]", Sql, StringComparison.InvariantCulture);
        }

        #endregion Operators pgroonga_text_full_text_search_ops_v2

        #region Operators pgroonga_text_term_search_ops_v2

        [Fact]
        public void OperatorPrefixSearchV2()
        {
            using var ctx = CreateContext();
            var rows = ctx.PGroongaTypes
                .Count(r => r.Tag.PrefixSearch("pg"));
            Assert.Equal(2, rows);
            Assert.Contains(@"""Tag"" &^ 'pg'", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void OperatorPrefixRkSearchV2()
        {
            using var ctx = CreateContext();
            var rows = ctx.PGroongaTypes
                .Count(r => r.Tag.PrefixRkSearch("pi-ji-"));
            Assert.Equal(2, rows);
            Assert.Contains(@"""Tag"" &^~ 'pi-ji-'", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void OperatorPrefixSearchInV2()
        {
            using var ctx = CreateContext();
            var rows = ctx.PGroongaTypes
                .Count(r => r.Tag.PrefixSearchIn(new[] { "pg", "gro" }));
            Assert.Equal(3, rows);
            Assert.Contains(@"""Tag"" &^| ARRAY['pg','gro']::text[]", Sql, StringComparison.InvariantCulture);
        }

        [Fact]
        public void OperatorPrefixRkSearchInV2()
        {
            using var ctx = CreateContext();
            var rows = ctx.PGroongaTypes
                .Count(r => r.Tag.PrefixRkSearchIn(new[] { "pi-ji-", "posu" }));
            Assert.Equal(4, rows);
            Assert.Contains(@"""Tag"" &^~| ARRAY['pi-ji-','posu']::text[]", Sql, StringComparison.InvariantCulture);
        }

        #endregion Operators pgroonga_text_term_search_ops_v2

        #region Operators pgroonga_text_regexp_ops_v2

        [Fact]
        public void OperatorRegexpMatchV2()
        {
            using var ctx = CreateContext();
            var row = ctx.PGroongaTypes
                .Single(r => r.Content.RegexpMatch("\\Apostgresql"));
            Assert.Equal(1, row.Id);
            Assert.Contains(@"""Content"" &~ '\Apostgresql'", Sql, StringComparison.InvariantCulture);
        }

        #endregion Operators pgroonga_text_term_search_ops_v2

        #region Support

        PGroongaContext CreateContext() => Fixture.CreateContext();

        string Sql => Fixture.TestSqlLoggerFactory.Sql;

        #endregion Support
    }
}
