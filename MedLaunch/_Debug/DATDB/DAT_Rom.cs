using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MedLaunch._Debug.DATDB
{
    public class DAT_Rom
    {
        public int rid { get; set; }
        public int? gid { get; set; }
        public string name { get; set; }
        public string romName { get; set; }
        public string country { get; set; }
        public string language { get; set; }
        public string developmentStatus { get; set; }
        public string otherFlags { get; set; }
        public string cloneOf { get; set; }
        public string copyright { get; set; }
        public string size { get; set; }
        public string crc { get; set; }
        public string md5 { get; set; }
        public string sha1 { get; set; }
        public string year { get; set; }
        public string publisher { get; set; }
        public string developer { get; set; }
        public int datProviderId { get; set; }
        public int pid { get; set; }
        public string description { get; set; }


        /// <summary>
        /// return list of all roms
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Rom> GetRoms()
        {
            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Rom
                             select g).AsNoTracking();
                return cData.ToList();
            }
        }

        /// <summary>
        /// return list of all roms based on platform
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Rom> GetRoms(int pid)
        {
            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Rom
                             where g.pid == pid
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return list of all roms based on platform that have no game set
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Rom> GetRomsWithNoGameId(int pid)
        {
            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Rom
                             where g.pid == pid && g.gid == null
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return rom based on md5 hash
        /// </summary>
        /// <returns></returns>
        public static DAT_Rom GetRom(string md5)
        {
            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Rom
                             where g.md5.ToUpper() == md5.ToUpper()
                             select g);
                return cData.FirstOrDefault();
            }
        }

        public static void UpdateRoms(List<DAT_Rom> roms)
        {
            using (var db = new AsniDATAdminDbContext())
            {
                db.DAT_Rom.UpdateRange(roms);
                db.SaveChanges();
            }
        }

        public static int[] SaveToDatabase(List<DAT_Rom> roms)
        {
            // get current rom list
            List<DAT_Rom> current = DAT_Rom.GetRoms();

            int added = 0;
            int updated = 0;

            // create temp objects pre-database actions
            List<DAT_Rom> toAdd = new List<DAT_Rom>();
            List<DAT_Rom> toUpdate = new List<DAT_Rom>();
            
            // iterate through each incoming rom
            foreach (var r in roms)
            {
                // attempt rom lookup in current
                DAT_Rom l = (from a in current
                                where a.md5 == r.md5 && a.pid == r.pid
                                select a).ToList().FirstOrDefault();

                if (l == null)
                {
                    // no entry found
                    toAdd.Add(r);
                }
                else
                {
                    // entry found - update required fields
                    DAT_Rom dr = r;
                    dr.gid = l.gid;
                    dr.rid = l.rid;

                    toUpdate.Add(dr);
                }
            }


            using (var db = new AsniDATAdminDbContext())
            {
                // check for duplicate keys
                var distinctToAdd = toAdd.GroupBy(x => x.rid).Select(g => g.OrderByDescending(x => x.rid).First());
                var distinctToUpdate = toUpdate.GroupBy(x => x.rid).Select(g => g.OrderByDescending(x => x.rid).First());

                // update existing entries
                db.DAT_Rom.UpdateRange(distinctToUpdate);
                // add new entries
                db.DAT_Rom.AddRange(distinctToAdd);

                db.SaveChanges();

                added = distinctToAdd.Count();
                updated = distinctToUpdate.Count();

                return new int[] { added, updated };
            }

       }

    }
}
