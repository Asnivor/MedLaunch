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
    public class GamesLibrarySorting
    {
        public static void OnSort()
        {

        }

        public static void AddSortColumn(DataGrid sender, string sortColumn, ListSortDirection direction)
        {
            var cView = CollectionViewSource.GetDefaultView(sender.ItemsSource);
            cView.SortDescriptions.Add(new SortDescription(sortColumn, direction));
            //Add the sort arrow on the DataGridColumn
            foreach (var col in sender.Columns.Where(x => x.SortMemberPath == sortColumn))
            {
                col.SortDirection = direction;
            }
        }
    }
}
