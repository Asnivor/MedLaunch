using System.Collections.Generic;
using System.Linq;

namespace MedLaunch._Debug.skeletonKey
{
    public class SK_Game
    {
        public int gid { get; set; }
        public string name { get; set; }
        public int pid { get; set; }
        public string plot { get; set; }
        public string releasedate { get; set; }
        public string genre { get; set; }
        public string developer { get; set; }
        public string publisher { get; set; }
        public string players { get; set; }
        public string rating { get; set; }
        public string ESRB { get; set; }
        public string videourl01 { get; set; }
        public string videourl02 { get; set; }
        public string videourl03 { get; set; }
        public string videourl04 { get; set; }
        public string videourl05 { get; set; }
        public string gamefaqs_url{ get; set; }
        public string mobygames_url { get; set; }
        public string giantbomb_url { get; set; }
        public string boxart01 { get; set; }
        public string boxart02 { get; set; }
        public string boxart03 { get; set; }
        public string boxart04 { get; set; }
        public string boxart05 { get; set; }
        public string boxart06 { get; set; }
        public string boxart07 { get; set; }
        public string boxart08 { get; set; }
        public string boxart09 { get; set; }
        public string boxart10 { get; set; }
        public string boxback01 { get; set; }
        public string boxback02 { get; set; }
        public string boxback03 { get; set; }
        public string boxback04 { get; set; }
        public string boxback05 { get; set; }
        public string boxback06 { get; set; }
        public string boxback07 { get; set; }
        public string boxback08 { get; set; }
        public string boxback09 { get; set; }
        public string boxback10 { get; set; }
        public string media { get; set; }
        public string snapshot01 { get; set; }
        public string snapshot02 { get; set; }
        public string snapshot03 { get; set; }
        public string snapshot04 { get; set; }
        public string snapshot05 { get; set; }
        public string snapshot06 { get; set; }
        public string snapshot07 { get; set; }
        public string snapshot08 { get; set; }
        public string snapshot09 { get; set; }
        public string snapshot10 { get; set; }
        public string snapshot11 { get; set; }
        public string snapshot12 { get; set; }
        public string snapshot13 { get; set; }
        public string snapshot14 { get; set; }
        public string snapshot15 { get; set; }
        public string snapshot16 { get; set; }
        public string snapshot17 { get; set; }
        public string snapshot18 { get; set; }
        public string snapshot19 { get; set; }
        public string snapshot20 { get; set; }
        public string fanart01 { get; set; }
        public string fanart02 { get; set; }
        public string fanart03 { get; set; }
        public string fanart04 { get; set; }
        public string fanart05 { get; set; }
        public string fanart06 { get; set; }
        public string fanart07 { get; set; }
        public string fanart08 { get; set; }
        public string fanart09 { get; set; }
        public string fanart10 { get; set; }
        public string fanart11 { get; set; }
        public string fanart12 { get; set; }
        public string fanart13 { get; set; }
        public string fanart14 { get; set; }
        public string fanart15 { get; set; }
        public string fanart16 { get; set; }
        public string fanart17 { get; set; }
        public string fanart18 { get; set; }
        public string fanart19 { get; set; }
        public string fanart20 { get; set; }
        public string banner01 { get; set; }
        public string banner02 { get; set; }
        public string banner03 { get; set; }
        public string banner04 { get; set; }
        public string banner05 { get; set; }
        public string clearlogo01 { get; set; }
        public string clearlogo02 { get; set; }
        public string clearlogo03 { get; set; }
        public string clearlogo04 { get; set; }
        public string clearlogo05 { get; set; }
        public string notes { get; set; }


        /// <summary>
        /// return a list with all skeletonKey games
        /// </summary>
        /// <returns></returns>
        public static List<SK_Game> GetGames()
        {
            using (var context = new skeletonKeyAdminDbContext())
            {
                var cData = (from g in context.SK_Game
                             select g);
                return cData.ToList();
            }
        }
    }
}
