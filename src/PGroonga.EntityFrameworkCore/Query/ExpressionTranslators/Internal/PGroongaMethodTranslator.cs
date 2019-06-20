using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal
{
    /// <summary>
    /// Provides translations for PGroonga full-text search methods.
    /// </summary>
    public class PGroongaMethodTranslator : IMethodCallTranslator
    {
        static readonly IReadOnlyDictionary<string, string> SqlNameByMethodName = new Dictionary<string, string>
        {
            [nameof(PGroongaDbFunctionsExtensions.PgroongaCommand)] = "pgroonga_command",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaCommandEscapeValue)] = "pgroonga_command_escape_value",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaEscape)] = "pgroonga_escape",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaFlush)] = "pgroonga_flush",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaHighlightHtml)] = "pgroonga_highlight_html",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaIsWritable)] = "pgroonga_is_writable",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaMatchPositionsByte)] = "pgroonga_match_positions_byte",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaMatchPositionsCharacter)] = "pgroonga_match_positions_character",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaNormalize)] = "pgroonga_normalize",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaQueryEscape)] = "pgroonga_query_escape",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaQueryExpand)] = "pgroonga_query_expand",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaQueryExtractKeywords)] = "pgroonga_query_extract_keywords",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaSetWritable)] = "pgroonga_set_writable",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaScore)] = "pgroonga_score",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaSnippetHtml)] = "pgroonga_snippet_html",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaTableName)] = "pgroonga_table_name",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaWalApply)] = "pgroonga_wal_apply",
            [nameof(PGroongaDbFunctionsExtensions.PgroongaWalTruncate)] = "pgroonga_wal_truncate"
        };

        /// <inheritdoc />
        [CanBeNull]
        public Expression Translate(MethodCallExpression e)
        {
            if (e.Method.DeclaringType != typeof(PGroongaDbFunctionsExtensions) &&
                e.Method.DeclaringType != typeof(PGroongaLinqExtensions))
                return null;

            if (!SqlNameByMethodName.TryGetValue(e.Method.Name, out var sqlFunctionName))
                return TryTranslateOperator(e);

            if (sqlFunctionName != "pgroonga_score")
                return new SqlFunctionExpression(sqlFunctionName, e.Method.ReturnType, e.Arguments.Skip(1));

            // hack for pgroonga_score
            return new SqlFunctionExpression(sqlFunctionName, e.Method.ReturnType, new[]
            {
                new SqlFragmentExpression("tableoid"),
                new SqlFragmentExpression("ctid")
            });
        }

        [CanBeNull]
        static Expression TryTranslateOperator([NotNull] MethodCallExpression e)
        {
            if (e.Method.DeclaringType != typeof(PGroongaLinqExtensions))
                return null;

            switch (e.Method.Name)
            {
                case nameof(PGroongaLinqExtensions.Match):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&@", typeof(bool));

                case nameof(PGroongaLinqExtensions.Query):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&@~", typeof(bool));

                case nameof(PGroongaLinqExtensions.SimilarSearch):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&@*", typeof(bool));

                case nameof(PGroongaLinqExtensions.ScriptQuery):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&`", typeof(bool));

                case nameof(PGroongaLinqExtensions.MatchIn):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&@|", typeof(bool));

                case nameof(PGroongaLinqExtensions.QueryIn):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&@~|", typeof(bool));

                case nameof(PGroongaLinqExtensions.PrefixSearch):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&^", typeof(bool));

                case nameof(PGroongaLinqExtensions.PrefixRkSearch):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&^~", typeof(bool));

                case nameof(PGroongaLinqExtensions.PrefixSearchIn):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&^|", typeof(bool));

                case nameof(PGroongaLinqExtensions.PrefixRkSearchIn):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&^~|", typeof(bool));

                case nameof(PGroongaLinqExtensions.RegexpMatch):
                    return new CustomBinaryExpression(e.Arguments[0], e.Arguments[1], "&~", typeof(bool));

                default:
                    return null;
            }
        }
    }
}
