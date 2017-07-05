using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB.Platforms.TOSEC.Models
{
    public class ToSecCollection
    {
        public List<DAT_Rom> Data { get; set; }

        public ToSecCollection(int platformId)
        {
            Data = new List<DAT_Rom>();
            if (platformId == 0)
                Data = ImportToSecData.Go();
            else
                Data = ImportToSecData.Go(platformId);
        }
    }
}
