using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace PGroongaTests.Supports
{
    public class PGroongaFixture : SharedStoreFixtureBase<PGroongaContext>
    {
        protected override string StoreName { get; } = "PGroongaTest";

        protected override IServiceCollection AddServices(IServiceCollection serviceCollection)
            => base.AddServices(serviceCollection).AddEntityFrameworkPGroonga();

        public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
        {
            var optionsBuilder = base.AddOptions(builder);
            _ = new NpgsqlDbContextOptionsBuilder(optionsBuilder);

            return optionsBuilder;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder, DbContext context)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder, context);

            modelBuilder.HasPostgresExtension("pgroonga");

            modelBuilder.Entity<PGroongaType>()
                .HasIndex(t => new {t.Id, t.Content})
                .HasMethod("pgroonga")
                .HasDatabaseName("ix_pgroongatypes_id_content");

            modelBuilder.Entity<PGroongaType>()
                .HasIndex(t => t.Tag)
                .HasMethod("pgroonga");
        }

        protected override void Seed(PGroongaContext context)
            => PGroongaContext.Seed(context);

        protected override ITestStoreFactory TestStoreFactory => NpgsqlTestStoreFactory.Instance;

        public TestSqlLoggerFactory TestSqlLoggerFactory =>
            (TestSqlLoggerFactory) ServiceProvider.GetRequiredService<ILoggerFactory>();
    }
}
