using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class PGroongaLinqExtensions
    {
        #region TEXT fields

        /// <summary>
        /// This method generates the "&amp;@" match operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text, varchar or jsonb field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/match-v2.html</remarks>
        public static bool Match(this string query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;@~" query operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text, varchar or jsonb field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/query-v2.html</remarks>
        public static bool Query(this string query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;@*" similar search operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text or varchar field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/similar-search-v2.html</remarks>
        public static bool SimilarSearch(this string query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;`" script operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text, varchar or jsonb field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/script-v2.html</remarks>
        public static bool ScriptQuery(this string query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;@|" match in operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text or varchar field</param>
        /// <param name="keywords">Keywords to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/match-in-v2.html</remarks>
        public static bool MatchIn(this string query, IEnumerable<string> keywords) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;@~|" query in operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text or varchar field</param>
        /// <param name="keywords">Keywords to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/query-in-v2.html</remarks>
        public static bool QueryIn(this string query, IEnumerable<string> keywords) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;^" prefix search operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text or varchar field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/prefix-search-v2.html</remarks>
        public static bool PrefixSearch(this string query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;^~" prefix rk search operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text or varchar field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/prefix-rk-search-v2.html</remarks>
        public static bool PrefixRkSearch(this string query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;^|" prefix search in operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text or varchar field</param>
        /// <param name="keywords">Keywords to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/prefix-search-in-v2.html</remarks>
        public static bool PrefixSearchIn(this string query, IEnumerable<string> keywords) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;^~|" prefix rk search in operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text or varchar field</param>
        /// <param name="keywords">Keywords to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/prefix-rk-search-in-v2.html</remarks>
        public static bool PrefixRkSearchIn(this string query, IEnumerable<string> keywords) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;~" regex match operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text or varchar field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/regular-expression-v2.html</remarks>
        public static bool RegexpMatch(this string query, string keyword) => throw new NotSupportedException();

        #endregion

        #region ARRAY(TEXT) fields

        /// <summary>
        /// This method generates the "&amp;@" match operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/match-v2.html</remarks>
        public static bool Match(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;@~" query operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/query-v2.html</remarks>
        public static bool Query(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;@*" similar search operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/similar-search-v2.html</remarks>
        public static bool SimilarSearch(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;`" script operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/script-v2.html</remarks>
        public static bool ScriptQuery(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;@|" match in operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keywords">Keywords to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/match-in-v2.html</remarks>
        public static bool MatchIn(this IEnumerable<string> query, IEnumerable<string> keywords) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;@~|" query in operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keywords">Keywords to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/query-in-v2.html</remarks>
        public static bool QueryIn(this IEnumerable<string> query, IEnumerable<string> keywords) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;^" prefix search operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/prefix-search-v2.html</remarks>
        public static bool PrefixSearch(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;^~" prefix rk search operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keyword">Keyword to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/prefix-rk-search-v2.html</remarks>
        public static bool PrefixRkSearch(this IEnumerable<string> query, string keyword) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;^|" prefix search in operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keywords">Keywords to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/prefix-search-in-v2.html</remarks>
        public static bool PrefixSearchIn(this IEnumerable<string> query, IEnumerable<string> keywords) => throw new NotSupportedException();

        /// <summary>
        /// This method generates the "&amp;^~|" prefix rk search in operator
        /// </summary>
        /// <param name="query">A plain search query, should be a text[] field</param>
        /// <param name="keywords">Keywords to search</param>
        /// <remarks>https://pgroonga.github.io/reference/operators/prefix-rk-search-in-v2.html</remarks>
        public static bool PrefixRkSearchIn(this IEnumerable<string> query, IEnumerable<string> keywords) => throw new NotSupportedException();

        #endregion
    }
}
