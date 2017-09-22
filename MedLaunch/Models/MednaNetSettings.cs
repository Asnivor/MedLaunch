using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class MednaNetSettings
    {
        public int Id { get; set; }
        public string InstallKey { get; set; }
        public string LastUsername { get; set; }

        public static MednaNetSettings GetMednaNetDefaults()
        {
            MednaNetSettings ms = new MednaNetSettings
            {
                Id = 1,
                InstallKey = ""
            };

            return ms;
        }

        // return MednaNet Settings entry from DB
        public static MednaNetSettings GetGlobals()
        {
            MednaNetSettings gs = new MednaNetSettings();
            using (var context = new MyDbContext())
            {
                var query = from s in context.MednaNetSettings
                            where s.Id == 1
                            select s;
                gs = query.FirstOrDefault();
            }
            return gs;
        }

        // write MednaNet Settings object to DB
        public static void SetGlobals(MednaNetSettings gs)
        {
            using (var context = new MyDbContext())
            {
                context.MednaNetSettings.Attach(gs);
                var entry = context.Entry(gs);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        // get install key
        public static string GetInstallKey()
        {
            MednaNetSettings gs = new MednaNetSettings();
            using (var context = new MyDbContext())
            {
                var query = from s in context.MednaNetSettings
                            where s.Id == 1
                            select s;
                gs = query.FirstOrDefault();
            }
            return gs.InstallKey;
        }

        // set install key
        public static void SetInstallKey(string installKey)
        {
            if (installKey == null)
                return;

            MednaNetSettings gs = MednaNetSettings.GetGlobals();
            gs.InstallKey = installKey;

            using (var context = new MyDbContext())
            {
                context.MednaNetSettings.Attach(gs);
                var entry = context.Entry(gs);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        // get install key
        public static string GetUsername()
        {
            MednaNetSettings gs = new MednaNetSettings();
            using (var context = new MyDbContext())
            {
                var query = from s in context.MednaNetSettings
                            where s.Id == 1
                            select s;
                gs = query.FirstOrDefault();
            }
            return gs.LastUsername;
        }

        // set install key
        public static void SetUsername(string username)
        {
            if (username == null)
                return;

            MednaNetSettings gs = MednaNetSettings.GetGlobals();
            gs.LastUsername = username;

            using (var context = new MyDbContext())
            {
                context.MednaNetSettings.Attach(gs);
                var entry = context.Entry(gs);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
