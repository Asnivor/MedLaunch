using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch.Classes
{
    /// <summary>
    /// Just some static methods to pop various messageboxes
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// To be called when a user tries to configure a controller that has not been
        /// implemented when targeting mednafen < 1.21.x
        /// </summary>
        public static void PopControllerTargetingIssue()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Sorry, MedLaunch only supports configuring this controller if you are \ntargeting a new enough version of mednafen.");
            sb.Append("\n\n");
            sb.Append("Current Version Targeted:\t\t" + VersionChecker.Instance.CurrentMedVerDesc.FullVersionString);
            sb.Append("\n");
            sb.Append("Minimum Version Required:\t1.21.0");
            sb.Append("\n\n");
            sb.Append("Please target a mednafen folder that contains a new enough version...");

            string message = sb.ToString();
            string header = "FEATURE NOT IMPLEMENTED";

            ShowMahappsMessageDialog(message, header);
        }

        /// <summary>
        /// Use mahapps dialog to show message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="header"></param>
        public static async void ShowMahappsMessageDialog(string message, string header)
        {
            MetroDialogSettings settings = new MetroDialogSettings
            {
                AnimateShow = false,
                AnimateHide = false,                 
            };

            await GetMainWindow().ShowMessageAsync(header, message, MessageDialogStyle.Affirmative, settings);
        }
       
        /// <summary>
        /// Helper method to get the application mainwindow
        /// </summary>
        /// <returns></returns>
        private static MainWindow GetMainWindow()
        {
            return Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }
    }
}
