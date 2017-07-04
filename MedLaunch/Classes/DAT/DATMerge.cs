using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.DAT
{
    public class DATMerge
    {
        public int id { get; set; }
        public string RomName { get; set; }
        public string Name { get; set; }
        public string CloneOf { get; set; }
        public string Copyright { get; set; }
        public string Country { get; set; }
        public int DatProviderId { get; set; }
        public string Description { get; set; }
        public string DevelopmentStatus { get; set; }
        public int? gid { get; set; }
        public string Language { get; set; }
        public string MD5 { get; set; }
        public string OtherFlags { get; set; }
        public int pid { get; set; }
        public string GameName { get; set; }
        public int? gdbid { get; set; }
        public string Year { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }




        /* Static Methods */

        /// <summary>
        /// return a list of entries based on systemid and md5 hash
        /// </summary>
        /// <param name="inputList"></param>
        /// <param name="systemId"></param>
        /// <param name="md5Hash"></param>
        /// <returns></returns>
        public static List<DATMerge> GetDATsByHash(List<DATMerge> inputList, int systemId, string md5Hash)
        {
            var pData = FilterByMedLaunchSystemId(inputList, systemId);

            var cData = (from g in pData
                        where g.MD5.ToUpper().Trim() == md5Hash.ToUpper().Trim()
                        select g).ToList();
            return cData;
        }

        /// <summary>
        /// return a list of entries based on systemid and extraflags (serial number)
        /// </summary>
        /// <param name="inputList"></param>
        /// <param name="systemId"></param>
        /// <param name="md5Hash"></param>
        /// <returns></returns>
        public static List<DATMerge> GetDATsBySN(List<DATMerge> inputList, int systemId, string serialNumber)
        {
            var pData = FilterByMedLaunchSystemId(inputList, systemId);

            var cData = (from g in pData
                         where g.OtherFlags.ToUpper().Trim() == serialNumber.ToUpper().Trim()
                         select g).ToList();
            return cData;
        }

        public static List<DATMerge> FilterByMedLaunchSystemId(List<DATMerge> inputList, int medSysId)
        {
            List<DATMerge> working = new List<DATMerge>();

            switch (medSysId)
            {
                case 1:
                    working = inputList.Where(a => a.pid == 4 || a.pid == 41).ToList();
                    break;
                case 2:
                    working = inputList.Where(a => a.pid == 5).ToList();
                    break;
                case 3:
                    working = inputList.Where(a => a.pid == 4924).ToList();
                    break;
                case 4:
                    working = inputList.Where(a => a.pid == 36 || a.pid == 18).ToList();
                    break;
                case 5:
                    working = inputList.Where(a => a.pid == 20).ToList();
                    break;
                case 6:
                    working = inputList.Where(a => a.pid == 4923 || a.pid == 4924).ToList();
                    break;
                case 7:
                    working = inputList.Where(a => a.pid == 34 || a.pid == 4955).ToList();
                    break;
                case 8:
                    working = inputList.Where(a => a.pid == 4930).ToList();
                    break;
                case 9:
                    working = inputList.Where(a => a.pid == 10).ToList();
                    break;
                case 10:
                    working = inputList.Where(a => a.pid == 35).ToList();
                    break;
                case 11:
                    working = inputList.Where(a => a.pid == 7 || a.pid == 4936).ToList();
                    break;
                case 12:
                    working = inputList.Where(a => a.pid == 6).ToList();
                    break;
                case 13:
                    working = inputList.Where(a => a.pid == 17).ToList();
                    break;
                case 14:
                    working = inputList.Where(a => a.pid == 4918).ToList();
                    break;
                case 15:
                    working = inputList.Where(a => a.pid == 4925 || a.pid == 4926).ToList();
                    break;
                case 16:
                    working = inputList.Where(a => a.pid == 6).ToList();
                    break;
                case 17:
                    working = inputList.Where(a => a.pid == 34 || a.pid == 4955).ToList();
                    break;
                case 18:
                    working = inputList.Where(a => a.pid == 34 || a.pid == 4955).ToList();
                    break;
            }

            return working;
        }


        /// <summary>
        /// return all entries
        /// </summary>
        /// <returns></returns>
        public static List<DATMerge> GetDATs()
        {
            using (var context = new AsniDATDbContext())
            {
                var cData = (from g in context.DATMerge
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return all entries based on platformId
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static List<DATMerge> GetDATs(int pid)
        {
            using (var context = new AsniDATDbContext())
            {
                var cData = (from g in context.DATMerge
                             where g.pid == pid
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return list where md5 matches
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static List<DATMerge> GetDATs(string md5)
        {
            using (var context = new AsniDATDbContext())
            {
                var cData = (from g in context.DATMerge
                             where g.MD5.ToUpper().Trim() == md5.ToUpper().Trim()
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return the first entry where the MD5 hash matches
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static DATMerge GetDAT(string md5)
        {
            using (var context = new AsniDATDbContext())
            {
                var cData = (from g in context.DATMerge
                             where g.MD5.ToUpper().Trim() == md5.ToUpper().Trim()
                             select g).FirstOrDefault();
                return cData;
            }
        }

        

    }
}
