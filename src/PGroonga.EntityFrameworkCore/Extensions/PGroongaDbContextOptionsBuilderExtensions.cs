using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// PGroonga specific extension methods for <see cref="NpgsqlDbContextOptionsBuilder"/>.
    /// </summary>
    public static class PGroongaDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Use NetTopologySuite to access SQL Server spatial data.
        /// </summary>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static NpgsqlDbContextOptionsBuilder UsePGroonga(
            this NpgsqlDbContextOptionsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var coreOptionsBuilder = ((IRelationalDbContextOptionsBuilderInfrastructure)builder).OptionsBuilder;

            var extension = coreOptionsBuilder.Options.FindExtension<PGroongaOptionsExtension>()
                            ?? new PGroongaOptionsExtension();

            ((IDbContextOptionsBuilderInfrastructure)coreOptionsBuilder).AddOrUpdateExtension(extension);

            return builder;
        }
    }
}
