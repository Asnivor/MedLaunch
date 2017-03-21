using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MedLaunch.Classes.GamesLibrary
{
    public class ColumnInfo
    {
        /// <summary>
        /// empty constructor
        /// </summary>
        public ColumnInfo()
        {
            Header = null;
            PropertyPath = null;
            DisplayIndex = 0;
            SortDirection = new ListSortDirection();
            WidthValue = 0;
        }

        /// <summary>
        /// Constructor that takes an actual datagrid column
        /// </summary>
        /// <param name="column"></param>
        public ColumnInfo(DataGridColumn column)
        {
            Header = column.Header;
            PropertyPath = ((Binding)((DataGridBoundColumn)column).Binding).Path.Path;
            WidthValue = column.Width.DisplayValue;
            WidthType = column.Width.UnitType;
            SortDirection = column.SortDirection;
            DisplayIndex = column.DisplayIndex;
        }

        public static void ApplyColumnInfo(DataGrid dataGrid, ColumnInfoObject colInfoList)
        {
            App _App = (App)Application.Current;

            // apply the column settings
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                ColumnInfo ci = new ColumnInfo();
                var lookup = (from a in colInfoList.ColumnInfoList
                             where a.PropertyPath == ((Binding)((DataGridBoundColumn)dataGrid.Columns[i]).Binding).Path.Path
                             //where a.Header == dataGrid.Columns[i].Header
                             select a).FirstOrDefault();

                if (lookup == null)
                    continue;

                if (lookup.DisplayIndex == -1)
                    continue;

                // clear existing                
                dataGrid.Columns[i].SortDirection = null;
                
                // set the sortdirection on the datagrid itself
                dataGrid.Columns[i].SortDirection = lookup.SortDirection;

                // width and display index
                dataGrid.Columns[i].DisplayIndex = lookup.DisplayIndex;
                dataGrid.Columns[i].Width = new DataGridLength(lookup.WidthValue, lookup.WidthType);                
            }

            // now apply the sort descriptions
            using (_App.GamesLibrary.LibraryView.DeferRefresh())
            {
                _App.GamesLibrary.LibraryView.SortDescriptions.Clear();
                for (int i = 0; i < colInfoList.SortDescriptionList.Count; i++)
                {
                    _App.GamesLibrary.LibraryView.SortDescriptions.Add(new SortDescription(colInfoList.SortDescriptionList[i].PropertyName, colInfoList.SortDescriptionList[i].Direction));
                }
            }
        }

        public static ColumnInfoObject GetColumnInfo(DataGrid dataGrid)
        {
            App _App = (App)Application.Current;
            ColumnInfoObject coo = new ColumnInfoObject();

            List<ColumnInfo> list = new List<ColumnInfo>();

            foreach (DataGridColumn c in dataGrid.Columns)
            {
                ColumnInfo ci = new ColumnInfo(c);
                list.Add(ci);
            }

            coo.ColumnInfoList = list;

            // get sort descriptions from view
            coo.SortDescriptionList = new Dictionary<int, SortDescription>();
            for (int i = 0; i < _App.GamesLibrary.LibraryView.SortDescriptions.Count; i++)
            {
                coo.SortDescriptionList.Add(i, _App.GamesLibrary.LibraryView.SortDescriptions[i]);
            }
            
            return coo;
        }

        public void Apply(DataGridColumn column, int gridColumnCount, SortDescriptionCollection sortDescriptions)
        {
            column.Width = new DataGridLength(WidthValue, WidthType);
            column.SortDirection = SortDirection;
            if (SortDirection != null)
            {
                sortDescriptions.Add(new SortDescription(PropertyPath, SortDirection.Value));
            }

            column.DisplayIndex = DisplayIndex;
            /*
            if (column.DisplayIndex != DisplayIndex)
            {
                var maxIndex = (gridColumnCount == 0) ? 0 : gridColumnCount - 1;
                column.DisplayIndex = (DisplayIndex <= maxIndex) ? DisplayIndex : maxIndex;
            }
            */
        }
        public object Header;
        public string PropertyPath;
        public ListSortDirection? SortDirection;
        public int DisplayIndex;
        public double WidthValue;
        public DataGridLengthUnitType WidthType;
    }

    public class ColumnInfoObject
    {
        public int FilterNumber { get; set; }
        public List<ColumnInfo> ColumnInfoList { get; set; }
        public Dictionary<int, SortDescription> SortDescriptionList { get; set; }

        public ColumnInfoObject()
        {
            ColumnInfoList = new List<ColumnInfo>();
            SortDescriptionList = new Dictionary<int, SortDescription>();
        }
    }
}
