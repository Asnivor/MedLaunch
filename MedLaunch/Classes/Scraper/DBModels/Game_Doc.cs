using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DBModels
{
    public class Game_Doc
    {
        public int id { get; set; }
        public int? gid { get; set; }
        public string downloadUrl { get; set; }
        public int? pid { get; set; }

        public static List<Game_Doc> GetDocs()
        {
            using (var context = new ScrapeDbContext())
            {
                var cData = (from g in context.Game_Doc
                             select g).ToList();
                return cData;
            }
        }

        public static Game_Doc ReturnDocEntry(int id)
        {
            using (var context = new ScrapeDbContext())
            {
                var cData = (from g in context.Game_Doc
                             where g.id == id
                             select g).FirstOrDefault();
                return cData;
            }
        }

        public static List<Game_Doc> ReturnDocEntriesByGid(int gid)
        {
            using (var context = new ScrapeDbContext())
            {
                var cData = (from g in context.Game_Doc
                             where g.gid == gid
                             select g).ToList();
                return cData;
            }
        }

        public static void AddDoc(Game_Doc doc)
        {
            using (var aG = new ScrapeDbContext())
            {
                // check whether doc url already exists
                List<Game_Doc> docs = (from a in aG.Game_Doc
                                       where a.downloadUrl == doc.downloadUrl
                                       select a).ToList();

                if (docs.Count == 0)
                {
                    aG.Game_Doc.Add(doc);
                    aG.SaveChanges();
                    aG.Dispose();
                }
            }
        }

    }
}
