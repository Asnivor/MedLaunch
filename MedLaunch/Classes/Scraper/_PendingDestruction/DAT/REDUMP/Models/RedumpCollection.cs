using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DAT.REDUMP.Models
{
    public class RedumpCollection
    {
        // Properties
        public List<RedumpObject> Data { get; set; }

        // Constructors
        public RedumpCollection()
        {
            Data = new List<RedumpObject>();

            Data = ImportRedumpData.Go();
        }
    }
}
