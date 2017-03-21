using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GamesLibraryView
    {
        public static GamesLibraryModel SelectedGame { get; set; }
        public static DataGrid DG { get; set; }
        public static App _App { get; set; }       

        public static void StoreSelectedRow(DataGrid dataGrid)
        {
            SelectedGame = null;

            // init datagrid property
            if (DG == null)
                DG = dataGrid;

            // get the model selected item
            int count = DG.SelectedItems.Count;
            var items = DG.SelectedItems;

            List<GamesLibraryModel> glList = new List<GamesLibraryModel>();
            if (count != 0)
            {
                foreach (GamesLibraryModel m in items)
                {
                    glList.Add(m);
                }
            }
            else
                return;

            // add the first entry
            SelectedGame = glList.First();   
        }

        public static void RestoreSelectedRow()
        {
            GamesLibraryModel m = SelectedGame;

            // iterate through each row in the datagrid
            int count = DG.Items.Count;
            for (int i = 0; i < count; i++)
            {
                GamesLibraryModel game = DG.Items[i] as GamesLibraryModel;
                if (game.ID == m.ID)
                {
                    GamesLibraryModel lo = DG.Items.CurrentItem as GamesLibraryModel;
                    if (lo.ID != game.ID)
                    {
                        // selection has skipped on 1 entry to far
                        //SelectRowByIndex(DG, (i - 1));
                        DG.Items.MoveCurrentToPrevious();
                    }
                    else
                    {
                        // match found                      

                        SelectRowByIndex(DG, i);
                    }
                    
                    break;
                }
            }
        }
        
        public static void SelectRowByIndex(DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            dataGrid.SelectedItems.Clear();
            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            dataGrid.SelectedItem = item;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                /* bring the data item (Product object) into view
                 * in case it has been virtualized away */
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            if (row != null)
            {
                DataGridCell cell = GetCell(dataGrid, row, 0);
                if (cell != null)
                    cell.Focus();
            }
        }

        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = UIHandler.FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = UIHandler.FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }

        
    }
}
