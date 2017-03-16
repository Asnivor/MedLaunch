using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static void ApplyColumnInfo(DataGrid dataGrid, List<ColumnInfo> colInfoList)
        {
            //List<DataGridColumn> columns = dataGrid.Columns.OrderBy(a => a.DisplayIndex).ToList();
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                ColumnInfo ci = new ColumnInfo();
                var lookup = (from a in colInfoList
                             //where a.PropertyPath == ((Binding)((DataGridBoundColumn)dataGrid.Columns[i]).Binding).Path.Path
                             where a.Header == dataGrid.Columns[i].Header
                             select a).FirstOrDefault();

                if (lookup == null)
                    continue;

                if (lookup.DisplayIndex == -1)
                    continue;

                dataGrid.Columns[i].SortDirection = lookup.SortDirection;
                dataGrid.Columns[i].DisplayIndex = lookup.DisplayIndex;
                dataGrid.Columns[i].Width = new DataGridLength(lookup.WidthValue, lookup.WidthType);
            }
        }

        public static List<ColumnInfo> GetColumnInfo(DataGrid dataGrid)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();
            foreach (DataGridColumn c in dataGrid.Columns)
            {
                ColumnInfo ci = new ColumnInfo(c);
                list.Add(ci);
            }

            return list;
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
}
