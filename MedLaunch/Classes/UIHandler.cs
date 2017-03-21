using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MedLaunch.Models;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using MedLaunch.Classes.Controls;

namespace MedLaunch.Classes
{
    public class UIHandler
    {
        // properties
        public List<Button> Buttons { get; set; }
        public List<RadioButton> RadioButtons { get; set; }
        public List<Label> Labels { get; set; }
        public List<CheckBox> CheckBoxes { get; set; }
        public List<TextBox> TextBoxes { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<ComboBox> ComboBoxes { get; set; }
        public List<NumericUpDown> NumericUpDowns { get; set; }
        public List<ToggleButton> ToggleButtons { get; set; }
        public List<ColorPicker> Colorpickers { get; set; }
        //public List<InputWidget> InputWidgets { get; set; }



        // Populate UIHandler Object
        public static UIHandler GetChildren(WrapPanel wp)
        {                   
            // create a new instance of UIHandler class
            UIHandler ui = new UIHandler();

            // get all child buttons
            ui.Buttons = GetLogicalChildCollection<Button>(wp);
            ui.RadioButtons = GetLogicalChildCollection<RadioButton>(wp);
            ui.Labels = GetLogicalChildCollection<Label>(wp);
            ui.CheckBoxes = GetLogicalChildCollection<CheckBox>(wp);
            ui.TextBoxes = GetLogicalChildCollection<TextBox>(wp);
            ui.Sliders = GetLogicalChildCollection<Slider>(wp);
            ui.ComboBoxes = GetLogicalChildCollection<ComboBox>(wp);
            ui.NumericUpDowns = GetLogicalChildCollection<NumericUpDown>(wp);
            ui.Colorpickers = GetLogicalChildCollection<ColorPicker>(wp);
            //ui.InputWidgets = GetLogicalChildCollection<InputWidget>(wp);

            return ui;
        }
        public static UIHandler GetChildren(StackPanel wp)
        {
            // create a new instance of UIHandler class
            UIHandler ui = new UIHandler();

            // get all child buttons
            ui.Buttons = GetLogicalChildCollection<Button>(wp);
            ui.RadioButtons = GetLogicalChildCollection<RadioButton>(wp);
            ui.Labels = GetLogicalChildCollection<Label>(wp);
            ui.CheckBoxes = GetLogicalChildCollection<CheckBox>(wp);
            ui.TextBoxes = GetLogicalChildCollection<TextBox>(wp);
            ui.Sliders = GetLogicalChildCollection<Slider>(wp);
            ui.ComboBoxes = GetLogicalChildCollection<ComboBox>(wp);
            ui.NumericUpDowns = GetLogicalChildCollection<NumericUpDown>(wp);
            ui.Colorpickers = GetLogicalChildCollection<ColorPicker>(wp);
            //ui.InputWidgets = GetLogicalChildCollection<InputWidget>(wp);

            return ui;
        }
        public static UIHandler GetChildren(Grid wp)
        {
            // create a new instance of UIHandler class
            UIHandler ui = new UIHandler();

            // get all child buttons
            ui.Buttons = GetLogicalChildCollection<Button>(wp);
            ui.RadioButtons = GetLogicalChildCollection<RadioButton>(wp);
            ui.Labels = GetLogicalChildCollection<Label>(wp);
            ui.CheckBoxes = GetLogicalChildCollection<CheckBox>(wp);
            ui.TextBoxes = GetLogicalChildCollection<TextBox>(wp);
            ui.Sliders = GetLogicalChildCollection<Slider>(wp);
            ui.ComboBoxes = GetLogicalChildCollection<ComboBox>(wp);
            ui.NumericUpDowns = GetLogicalChildCollection<NumericUpDown>(wp);
            ui.Colorpickers = GetLogicalChildCollection<ColorPicker>(wp);
            //ui.InputWidgets = GetLogicalChildCollection<InputWidget>(wp);

            return ui;
        }
        public static UIHandler GetChildren(Panel wp)
        {
            // create a new instance of UIHandler class
            UIHandler ui = new UIHandler();

            // get all child buttons
            ui.Buttons = GetLogicalChildCollection<Button>(wp);
            ui.RadioButtons = GetLogicalChildCollection<RadioButton>(wp);
            ui.Labels = GetLogicalChildCollection<Label>(wp);
            ui.CheckBoxes = GetLogicalChildCollection<CheckBox>(wp);
            ui.TextBoxes = GetLogicalChildCollection<TextBox>(wp);
            ui.Sliders = GetLogicalChildCollection<Slider>(wp);
            ui.ComboBoxes = GetLogicalChildCollection<ComboBox>(wp);
            ui.NumericUpDowns = GetLogicalChildCollection<NumericUpDown>(wp);
            ui.Colorpickers = GetLogicalChildCollection<ColorPicker>(wp);
            //ui.InputWidgets = GetLogicalChildCollection<InputWidget>(wp);

            return ui;
        }
        public static UIHandler GetChildren(DockPanel wp)
        {
            // create a new instance of UIHandler class
            UIHandler ui = new UIHandler();

            // get all child buttons
            ui.Buttons = GetLogicalChildCollection<Button>(wp);
            ui.RadioButtons = GetLogicalChildCollection<RadioButton>(wp);
            ui.Labels = GetLogicalChildCollection<Label>(wp);
            ui.CheckBoxes = GetLogicalChildCollection<CheckBox>(wp);
            ui.TextBoxes = GetLogicalChildCollection<TextBox>(wp);
            ui.Sliders = GetLogicalChildCollection<Slider>(wp);
            ui.ComboBoxes = GetLogicalChildCollection<ComboBox>(wp);
            ui.NumericUpDowns = GetLogicalChildCollection<NumericUpDown>(wp);
            ui.Colorpickers = GetLogicalChildCollection<ColorPicker>(wp);
            //ui.InputWidgets = GetLogicalChildCollection<InputWidget>(wp);

            return ui;
        }

        public static UIHandler GetChildren(Border wp)
        {
            // create a new instance of UIHandler class
            UIHandler ui = new UIHandler();

            // get all child buttons
            ui.Buttons = GetLogicalChildCollection<Button>(wp);
            ui.RadioButtons = GetLogicalChildCollection<RadioButton>(wp);
            ui.Labels = GetLogicalChildCollection<Label>(wp);
            ui.CheckBoxes = GetLogicalChildCollection<CheckBox>(wp);
            ui.TextBoxes = GetLogicalChildCollection<TextBox>(wp);
            ui.Sliders = GetLogicalChildCollection<Slider>(wp);
            ui.ComboBoxes = GetLogicalChildCollection<ComboBox>(wp);
            ui.NumericUpDowns = GetLogicalChildCollection<NumericUpDown>(wp);
            ui.Colorpickers = GetLogicalChildCollection<ColorPicker>(wp);
            //ui.InputWidgets = GetLogicalChildCollection<InputWidget>(wp);

            return ui;
        }

        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        public static List<T> GetVisualChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : DependencyObject
        {
            DependencyObject children;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                children = VisualTreeHelper.GetChild(parent, i);
                
            }
            
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent)
        where T : DependencyObject
        {
            //int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                var childType = child as T;
                if (childType != null)
                {
                    yield return (T)child;
                }

                foreach (var other in FindVisualChildren<T>(child))
                {
                    yield return other;
                }
            }
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
