using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Models
{
    public class ConfigNetplaySettings
    {
        public int ConfigNPId { get; set; }
        public String netplay__console__font { get; set; }          // 5x7 6x9 6x12 6x13 9x18
        public int? netplay__console__lines { get; set; }            // 5 through 64
        public int? netplay__console__scale { get; set; }            // 0 through 16        
        public int? netplay__localplayers { get; set; }             // 0 through 16
        public String netplay__nick { get; set; }

        public static ConfigNetplaySettings GetNetplayDefaults()
        {
            ConfigNetplaySettings npSettings = new ConfigNetplaySettings
            {
                netplay__console__font = "9x18",
                netplay__console__scale = 1,
                netplay__console__lines = 5,
                netplay__localplayers = 1,
                netplay__nick = "RetroPlayer"
            };
            return npSettings;
        }

        public static void SaveToDatabase(List<ConfigNetplaySettings> Configs)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.ConfigNetplaySettings.AsNoTracking().ToList();

                List<ConfigNetplaySettings> toAdd = new List<ConfigNetplaySettings>();
                List<ConfigNetplaySettings> toUpdate = new List<ConfigNetplaySettings>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in Configs)
                {
                    ConfigNetplaySettings t = (from a in current
                                            where a.ConfigNPId == g.ConfigNPId
                                            select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.ConfigNetplaySettings.UpdateRange(toUpdate);
                db.ConfigNetplaySettings.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        // return Netplay Settings entry from DB
        public static ConfigNetplaySettings GetNetplay()
        {
            ConfigNetplaySettings nps = new ConfigNetplaySettings();
            using (var context = new MyDbContext())
            {
                var query = from s in context.ConfigNetplaySettings
                            where s.ConfigNPId == 1
                            select s;
                nps = query.FirstOrDefault();
            }
            return nps;
        }

        // write Global Settings object to DB
        public static void SetNetplay(ConfigNetplaySettings nps)
        {
            using (var context = new MyDbContext())
            {
                context.ConfigNetplaySettings.Attach(nps);
                var entry = context.Entry(nps);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void LoadNetplaySettings(TextBox Nickname, Slider LocalPlayers, Slider ConsoleLines, Slider ConsoleScale,
            RadioButton resOne, RadioButton resTwo, RadioButton resThree, RadioButton resFour, RadioButton resFive)
        {
            ConfigNetplaySettings nps = GetNetplay();

            Nickname.Text = nps.netplay__nick;
            LocalPlayers.Value = Convert.ToDouble(nps.netplay__localplayers);
            ConsoleLines.Value = Convert.ToDouble(nps.netplay__console__lines);
            ConsoleScale.Value = Convert.ToDouble(nps.netplay__console__scale);

            if (nps.netplay__console__font == "5x7")
                resOne.IsChecked = true;
            if (nps.netplay__console__font == "6x9")
                resTwo.IsChecked = true;
            if (nps.netplay__console__font == "6x12")
                resThree.IsChecked = true;
            if (nps.netplay__console__font == "6x13")
                resFour.IsChecked = true;
            if (nps.netplay__console__font == "9x18")
                resFive.IsChecked = true;
        }

        public static void LoadNetplaySettings()
        {
            ConfigNetplaySettings nps = GetNetplay();
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            TextBox tbNetplayNick = (TextBox)mw.FindName("tbNetplayNick");
            Slider slLocalPlayersValue = (Slider)mw.FindName("slLocalPlayersValue");
            Slider slConsoleLinesValue = (Slider)mw.FindName("slConsoleLinesValue");
            Slider slConsoleScaleValue = (Slider)mw.FindName("slConsoleScaleValue");
            RadioButton resOne = (RadioButton)mw.FindName("resOne");
            RadioButton resTwo = (RadioButton)mw.FindName("resTwo");
            RadioButton resThree = (RadioButton)mw.FindName("resThree");
            RadioButton resFour = (RadioButton)mw.FindName("resFour");
            RadioButton resFive = (RadioButton)mw.FindName("resFive");

            tbNetplayNick.Text = nps.netplay__nick;
            slLocalPlayersValue.Value = Convert.ToDouble(nps.netplay__localplayers);
            slConsoleLinesValue.Value = Convert.ToDouble(nps.netplay__console__lines);
            slConsoleScaleValue.Value = Convert.ToDouble(nps.netplay__console__scale);

            if (nps.netplay__console__font == "5x7")
                resOne.IsChecked = true;
            if (nps.netplay__console__font == "6x9")
                resTwo.IsChecked = true;
            if (nps.netplay__console__font == "6x12")
                resThree.IsChecked = true;
            if (nps.netplay__console__font == "6x13")
                resFour.IsChecked = true;
            if (nps.netplay__console__font == "9x18")
                resFive.IsChecked = true;
        }

        public static void SaveNetplaySettings(TextBox Nickname, Slider LocalPlayers, Slider ConsoleLines, Slider ConsoleScale,
            RadioButton resOne, RadioButton resTwo, RadioButton resThree, RadioButton resFour, RadioButton resFive)
        {
            ConfigNetplaySettings nps = GetNetplay();

            nps.netplay__nick = Nickname.Text;
            nps.netplay__localplayers = Convert.ToInt32(LocalPlayers.Value);
            nps.netplay__console__lines = Convert.ToInt32(ConsoleLines.Value);
            nps.netplay__console__scale = Convert.ToInt32(ConsoleScale.Value);

            if (resOne.IsChecked == true)
                nps.netplay__console__font = "5x7";
            if (resTwo.IsChecked == true)
                nps.netplay__console__font = "6x9";
            if (resThree.IsChecked == true)
                nps.netplay__console__font = "6x12";
            if (resFour.IsChecked == true)
                nps.netplay__console__font = "6x13";
            if (resFive.IsChecked == true)
                nps.netplay__console__font = "9x18";

            SetNetplay(nps);
        }

        public static void SaveNetplaySettings()
        {
            ConfigNetplaySettings nps = GetNetplay();

            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            TextBox tbNetplayNick = (TextBox)mw.FindName("tbNetplayNick");
            Slider slLocalPlayersValue = (Slider)mw.FindName("slLocalPlayersValue");
            Slider slConsoleLinesValue = (Slider)mw.FindName("slConsoleLinesValue");
            Slider slConsoleScaleValue = (Slider)mw.FindName("slConsoleScaleValue");
            RadioButton resOne = (RadioButton)mw.FindName("resOne");
            RadioButton resTwo = (RadioButton)mw.FindName("resTwo");
            RadioButton resThree = (RadioButton)mw.FindName("resThree");
            RadioButton resFour = (RadioButton)mw.FindName("resFour");
            RadioButton resFive = (RadioButton)mw.FindName("resFive");

            nps.netplay__nick = tbNetplayNick.Text;
            nps.netplay__localplayers = Convert.ToInt32(slLocalPlayersValue.Value);
            nps.netplay__console__lines = Convert.ToInt32(slConsoleLinesValue.Value);
            nps.netplay__console__scale = Convert.ToInt32(slConsoleScaleValue.Value);

            if (resOne.IsChecked == true)
                nps.netplay__console__font = "5x7";
            if (resTwo.IsChecked == true)
                nps.netplay__console__font = "6x9";
            if (resThree.IsChecked == true)
                nps.netplay__console__font = "6x12";
            if (resFour.IsChecked == true)
                nps.netplay__console__font = "6x13";
            if (resFive.IsChecked == true)
                nps.netplay__console__font = "9x18";

            SetNetplay(nps);
        }




    }
}
