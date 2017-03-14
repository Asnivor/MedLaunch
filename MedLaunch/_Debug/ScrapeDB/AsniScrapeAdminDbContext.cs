using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Sqlite;
using MedLaunch.Classes;
using System.Collections;

namespace MedLaunch._Debug.ScrapeDB
{
    public class AsniScrapeAdminDbContext : DbContext
    {
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public DbSet<GDB_Platform> GDB_Platform { get; set; }               // table containing thegamesdb.net platforms (mednafen compatible only)
        public DbSet<GDB_Game> GDB_Game { get; set; }                       // table containing thegamesdb.net platform games list
        public DbSet<MOBY_Platform> MOBY_Platform { get; set; }             // table containing mobygames platforms (mednafen compatible only)
        public DbSet<MOBY_Game> MOBY_Game { get; set; }                     // table containing mobygames platform games list (medlaunch generated primary key)

        public DbSet<Junction> Junction { get; set; }                       // junction table linking gdb games with all other data

        public DbSet<MasterView> MasterView { get; set; }                   // SQLite view that really brings the room together

        // define keys and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GDB_Platform>()
                .HasKey(c => c.pid);
            modelBuilder.Entity<GDB_Game>()
                .HasKey(c => c.gid);
            modelBuilder.Entity<MOBY_Platform>()
                .HasKey(c => c.pid);
            modelBuilder.Entity<MOBY_Game>()
                .HasKey(c => c.mid);

            modelBuilder.Entity<Junction>()
                .HasKey(c => c.gid);

            modelBuilder.Entity<MasterView>()
                .HasKey(c => c.gid);
        }


        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection(@"DataSource = ..\..\Data\System\AsniScrape.db");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }

    
}
