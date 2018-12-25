using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal
{
    public class PGroongaMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
    {
        public virtual IEnumerable<IMethodCallTranslator> Translators { get; } = new IMethodCallTranslator[]
        {
            new PGroongaMethodTranslator()
        };
    }
}
