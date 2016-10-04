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

namespace MedLaunch.Classes
{
    public class UIHandler
    {
        public List<Button> Buttons { get; set; }
        public List<RadioButton> RadioButtons { get; set; }
        public List<Label> Labels { get; set; }
        public List<CheckBox> CheckBoxes { get; set; }
        public List<TextBox> TextBoxes { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<ComboBox> ComboBoxes { get; set; }
        public List<NumericUpDown> NumericUpDowns { get; set; }
        public List<ToggleButton> ToggleButtons { get; set; }

       

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
    }
}
