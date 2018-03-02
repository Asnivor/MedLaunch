using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Models;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{
    /// <summary>
    /// Init class
    /// </summary>
    public class Initialisation
    {
        public static void Go(MainWindow mw)
        {
            // start async UI
            PopUI(mw);
        }

        private async static void PopUI(MainWindow mw)
        {
            // instantiate Progress Dialog
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = true,
                AnimateHide = true,

            };
            string message = "Please wait...";
            var controller = await mw.ShowProgressAsync("MedLaunch Initialisation", message, settings: mySettings);
            controller.SetCancelable(false);
            await Task.Delay(300);

            await controller.CloseAsync();
        }

        private static void CreateDatabase(MainWindow mw)
        {
            // initialise SQLite db if it does not already exist
            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
                // populate stock data 
                DbEF.InitialSeed();
                context.SaveChanges();
            }
        }
    }
}
