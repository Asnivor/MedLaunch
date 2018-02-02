using Microsoft.Data.Entity;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.skeletonKey
{
    public class skeletonKeyAdminDbContext : DbContext
    {
        // Add a DbSet for each entity type that you want to include in your model.For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<SK_System> SK_System { get; set; }  
        public DbSet<SK_Game> SK_Game { get; set; }

        // define keys and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SK_System>()
                .HasKey(c => c.pid);
            modelBuilder.Entity<SK_Game>()
                .HasKey(c => c.gid);
        }


        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection(@"DataSource = ..\..\Data\System\skeletonKey.db");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }
}
