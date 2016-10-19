using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Extensions
{
    public static class ImageExtension
    {
        // If Image.Source is null hide (collapse) the image
        public static void SetVisibility(this Image image)
        {
            if (image.Source == null)
            {
                image.Visibility = Visibility.Collapsed;
            }
            else
            {
                image.Visibility = Visibility.Visible;
            }
        }
    }
}
