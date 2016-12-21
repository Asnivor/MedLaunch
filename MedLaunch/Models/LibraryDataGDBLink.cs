using MedLaunch.Classes;
using MedLaunch.Classes.GamesLibrary;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{

    public class LibraryDataGDBLink
    {
        public int GDBId { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Year { get; set; }
        
        public string Players { get; set; }
        public string Coop { get; set; }
        public string ESRB { get; set; }


        public static LibraryDataGDBLink GetLibraryData(int gamesdbID)
        {
            using (var db = new MyDbContext())
            {
                var c = db.LibraryDataGDBLink.Where(a => a.GDBId == gamesdbID).FirstOrDefault();
                return c;
            }
        }

        public static List<LibraryDataGDBLink> GetLibraryData()
        {
            using (var db = new MyDbContext())
            {
                var c = (from a in db.LibraryDataGDBLink
                        select a).ToList();
                return c;
            }
        }

        public static void SaveToDataBase(LibraryDataGDBLink lib)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.LibraryDataGDBLink.AsNoTracking().ToList();

                List<LibraryDataGDBLink> toAdd = new List<LibraryDataGDBLink>();
                List<LibraryDataGDBLink> toUpdate = new List<LibraryDataGDBLink>();

                LibraryDataGDBLink t = (from a in current
                             where (a.GDBId == lib.GDBId)
                             select a).FirstOrDefault();
                if (t == null) { toAdd.Add(lib); }
                else { toUpdate.Add(lib); }

                db.LibraryDataGDBLink.UpdateRange(toUpdate);
                db.LibraryDataGDBLink.AddRange(toAdd);
                db.SaveChanges();
            }
        }
    }
  
}
