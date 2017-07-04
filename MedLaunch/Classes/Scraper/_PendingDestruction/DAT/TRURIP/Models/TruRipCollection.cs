using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DAT.TRURIP.Models
{
    public class TruRipCollection
    {
        // Properties
        public List<TruRipObject> Data { get; set; }

        // Constructors
        public TruRipCollection()
        {
            Data = new List<TruRipObject>();

            Data = ImportTruRipData.Go();
        }
    }
}
