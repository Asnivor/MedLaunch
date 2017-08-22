using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.DAT
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
            using (var context = new AsniDATDbContext())
            {
                var cData = (from g in context.DAT_Rom
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return list of all roms based on platform
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Rom> GetRoms(int pid)
        {
            using (var context = new AsniDATDbContext())
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
            using (var context = new AsniDATDbContext())
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
            using (var context = new AsniDATDbContext())
            {
                var cData = (from g in context.DAT_Rom
                             where g.md5.ToUpper() == md5.ToUpper()
                             select g);
                return cData.FirstOrDefault();
            }
        }

        public static List<DAT_Rom> GetRoms(string md5)
        {
            using (var context = new AsniDATDbContext())
            {
                var cData = (from g in context.DAT_Rom
                             where g.md5.ToUpper() == md5.ToUpper()
                             select g);
                return cData.ToList();
            }
        }
    }
}
