using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DAT.OFFLINENOINTRO.Models
{
    public class OfflineNoIntroCollection
    {
        // Properties
        public List<OfflineNoIntroObject> Data { get; set; }

        // Constructors
        public OfflineNoIntroCollection()
        {
            Data = new List<OfflineNoIntroObject>();

            Data = OfflineImportNoIntroData.Go();
        }
    }
}
