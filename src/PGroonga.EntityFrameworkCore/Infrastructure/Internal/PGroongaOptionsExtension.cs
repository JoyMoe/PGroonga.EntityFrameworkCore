using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using PGroonga.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal
{
    public class PGroongaOptionsExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo _info;

        void IDbContextOptionsExtension.ApplyServices(IServiceCollection services)
            => services.AddEntityFrameworkPGroonga();

        public virtual DbContextOptionsExtensionInfo Info
            => _info ??= new ExtensionInfo(this);

        public virtual long GetServiceProviderHashCode() => 0;

        public virtual void Validate(IDbContextOptions options)
        {
            var internalServiceProvider = options.FindExtension<CoreOptionsExtension>()?.InternalServiceProvider;
            if (internalServiceProvider == null) return;
            using var scope = internalServiceProvider.CreateScope();
            if (scope.ServiceProvider.GetService<IEnumerable<IMethodCallTranslatorPlugin>>()
                    ?.Any(s => s is PGroongaMethodCallTranslatorPlugin) != true)
            {
                throw new InvalidOperationException($"{nameof(PGroongaDbContextOptionsBuilderExtensions.UsePGroonga)} requires {nameof(PGroongaServiceCollectionExtensions.AddEntityFrameworkPGroonga)} to be called on the internal service provider used.");
            }
        }

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            public ExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }

            public override bool IsDatabaseProvider => false;

            public override long GetServiceProviderHashCode() => 0;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
                => debugInfo["Npgsql:" + nameof(PGroongaDbContextOptionsBuilderExtensions.UsePGroonga)] = "1";

            public override string LogFragment => "using PGroonga ";
        }
    }
}
