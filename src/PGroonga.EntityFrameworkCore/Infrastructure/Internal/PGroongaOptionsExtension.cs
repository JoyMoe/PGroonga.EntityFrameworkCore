using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal
{
    public class PGroongaOptionsExtension : IDbContextOptionsExtension
    {
        public virtual string LogFragment => "using PGroonga ";

        public virtual bool ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkPGroonga();

            return false;
        }

        public virtual long GetServiceProviderHashCode() => 0;

        public virtual void Validate(IDbContextOptions options)
        {
            var internalServiceProvider = options.FindExtension<CoreOptionsExtension>()?.InternalServiceProvider;
            if (internalServiceProvider != null)
            {
                using (var scope = internalServiceProvider.CreateScope())
                {
                    if (scope.ServiceProvider.GetService<IEnumerable<IMethodCallTranslatorPlugin>>()
                            ?.Any(s => s is PGroongaMethodCallTranslatorPlugin) != true)
                    {
                        throw new InvalidOperationException($"{nameof(DbContextOptionsBuilderExtensions.UsePGroonga)} requires {nameof(PGroongaServiceCollectionExtensions.AddEntityFrameworkPGroonga)} to be called on the internal service provider used.");
                    }
                }
            }
        }
    }
}
