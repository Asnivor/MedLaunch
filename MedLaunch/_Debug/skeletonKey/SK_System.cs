using System.Collections.Generic;
using System.Linq;

namespace MedLaunch._Debug.skeletonKey
{
    public class SK_System
    {
        /// <summary>
        /// Platform ID
        /// </summary>
        public int pid { get; set; }
        /// <summary>
        /// Platform Name
        /// </summary>
        public string platformName { get; set; }

        /// <summary>
        /// return a list with all skeletonKey mednafen-compatible platforms
        /// these have been populated manually
        /// </summary>
        /// <returns></returns>
        public static List<SK_System> GetSystems()
        {
            using (var context = new skeletonKeyAdminDbContext())
            {
                var cData = (from g in context.SK_System
                             select g);
                return cData.ToList();
            }
        }
    }
}
