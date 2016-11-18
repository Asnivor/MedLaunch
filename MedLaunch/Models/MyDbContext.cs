using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Sqlite;
using MedLaunch.Classes;
using System.Collections;

namespace MedLaunch.Models
{
    public class MyDbContext : DbContext
    {

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public DbSet<ConfigBaseSettings> ConfigBaseSettings { get; set; }                       // table containing general mednafen settings
        public DbSet<ConfigSystemSettings> ConfigSystemSettings { get; set; }                   // system specific config entries
        public DbSet<ConfigNetplaySettings> ConfigNetplaySettings { get; set; }                 // table containing general mednafen netplay settings
        public DbSet<ConfigServerSettings> ConfigServerSettings { get; set; }                   // table containing mednafen netplay server info
        public DbSet<Versions> Versions { get; set; }                                           // table containing app and database version information
        public DbSet<Paths> Paths { get; set; }                                                 // table containing filesystem paths
        //public GSystem GSystem { get; set; }                                                    // Non-DB table containing list of different systems
        public DbSet<Game> Game { get; set; }                                                   // table containing list of imported ROMs
        public DbSet<GlobalSettings> GlobalSettings { get; set; }                               // launcher settings

        public DbSet<GDBPlatformGame> GDBPlatformGame { get; set; }                             // basic list of all games per platform
        public DbSet<GDBGameData> GDBGameData { get; set; }                                     // Game data from thegamesdb.net - primary key is the gamesdb.net GameId
        public DbSet<GDBLink> GDBLink { get; set; }                                             // table that links games with gamedb gamedata

        public DbSet<LibraryDataGDBLink> LibraryDataGDBLink { get; set; }                       // table that links GDB ids with basic scraped data (for display in the games library datagrid)
        

        //public DbSet<MobyPlatformGame> MobyPlatformGame { get; set; }                           // basic list of all moby games per platform

        // define keys and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigBaseSettings>()
                .HasKey(c => c.ConfigId);

            modelBuilder.Entity<ConfigSystemSettings>()
                .HasKey(c => c.ConfigSysId);

            modelBuilder.Entity<ConfigNetplaySettings>()
                .HasKey(c => c.ConfigNPId);

            modelBuilder.Entity<ConfigServerSettings>()
                .HasKey(c => c.ConfigServerId);

            modelBuilder.Entity<Versions>()
                .HasKey(c => c.versionId);

            modelBuilder.Entity<Paths>()
                .HasKey(c => c.pathId);
            /*
            modelBuilder.Entity<GameSystem>()
                .HasKey(c => c.systemId);
*/
            modelBuilder.Entity<Game>()
                .HasKey(c => c.gameId);

            modelBuilder.Entity<GlobalSettings>()
                .HasKey(c => c.settingsId);

            modelBuilder.Entity<GDBPlatformGame>()
                .HasKey(c => c.id);

            modelBuilder.Entity<GDBGameData>()
                .HasKey(c => c.GdbId);

            modelBuilder.Entity<GDBLink>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<LibraryDataGDBLink>()
                      .HasKey(c => c.GDBId);

            /*
            modelBuilder.Entity<MobyPlatformGame>()
                .HasKey(c => c.Id); */
        }


        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConn = new SqliteConnection(@"DataSource = Data\Settings\MedLaunch.db");
            optionsBuilder.UseSqlite(sqliteConn);
        }
    }

    
}
