using Microsoft.Data.Entity;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes.Scraper.DBModels;

namespace MedLaunch.Classes.Scraper
{
    public class ScrapeDbContext : DbContext
    {
        public DbSet<MasterView> MasterView { get; set; }                   // SQLite view that really brings the room together

        public DbSet<Game_Doc> Game_Doc { get; set; }                       // table with manuals and links

        // define keys and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MasterView>()
                .HasKey(c => c.gid);

            modelBuilder.Entity<Game_Doc>()
                .HasKey(c => c.id);
        }


        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection(@"DataSource = Data\System\AsniScrape.db");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }
}
