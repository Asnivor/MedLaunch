using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB.Platforms.PSXDATACENTER.Models
{
    public class PsxDataCenterCollection
    {
        // Properties
        public List<DAT_Rom> Data { get; set; }

        public PsxDataCenterCollection()
        {
            Data = new List<DAT_Rom>();

            Data = ImportPsxDataCenterdata.Go();
        }
    }
}
