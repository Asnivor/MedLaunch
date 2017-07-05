using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DAT.TOSEC.Models
{
    public class ToSecCollection
    {
        // Properties
        public List<ToSecObject> Data { get; set; }

        // Constructors
        public ToSecCollection()
        {
            Data = new List<ToSecObject>();

            Data = ImportToSecData.Go();
        }
    }
}
