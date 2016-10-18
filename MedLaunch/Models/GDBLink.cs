using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace MedLaunch.Models
{
    public class GDBLink
    {
        public int Id { get; set; }
        public int? GdbId { get; set; }
        public int? GameId { get; set; }

        public static GDBLink GetRecord(int GameId)
        {
            using (var db = new MyDbContext())
            {
                var c = db.GDBLink.Where(a => a.GameId == GameId).FirstOrDefault();
                return c;
            }

        }

        public static List<GDBLink> GetRecords(int GameId)
        {
            using (var db = new MyDbContext())
            {
                List<GDBLink> l = new List<GDBLink>();
                l = db.GDBLink.Where(a => a.GameId == GameId).ToList();
                return l;
            }

        }

        public static void DeleteRecord(GDBLink linkRecord)
        {
            using (var db = new MyDbContext())
            {
                db.GDBLink.Remove(linkRecord);
                db.SaveChanges();
            }
        }

        public static void SaveToDatabase(GDBLink link)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.GDBLink.AsNoTracking().ToList();

                List<GDBLink> toAdd = new List<GDBLink>();
                List<GDBLink> toUpdate = new List<GDBLink>();

                GDBLink t = (from a in current
                             where (a.GdbId == link.GdbId && a.GameId == link.GameId)
                             select a).FirstOrDefault();
                if (t == null) { toAdd.Add(link); }
                else { toUpdate.Add(link); }

                db.GDBLink.UpdateRange(toUpdate);
                db.GDBLink.AddRange(toAdd);
                db.SaveChangesAsync();
            }
        }

    }

    
}
