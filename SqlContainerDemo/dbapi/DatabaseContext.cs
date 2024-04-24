using dbapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace dbapi
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions)
        : base(dbContextOptions)
        {

            try
            {
                var dbcreateor = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

                if (dbcreateor != null)
                {
                    if (!dbcreateor.CanConnect()) dbcreateor.Create();
                    if (!dbcreateor.HasTables()) dbcreateor.CreateTables();

                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine($"DatabaseContext: {ex.Message}");
            }

        }

        public DbSet<Customer> Customers { get; set; } = null;
    }
}
