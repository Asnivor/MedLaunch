using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB.Platforms.NOINTRO.Models
{
    public class NoIntroCollection
    {
        // Properties
        public List<DAT_Rom> Data { get; set; }

        // Constructors
        /*
        public NoIntroCollection()
        {
            Data = new List<DAT_Rom>();

            Data = ImportNoIntroData.Go();
        }
        */

        public NoIntroCollection(int platformId)
        {
            Data = new List<DAT_Rom>();
            if (platformId == 0)
                Data = ImportNoIntroData.Go();
            else
                Data = ImportNoIntroData.Go(platformId);
        }
    }
}
