using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal
{
    /// <summary>
    /// Provides translations for PGroonga full-text search methods.
    /// </summary>
    public class PGroongaMethodTranslator : IMethodCallTranslator
    {
        /// <inheritdoc />
        [CanBeNull]
        public Expression Translate(MethodCallExpression e)
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
