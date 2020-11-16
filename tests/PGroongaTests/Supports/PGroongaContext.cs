using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestUtilities;

namespace PGroongaTests.Supports
{
    public class PGroongaContext : PoolableDbContext
    {
        public PGroongaContext(DbContextOptions<PGroongaContext> options) :
            base(options)
        {
        }

        public DbSet<PGroongaType> PGroongaTypes { get; set; } = null!;

        public static void Seed(PGroongaContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.PGroongaTypes.Any()) return;

            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 1,
                Tag = "PostgreSQL",
                Content = "PostgreSQL is a relational database management system."
            });
            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 2,
                Tag = "Groonga",
                Content = "Groonga is a fast full text search engine that supports all languages."
            });
            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 3,
                Tag = "PGroonga",
                Content = "PGroonga is a PostgreSQL extension that uses Groonga as index."
            });
            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 4,
                Tag = "pglogical",
                Content = "There is groonga command."
            });
            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 5,
                Tag = "ポストグレスキューエル",
                Content = "There is katakana."
            });
            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 6,
                Tag = "ポスグレ",
                Content = "There is katakana."
            });
            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 7,
                Tag = "グルンガ",
                Content = "There is katakana."
            });
            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 8,
                Tag = "ピージールンガ",
                Content = "There is katakana."
            });
            context.PGroongaTypes.Add(new PGroongaType
            {
                Id = 9,
                Tag = "ピージーロジカル",
                Content = "There is katakana."
            });
            context.SaveChanges();
        }
    }
}
