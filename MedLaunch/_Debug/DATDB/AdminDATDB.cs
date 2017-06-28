using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch._Debug.DATDB
{
    public class AdminDATDB
    {
        public MainWindow mw { get; set; }
        public List<DAT_System> systems { get; set; }

        public AdminDATDB()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            systems = DAT_System.GetSystems();
        }
    }
}
