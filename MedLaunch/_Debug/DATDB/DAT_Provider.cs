using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB
{
    public class DAT_Provider
    {
        public int datProviderId { get; set; }
        public string providerName { get; set; }

        // <summary>
        /// return a list with all providers
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Provider> GetProviders()
        {
            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Provider
                             select g);
                return cData.ToList();
            }
        }
    }
}
