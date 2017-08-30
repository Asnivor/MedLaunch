using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MedLaunch.Models;
using System.Windows.Media.Imaging;
using Ookii.Dialogs.Wpf;
using Microsoft.Win32;

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

        public string ImgPath { get; set; }
        public int ImageDisplayType { get; set; }
        // 0 = stretch
        // 1 = tile

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

            // image display type
            ImageDisplayType = GlobalSettings.getBgImageDisplayType();
            if (ImageDisplayType == 0)
                rbDtStretch.IsChecked = true;
            if (ImageDisplayType == 1)
                rbDtTile.IsChecked = true;

            // opacity
            ImgOpacity = GlobalSettings.GetBGImageOpacity();
            slOpac.Value = ImgOpacity;

            // set button text
            ImgPath = GlobalSettings.GetFullBGImagePath(null);
            tbImagePath.Text = ImgPath;

            // display the image
            DisplayImage();
        }

        private void DisplayImage()
        {
            try
            {
                ImageSource imageSource = new BitmapImage(new Uri(ImgPath));
                imgBgImage.Source = imageSource;
            }
            catch
            {
                // do nothing
            }
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

            // bgimage
            gs.bgImageDisplayType = ImageDisplayType;
            gs.bgImageOpacity = ImgOpacity;
            gs.bgImagePath = tbImagePath.Text;

            GlobalSettings.SetGlobals(gs);

            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw.SetBackgroundImage();

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

            // bgground image stuff
            ImgPath = GlobalSettings.GetFullBGImagePath(GlobalSettings.GetDefaultBeetlePath());
            DisplayImage();
            ImgOpacity = 0.1;
            slOpac.Value = ImgOpacity;
            rbDtStretch.IsChecked = true;
            ImageDisplayType = 0;
            tbImagePath.Text = ImgPath;

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

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filePath = new OpenFileDialog();
            filePath.Multiselect = false;
            filePath.Title = "Select Background Image";
            filePath.Filter = "Image files (*.jpg, *.jpeg, *.gif, *.bmp, *.png) | *.jpg; *.jpeg; *.gif; *.bmp; *.png";
            filePath.ShowDialog();

            if (filePath.FileName.Length > 0)
            {
                tbImagePath.Text = filePath.FileName;
            }
        }



        private void rbDtStretch_Click(object sender, RoutedEventArgs e)
        {
            ImageDisplayType = 0;
        }

        private void rbDtTile_Click(object sender, RoutedEventArgs e)
        {
            ImageDisplayType = 1;
        }

        private void btnResetToBeetle_Click(object sender, RoutedEventArgs e)
        {
            ImgPath = GlobalSettings.GetFullBGImagePath(GlobalSettings.GetDefaultBeetlePath());
            DisplayImage();
            tbImagePath.Text = ImgPath;
        }

        private void tbImagePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            string text = tb.Text;
            ImgPath = GlobalSettings.GetFullBGImagePath(text);
            DisplayImage();
        }

        public double ImgOpacity { get; set; }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            string text = tb.Text;
            double d = 0.1;
            if (text != "")
            {
                d = double.Parse(text);
            }           

            ImgOpacity = d;
        }
    }
}
