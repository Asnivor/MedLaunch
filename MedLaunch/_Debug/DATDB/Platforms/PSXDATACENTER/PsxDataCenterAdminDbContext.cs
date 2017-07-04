using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Sqlite;

namespace MedLaunch._Debug.DATDB.Platforms.PSXDATACENTER
{
    public class PsxDataCenterAdminDbContext : DbContext
    {
        // Add a DbSet for each entity type that you want to include in your model.For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<PSX_Games> PSX_Games { get; set; }                       // table containing psxdatacenter games

        // define keys and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PSX_Games>()
                .HasKey(c => c.id);
        }


        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection(@"DataSource = ..\..\_Debug\DATDB\DATFiles\PSXDATACENTER\PSXDC.db");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }
}
