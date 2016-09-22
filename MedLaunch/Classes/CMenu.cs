using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedLaunch.Classes
{
    public static class CMenu
    {
        // generate context menu for ROM list display
        public static ContextMenu BuildGamesMenu(DataGrid dgGameList)
        {
            // Create new context menu object
            ContextMenu romMenu = new ContextMenu();

            // Play Game
            MenuItem menu1 = new MenuItem();
            menu1.Header = "Test";


            // get the selected item
            var selected = dgGameList.SelectedItems;
                           
            foreach (var item in selected)
            {
                Debug.WriteLine(item);
            }


            return romMenu;
            
        }
    }
}
