using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;

namespace PGroongaTests.Supports
{
    /// <summary>
    /// NpgsqlTestStoreFactory
    /// </summary>
    /// <remarks>https://github.com/npgsql/efcore.pg/blob/v3.1.0/test/EFCore.PG.FunctionalTests/TestUtilities/NpgsqlTestStoreFactory.cs</remarks>
    public class NpgsqlTestStoreFactory : RelationalTestStoreFactory
    {
        public static NpgsqlTestStoreFactory Instance { get; } = new NpgsqlTestStoreFactory();

        protected NpgsqlTestStoreFactory()
        {
        }

        public override TestStore Create(string storeName) => NpgsqlTestStore.Create(storeName);

        public override TestStore GetOrCreate(string storeName) => NpgsqlTestStore.GetOrCreate(storeName);

        public override IServiceCollection AddProviderServices(IServiceCollection serviceCollection) => serviceCollection.AddEntityFrameworkNpgsql();
    }
}
