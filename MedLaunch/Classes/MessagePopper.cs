using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedLaunch.Classes
{
    /// <summary>
    /// Just some static methods to pop various messageboxes
    /// </summary>
    public class MessagePopper
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

            //ShowMahappsMessageDialog(message, header);
            ShowMessageDialog(message, header);
        }

        public static void PopControllerNotFound()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("This option is not configurable");

            string message = sb.ToString();
            string header = "INPUT NOT FOUND";

            //ShowMahappsMessageDialog(message, header);
            ShowMessageDialog(message, header);
        }

        /*
        /// <summary>
        /// Use mahapps dialog to show message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="header"></param>
        public static void ShowMahappsMessageDialog(string message, string header)
        {
            MetroDialogSettings settings = new MetroDialogSettings
            {
                AnimateShow = false,
                AnimateHide = false,  
                               
            };

            MainWindow mw;

            // when medlaunch is starting up, the mainwindow may not be visible yet
            // if this is the case, then use standard MessageBox..
            try
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    // we are already on the UI thread
                    mw = GetMainWindow();
                    mw.ShowModalMessageExternal(header, message, MessageDialogStyle.Affirmative, settings);
                }
                else
                {
                    // re-invoke with UI access
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        mw = GetMainWindow();
                        mw.ShowModalMessageExternal(header, message, MessageDialogStyle.Affirmative, settings);
                    }));
                }
            }
            catch (Exception)
            {
                // there was an issue opening the mahapps message dialog
                // fall back to standard MessageBox
                MessageBox.Show(message, header);
            }
        }
        */

        /// <summary>
        /// Use mahapps external modal dialog to show a messagebox and return custom result enum
        /// Will fall back to using standard MessageBox if MahApps cannot display
        /// </summary>
        /// <param name="message"></param>
        /// <param name="header"></param>
        public static ReturnResult ShowMessageDialog(string message, string header, DialogButtonOptions buttonOptions = DialogButtonOptions.YES, MetroDialogSettings settings = null)
        {
            // use default settings if they havent been supplied
            if (settings == null)
            {
                settings = new MetroDialogSettings
                {
                    AnimateShow = false,
                    AnimateHide = false,
                };
            }

            MainWindow mw;
            
            // convert buttonOptions
            MessageDialogStyle btnOption = MessageDialogStyle.AffirmativeAndNegative;
            MessageBoxButton btn = MessageBoxButton.OK;
            switch (buttonOptions)
            {
                case DialogButtonOptions.YES:
                case DialogButtonOptions.OK:
                    btnOption = MessageDialogStyle.Affirmative;
                    btn = MessageBoxButton.OK;
                    break;
                case DialogButtonOptions.YESNO:
                    btnOption = MessageDialogStyle.AffirmativeAndNegative;
                    btn = MessageBoxButton.YesNo;
                    break;
                case DialogButtonOptions.YESNOPLUS1:
                    btnOption = MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary;
                    btn = MessageBoxButton.YesNoCancel;
                    break;
                case DialogButtonOptions.YESNOPLUS2:
                    btnOption = MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary;
                    break;
                case DialogButtonOptions.OKCANCEL:
                    btnOption = MessageDialogStyle.AffirmativeAndNegative;
                    btn = MessageBoxButton.OKCancel;
                    break;
                case DialogButtonOptions.YESNOCANCEL:
                    btnOption = MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary;
                    btn = MessageBoxButton.YesNoCancel;
                    break;
            }

            // set default return result
            ReturnResult result = ReturnResult.Negative;
            switch (btnOption)
            {
                case MessageDialogStyle.Affirmative:
                    result = ReturnResult.Affirmative;
                    break;
                case MessageDialogStyle.AffirmativeAndNegative:
                    result = ReturnResult.Negative;
                    break;
                default:
                    result = ReturnResult.FirstAux;
                    break;
            }

            // when medlaunch is starting up, the mainwindow may not be visible yet
            // if this is the case, then use standard MessageBox..
            try
            {
                MessageDialogResult mahRes;

                // check whether we are on the UI thread first
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    // we are already on the UI thread
                    mw = GetMainWindow();
                    mahRes = mw.ShowModalMessageExternal(header, message, btnOption, settings);

                    // convert the result
                    switch (mahRes)
                    {
                        case MessageDialogResult.Affirmative:
                            result = ReturnResult.Affirmative;
                            break;
                        case MessageDialogResult.Negative:
                            result = ReturnResult.Negative;
                            break;
                        case MessageDialogResult.FirstAuxiliary:
                            result = ReturnResult.FirstAux;
                            break;
                        case MessageDialogResult.SecondAuxiliary:
                            result = ReturnResult.SecondAux;
                            break;
                    }
                }
                else
                {
                    // re-invoke with UI access
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        mw = GetMainWindow();
                        mahRes = mw.ShowModalMessageExternal(header, message, btnOption, settings);

                        // convert the result
                        switch (mahRes)
                        {
                            case MessageDialogResult.Affirmative:
                                result = ReturnResult.Affirmative;
                                break;
                            case MessageDialogResult.Negative:
                                result = ReturnResult.Negative;
                                break;
                            case MessageDialogResult.FirstAuxiliary:
                                result = ReturnResult.FirstAux;
                                break;
                            case MessageDialogResult.SecondAuxiliary:
                                result = ReturnResult.SecondAux;
                                break;
                        }
                    }));
                }
            }
            catch (Exception)
            {
                // there was an issue opening the mahapps message dialog
                // fall back to standard MessageBox
                var msgRes = MessageBox.Show(message, header, btn, MessageBoxImage.Asterisk);

                // convert the result
                switch (msgRes)
                {
                    case MessageBoxResult.OK:
                    case MessageBoxResult.Yes:
                        result = ReturnResult.Affirmative;
                        break;
                    case MessageBoxResult.No:
                        result = ReturnResult.Negative;
                        break;
                    case MessageBoxResult.Cancel:
                        result = ReturnResult.FirstAux;
                        break;
                    case MessageBoxResult.None:
                        result = ReturnResult.Negative;
                        break;                       
                }
            }

            return result;   
        }

        /// <summary>
        /// Helper method to get the application mainwindow
        /// </summary>
        /// <returns></returns>
        private static MainWindow GetMainWindow()
        {
            return Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        public enum DialogButtonOptions
        {
            YES,
            YESNO,
            YESNOPLUS1,
            YESNOPLUS2,
            OK,
            OKCANCEL,
            YESNOCANCEL                        
        }

        public enum ReturnResult
        {
            Affirmative,
            Negative,
            FirstAux,
            SecondAux
        }
    }
}
