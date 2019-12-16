using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Npgsql;

namespace PGroongaTests.Supports
{
    /// <summary>
    /// NpgsqlTestStore
    /// </summary>
    /// <remarks>https://github.com/npgsql/efcore.pg/blob/v3.1.0/test/EFCore.PG.FunctionalTests/TestUtilities/NpgsqlTestStore.cs</remarks>
    public class NpgsqlTestStore : RelationalTestStore
    {
        public const int CommandTimeout = 600;

        public static NpgsqlTestStore GetOrCreate(string name) => new NpgsqlTestStore(name);

        public static NpgsqlTestStore Create(string name) => new NpgsqlTestStore(name, false);

        NpgsqlTestStore(
            string name,
            bool shared = true)
            : base(name, shared)
        {
            Name = name;

            // ReSharper disable VirtualMemberCallInConstructor
            ConnectionString = CreateConnectionString(Name);
            Connection = new NpgsqlConnection(ConnectionString);
            // ReSharper restore VirtualMemberCallInConstructor
        }

        protected override void Initialize(Func<DbContext> createContext, Action<DbContext> seed, Action<DbContext> clean)
        {
            if (!CreateDatabase(clean)) return;
            using var context = createContext();
            context.Database.EnsureCreatedResiliently();
            seed?.Invoke(context);
        }

        public override DbContextOptionsBuilder AddProviderOptions(DbContextOptionsBuilder builder)
            => builder.UseNpgsql(Connection, b => b.CommandTimeout(CommandTimeout));

        bool CreateDatabase(Action<DbContext> clean)
        {
            using var master = new NpgsqlConnection(CreateAdminConnectionString());

            if (DatabaseExists(Name))
            {
                using var context = new DbContext(
                    AddProviderOptions(
                            new DbContextOptionsBuilder()
                                .EnableServiceProviderCaching(false))
                        .Options);
                clean?.Invoke(context);
                Clean(context);
                return true;
            }

            ExecuteNonQuery(master, $@"CREATE DATABASE ""{Name}""");
            WaitForExists((NpgsqlConnection)Connection);

            return true;
        }

        void DeleteDatabase()
        {
            if (!DatabaseExists(Name)) return;

            using var master = new NpgsqlConnection(CreateAdminConnectionString());

            ExecuteNonQuery(master, $@"
REVOKE CONNECT ON DATABASE ""{Name}"" FROM PUBLIC;
SELECT pg_terminate_backend (pg_stat_activity.pid)
   FROM pg_stat_activity
   WHERE datname = '{Name}'");

            ExecuteNonQuery(master, $@"DROP DATABASE ""{Name}""");

            NpgsqlConnection.ClearAllPools();
        }

        static void WaitForExists(NpgsqlConnection connection)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }

                    NpgsqlConnection.ClearPool(connection);

                    connection.Open();
                    connection.Close();
                    return;
                }
                catch (PostgresException e)
                {
                    if (++retryCount >= 30
                        || e.SqlState != "08001" && e.SqlState != "08000" && e.SqlState != "08006")
                    {
                        throw;
                    }

                    Thread.Sleep(100);
                }
            }
        }

        static bool DatabaseExists(string name)
        {
            using var master = new NpgsqlConnection(CreateAdminConnectionString());
            return ExecuteScalar<long>(master, $@"SELECT COUNT(*) FROM pg_database WHERE datname = '{name}'") > 0;
        }

        public override void OpenConnection() => Connection.Open();

        public override Task OpenConnectionAsync() => Connection.OpenAsync();

        static T ExecuteScalar<T>(DbConnection connection, string sql, params object[] parameters)
            => Execute(connection, command => (T)command.ExecuteScalar(), sql, false, parameters);

        static int ExecuteNonQuery(DbConnection connection, string sql, object[] parameters = null)
            => Execute(connection, command => command.ExecuteNonQuery(), sql, false, parameters);

        static T Execute<T>(
            DbConnection connection, Func<DbCommand, T> execute, string sql,
            bool useTransaction = false, object[] parameters = null)
            => ExecuteCommand(connection, execute, sql, useTransaction, parameters);

        static T ExecuteCommand<T>(
            DbConnection connection, Func<DbCommand, T> execute, string sql, bool useTransaction, object[] parameters)
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
            connection.Open();
            try
            {
                using var transaction = useTransaction ? connection.BeginTransaction() : null;
                T result;
                using (var command = CreateCommand(connection, sql, parameters))
                {
                    command.Transaction = transaction;
                    result = execute(command);
                }
                transaction?.Commit();

                return result;
            }
            finally
            {
                if (connection.State == ConnectionState.Closed
                    && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        static DbCommand CreateCommand(
            DbConnection connection, string commandText, IReadOnlyList<object> parameters = null)
        {
            var command = (NpgsqlCommand)connection.CreateCommand();

            command.CommandText = commandText;
            command.CommandTimeout = CommandTimeout;

            if (parameters == null) return command;

            for (var i = 0; i < parameters.Count; i++)
            {
                command.Parameters.AddWithValue("p" + i, parameters[i]);
            }

            return command;
        }

        public static string CreateConnectionString(string name)
            => new NpgsqlConnectionStringBuilder("Server=localhost;Username=postgres;Password=Password12!")
            {
                Database = name
            }.ConnectionString;

        static string CreateAdminConnectionString() => CreateConnectionString("postgres");

        public override void Clean(DbContext context)
        {
            //
        }
    }
}
