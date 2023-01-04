using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using PGroonga.EntityFrameworkCore.Query.ExpressionTranslators.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// PGroonga extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class PGroongaServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services required for PGroonga support in the Npgsql provider for Entity Framework.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddEntityFrameworkPGroonga(
            this IServiceCollection serviceCollection)
        {
            new EntityFrameworkRelationalServicesBuilder(serviceCollection)
               .TryAdd<IMethodCallTranslatorPlugin, PGroongaMethodCallTranslatorPlugin>()
               .TryAdd<IEvaluatableExpressionFilterPlugin, PGroongaEvaluatableExpressionFilterPlugin>();

            return serviceCollection;
        }
    }
}
