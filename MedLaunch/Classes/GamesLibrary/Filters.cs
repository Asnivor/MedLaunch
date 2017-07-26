using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MedLaunch.Classes.GamesLibrary
{
    public class Filters
    {
        public App _App { get; set; }

        public Filters()
        {
            _App = ((App)Application.Current);
        }

        /*
         * 
         * SEARCH FILTER
         * 
         * */
        public void Search(object item, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(_App.GamesLibrary.SearchText))
            {
                e.Accepted = false;
                return;
            }

            GamesLibraryModel g = e.Item as GamesLibraryModel;

            if (g != null)
            {
                if (g.Game != null && g.Game.ToUpper() != "" && g.Game.ToUpper().Contains(_App.GamesLibrary.SearchText.ToUpper()) ||
                    g.Flags != null && g.Flags.ToUpper() != "" && g.Flags.ToUpper().Contains(_App.GamesLibrary.SearchText.ToUpper()) ||
                    g.Year != null && g.Year.ToUpper() != "" && g.Year.ToUpper().Contains(_App.GamesLibrary.SearchText.ToUpper()) //||
                    //g.DatName != null && g.DatName.ToUpper() != "" && g.DatName.ToUpper().Contains(_App.GamesLibrary.SearchText.ToUpper()) ||
                    //g.DatRom != null && g.DatRom.ToUpper() != "" && g.DatRom.ToUpper().Contains(_App.GamesLibrary.SearchText.ToUpper()) ||
                    //g.ID.ToString().ToUpper() != "" && g.ID.ToString().ToUpper().Contains(_App.GamesLibrary.SearchText.ToUpper())
                    )
                { e.Accepted = true; }
                else { e.Accepted = false; }
            }
        }

        /*
         * 
         * COUNTRY FILTERS
         * 
         * */
                
        public void CountryUSA(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                if (g.Country != null)
                    e.Accepted = (g.Country.ToUpper().Contains("US")) ? true : false;
                else
                    e.Accepted = false;
            }
        }

        public void CountryEUR(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                if (g.Country != null)
                    e.Accepted = (g.Country.ToUpper().Contains("EU")) ? true : false;
                else
                    e.Accepted = false;
            }
        }

        public void CountryJPN(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                if (g.Country != null)
                    e.Accepted = (g.Country.ToUpper().Contains("J")) ? true : false;
                else
                    e.Accepted = false;
            }
        }


        /*
         * 
         * SYSTEM FILTERS
         * (Multually exclusive with each other but NOT other types of filter)
         * 
         * */
        public void ShowFavorites(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                e.Accepted = (g.Favorite) ? true : false;
            }         
        }
        
        public void ShowUnscraped(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                if (g.Coop == null &&
                    g.ESRB == null &&
                    g.Players == null)
                {
                    // possibly not scraped - check for psx
                    string system = g.System;
                    if (GSystem.GetSystemCode(GSystem.GetSystemId(g.System)).ToLower() != "psx")
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }        

        public void ShowNes(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 11) ? true : false;
            }
        }

        public void ShowSnes(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 12) ? true : false;
            }
        }

        public void ShowSms(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 10) ? true : false;
            }
        }

        public void ShowMd(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 4) ? true : false;
            }
        }

        public void ShowSs(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 13) ? true : false;
            }
        }

        public void ShowPsx(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 9) ? true : false;
            }
        }

        public void ShowPce(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 7) ? true : false;
            }
        }

        public void ShowPcecd(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 18) ? true : false;
            }
        }
        public void ShowPcfx(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 8) ? true : false;
            }
        }

        public void ShowVb(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 14) ? true : false;
            }
        }

        public void ShowNgp(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 6) ? true : false;
            }
        }

        public void ShowWswan(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 15) ? true : false;
            }
        }

        public void ShowGb(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 1) ? true : false;
            }
        }

        public void ShowGba(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 2) ? true : false;
            }
        }

        public void ShowGg(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 5) ? true : false;
            }
        }

        public void ShowLynx(object sender, FilterEventArgs e)
        {
            GamesLibraryModel g = e.Item as GamesLibraryModel;
            if (g != null)
            {
                int sysId = GSystem.GetSystemIdSubFirst(g.System);
                e.Accepted = (sysId == 3) ? true : false;
            }
        }
    }
}
