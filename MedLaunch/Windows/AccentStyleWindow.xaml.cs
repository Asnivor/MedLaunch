using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MedLaunch.Models;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for AccentStyleWindow.xaml
    /// </summary>
    public partial class AccentStyleWindow : MetroWindow
    {
        public static readonly DependencyProperty ColorsProperty
            = DependencyProperty.Register("Colors",
                                          typeof(List<KeyValuePair<string, Color>>),
                                          typeof(AccentStyleWindow),
                                          new PropertyMetadata(default(List<KeyValuePair<string, Color>>)));

        public List<KeyValuePair<string, Color>> Colors
        {
            get { return (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        public AccentStyleWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            this.Colors = typeof(Colors)
                .GetProperties()
                .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null)))
                .ToList();

            var theme = ThemeManager.DetectAppStyle(Application.Current);
            //ThemeManager.ChangeAppStyle(this, theme.Item2, theme.Item1);

           

        }
        
        private void ChangeAppThemeButtonClick(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme("Base" + ((Button)sender).Content));
        }

        private void ChangeAppAccentButtonClick(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(((Button)sender).Content.ToString()), theme.Item1);
        }       

        private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAccent = AccentSelector.SelectedItem as Accent;
            if (selectedAccent != null)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, selectedAccent, theme.Item1);
                Application.Current.MainWindow.Activate();
            }
        }        

        private void SaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            string bg = theme.Item1.Name;
            string c = theme.Item2.Name;

            GlobalSettings gs = GlobalSettings.GetGlobals();
            gs.colorBackground = bg;
            gs.colorAccent = c;

            GlobalSettings.SetGlobals(gs);
            this.Close();
        }

        private void ResetToDefault_Click(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            string[] colors = GlobalSettings.GetGUIColorsDefaults();

            ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent(colors[1]),
                                    ThemeManager.GetAppTheme(colors[0]));

            //set the dropdown
            foreach (Accent item in AccentSelector.Items)
            {
                if (item.Name == colors[1])
                {
                    AccentSelector.SelectedValue = item;
                    break;
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // load theme colors from database
            string[] colors = GlobalSettings.GetGUIColors();

            ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent(colors[1]),
                                    ThemeManager.GetAppTheme(colors[0]));

            this.Close();
        }

        private void AccentStyleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // set combobox
            string[] colors = GlobalSettings.GetGUIColors();
            foreach (Accent item in AccentSelector.Items)
            {
                if (item.Name == colors[1])
                {
                    AccentSelector.SelectedValue = item;
                    break;
                }
            }
        }
    }
}
