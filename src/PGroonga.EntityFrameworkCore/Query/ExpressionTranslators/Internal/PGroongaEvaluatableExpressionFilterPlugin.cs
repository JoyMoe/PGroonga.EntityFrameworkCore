using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace PGroonga.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class PGroongaEvaluatableExpressionFilterPlugin : IEvaluatableExpressionFilterPlugin
    {
        public bool IsEvaluatableExpression(Expression expression)
            => !(
                expression is MethodCallExpression exp
                && exp.Method.DeclaringType == typeof(PGroongaDbFunctionsExtensions)
            );
    }
}
