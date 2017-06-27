using Microsoft.Data.Entity;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB
{
    public class AsniDATAdminDbContext : DbContext
    {
        // Add a DbSet for each entity type that you want to include in your model.For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<DAT_System> DAT_System { get; set; }                       // table containing thegamesdb.net platforms
        public DbSet<DAT_Game> DAT_Game { get; set; }                           // table containing all top-level games
        public DbSet<DAT_Provider> DAT_provider { get; set; }                   // table containing all DAT providers



        // define keys and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DAT_System>()
                .HasKey(c => c.pid);

            modelBuilder.Entity<DAT_Game>()
                .HasKey(c => c.gid);
        }


        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection(@"DataSource = ..\..\Data\System\AsniDAT.db");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }
}
