using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Extensions
{
    public static class LabelExtension
    {
        // if Label.Content is null - hide the label
        public static void SetVisibility(this Label label)
        {
            if (label.Content == null || label.Content.ToString() == "")
            {
                label.Visibility = Visibility.Collapsed;
            }
            else
            {
                label.Visibility = Visibility.Visible;
            }
        }
    }
}
