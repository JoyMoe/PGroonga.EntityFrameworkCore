using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

// ReSharper disable once CheckNamespace
namespace PGroonga.EntityFrameworkCore
{
    /// <summary>
    /// Provides translation services for PGroonga members.
    /// </summary>
    public class PGroongaMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
    {
        public PGroongaMethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory, IRelationalTypeMappingSource typeMappingSource) =>
            Translators = new IMethodCallTranslator[]
            {
                new PGroongaMethodCallTranslator((SqlExpressionFactory) sqlExpressionFactory, typeMappingSource),
            };

        public virtual IEnumerable<IMethodCallTranslator> Translators { get; }
    }

    /// <summary>
    /// Provides translations for PGroonga full-text search methods.
    /// </summary>
    public class PGroongaMethodCallTranslator : IMethodCallTranslator
    {
        private readonly SqlExpressionFactory _sqlExpressionFactory;
        private readonly RelationalTypeMapping _boolMapping;

        private static readonly IReadOnlyDictionary<string, string> SqlNameByMethodName = new Dictionary<string, string>
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

        public PGroongaMethodCallTranslator(SqlExpressionFactory sqlExpressionFactory, IRelationalTypeMappingSource typeMappingSource)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
            _boolMapping = typeMappingSource.FindMapping(typeof(bool));
        }

        /// <inheritdoc />
        [CanBeNull]
        public SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
        {
            if (method.DeclaringType != typeof(PGroongaDbFunctionsExtensions) &&
                method.DeclaringType != typeof(PGroongaLinqExtensions))
                return null;

            if (!SqlNameByMethodName.TryGetValue(method.Name, out var sqlFunctionName))
                return TryTranslateOperator(method, arguments);

            if (sqlFunctionName != "pgroonga_score")
                return _sqlExpressionFactory.Function(sqlFunctionName, arguments.Skip(1), method.ReturnType);

            // hack for pgroonga_score
            return _sqlExpressionFactory.Function(sqlFunctionName, new[]
            {
                _sqlExpressionFactory.Fragment("tableoid"),
                _sqlExpressionFactory.Fragment("ctid")
            }, method.ReturnType);
        }

        [CanBeNull]
        private SqlExpression TryTranslateOperator(MemberInfo method, IReadOnlyList<SqlExpression> arguments)
        {
            if (method.DeclaringType != typeof(PGroongaLinqExtensions))
                return null;

            return method.Name switch
            {
                nameof(PGroongaLinqExtensions.Match) => BoolReturningOnTwoQueries("&@"),
                nameof(PGroongaLinqExtensions.Query) => BoolReturningOnTwoQueries("&@~"),
                nameof(PGroongaLinqExtensions.SimilarSearch) => BoolReturningOnTwoQueries("&@*"),
                nameof(PGroongaLinqExtensions.ScriptQuery) => BoolReturningOnTwoQueries("&`"),
                nameof(PGroongaLinqExtensions.MatchIn) => BoolReturningOnTwoQueries("&@|"),
                nameof(PGroongaLinqExtensions.QueryIn) => BoolReturningOnTwoQueries("&@~|"),
                nameof(PGroongaLinqExtensions.PrefixSearch) => BoolReturningOnTwoQueries("&^"),
                nameof(PGroongaLinqExtensions.PrefixRkSearch) => BoolReturningOnTwoQueries("&^~"),
                nameof(PGroongaLinqExtensions.PrefixSearchIn) => BoolReturningOnTwoQueries("&^|"),
                nameof(PGroongaLinqExtensions.PrefixRkSearchIn) => BoolReturningOnTwoQueries("&^~|"),
                nameof(PGroongaLinqExtensions.RegexpMatch) => BoolReturningOnTwoQueries("&~"),
                _ => null
            };

            SqlCustomBinaryExpression BoolReturningOnTwoQueries(string @operator)
            {
#pragma warning disable EF1001
                return new SqlCustomBinaryExpression(
                    _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[0]),
                    _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[1]),
                    @operator,
                    typeof(bool),
                    _boolMapping
                );
#pragma warning restore EF1001
            }
        }
    }
}
