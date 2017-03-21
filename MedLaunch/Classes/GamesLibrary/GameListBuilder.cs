using MedLaunch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GameListBuilder //: INotifyPropertyChanged
    {
        // properties

        /*

    public string SortColumnName { get; set; }
    public ListSortDirection SortDirection { get; set; }
    public string DGStatesPath { get; set; }

    public CountryFilter countryFilter { get; set; }

    public List<DataGridGamesView> FilteredSetCache;

    private ObservableCollection<DataGridGamesView> filteredSet;
    public ObservableCollection<DataGridGamesView> FilteredSet
    {
        get
        {
            return filteredSet;
        }
        set
        {
            if (filteredSet != value)
            {
                filteredSet = value;
                OnPropertyChanged("FilteredSet");
            }
        }
    }

    public Dictionary<int, List<ColumnInfo>> DataGridStates { get; set; }


    public List<DataGridGamesView> AllGames { get; set; }
    public int SystemId { get; set; }
    public string SearchString { get; set; }
    public bool UpdateRequired { get; set; }

    // constructors

    public GameListBuilder()
    {
        // instantiate new AllGames object for the first time
        AllGames = new List<DataGridGamesView>();

        // populate the object
        AllGames = GamesLibraryDataGridRefresh.Update(AllGames);

        // country filter initial population
        countryFilter = GamesLibraryVisualHandler.GetSelectedCountryFilter();

        DGStatesPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\GamesLibraryColumnStates.json";

        // set initial sorting
        SortColumnName = "GAME";
        SortDirection = ListSortDirection.Ascending;

        UpdateRequired = false;

        if (DataGridStates == null)
        {
            // load defaults from disk
            DataGridStates = new Dictionary<int, List<ColumnInfo>>();
            DataGridStates.Add(1, new List<ColumnInfo>());
            DataGridStates.Add(2, new List<ColumnInfo>());
            DataGridStates.Add(3, new List<ColumnInfo>());
            DataGridStates.Add(4, new List<ColumnInfo>());
            DataGridStates.Add(5, new List<ColumnInfo>());
            DataGridStates.Add(6, new List<ColumnInfo>());
            DataGridStates.Add(7, new List<ColumnInfo>());
            DataGridStates.Add(8, new List<ColumnInfo>());
            DataGridStates.Add(9, new List<ColumnInfo>());
            DataGridStates.Add(10, new List<ColumnInfo>());
            DataGridStates.Add(11, new List<ColumnInfo>());
            DataGridStates.Add(12, new List<ColumnInfo>());
            DataGridStates.Add(13, new List<ColumnInfo>());
            DataGridStates.Add(14, new List<ColumnInfo>());
            DataGridStates.Add(15, new List<ColumnInfo>());
            DataGridStates.Add(16, new List<ColumnInfo>());
            DataGridStates.Add(17, new List<ColumnInfo>());
            DataGridStates.Add(18, new List<ColumnInfo>());
            DataGridStates.Add(19, new List<ColumnInfo>());
            DataGridStates.Add(20, new List<ColumnInfo>());
            DataGridStates.Add(21, new List<ColumnInfo>());
            DataGridStates.Add(22, new List<ColumnInfo>());
            DataGridStates.Add(23, new List<ColumnInfo>());
            DataGridStates.Add(24, new List<ColumnInfo>());
            DataGridStates.Add(25, new List<ColumnInfo>());

            LoadColumnDefaults();
        }

        LoadDataGridStatesFromDisk();
    }

    public void LoadColumnDefaults()
    {
        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json"))
        {
            string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json");
            var dict = JsonConvert.DeserializeObject<Dictionary<int, List<ColumnInfo>>>(json);
            for (int i = 0; i < DataGridStates.Count; i++)
            {
                DataGridStates[i + 1] = dict[1];
            }
        }
    }

    public void LoadColumnDefaults(int filterNumber)
    {
        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json"))
        {
            string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json");
            var dict = JsonConvert.DeserializeObject<Dictionary<int, List<ColumnInfo>>>(json);

            DataGridStates[filterNumber] = dict[1];                
        }
    }

    public void SaveDataGridStatesToDisk()
    {
        // convert to json
        string json = JsonConvert.SerializeObject(DataGridStates, Formatting.Indented);
        // save to disk (overwrite)
        try
        {
            File.WriteAllText(DGStatesPath, json);
        }
        catch
        {
            // IO error
            MessageBox.Show("There was an error writing to the config file: \n\n" + DGStatesPath + "\n\n Do you have the file open elsewhere??", "IO ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void LoadDataGridStatesFromDisk()
    {
        if (File.Exists(DGStatesPath))
        {
            // load the file
            string json = File.ReadAllText(DGStatesPath);
            DataGridStates = JsonConvert.DeserializeObject<Dictionary<int, List<ColumnInfo>>>(json);
        }
        else
        {
            // file does not exist - populate defaults
        }
    }

    protected void OnPropertyChanged(string propertyname)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    public void CountryFilterUpdate()
    {
        countryFilter = GamesLibraryVisualHandler.GetSelectedCountryFilter();

        List<DataGridGamesView> temp = new List<DataGridGamesView>();

        // update the collection based on country code filter
        switch (countryFilter)
        {
            case CountryFilter.EUR:                    
                temp = FilteredSetCache.Where(a => a.Country != null && a.Country.ToUpper().Contains("EU")).ToList();
                FilteredSet = new ObservableCollection<DataGridGamesView>(temp);
                break;
            case CountryFilter.JPN:
                temp = FilteredSetCache.Where(a => a.Country != null && a.Country.ToUpper().Contains("J")).ToList();
                FilteredSet = new ObservableCollection<DataGridGamesView>(temp);
                break;
            case CountryFilter.USA:
                temp = FilteredSetCache.Where(a => a.Country != null && a.Country.ToUpper().Contains("US")).ToList();
                FilteredSet = new ObservableCollection<DataGridGamesView>(temp);
                break;
            default:
                //FilteredSet = new ObservableCollection<DataGridGamesView>(FilteredSetCache.ToList());
                break;
        }            
    }

    // update game data for games datagrid including search
    public static void GetGames(DataGrid datagrid, int systemId, string search) //, CountryFilter countryFilter)
    {
        // get the full dataset from application
        App _App = ((App)Application.Current);
        //List<DataGridGamesView> allGames = _App.GamesList.AllGames;



        var result = GameListBuilder.Filter(systemId, search); //, countryFilter);
    }



    /// <summary>
    /// remove hidden systems from results set
    /// </summary>
    /// <param name="results"></param>
    /// <returns></returns>
    public static List<DataGridGamesView> RemoveHidden(List<DataGridGamesView> results)
    {
        bool[] visibilities = GlobalSettings.GetVisArray();

        // check whether there are actually any hidden systems
        bool systemsHidden = false;
        for (int i = 1; i <= visibilities.Length; i++)
        {
            if (i == 16 || i == 17)
                continue;

            if (visibilities[i - 1] == false)
            {
                systemsHidden = true;
                break;
            }
        }

        if (systemsHidden == false)
        {
            // there are no hidden systems - just return the results as-is
            return results;
        }                
        else
        {
            // there are hidden systems - remove them from results
            for (int i = 1; i <= visibilities.Length; i++)
            {
                if (i == 16 || i == 17) // skip fast and faust
                    continue;

                if (visibilities[i - 1] == false)
                {
                    results = (from a in results
                              where a.System != GSystem.GetSystemName(i)
                              select a).ToList();
                }
            }

            return results;
        }
    }

    public static List<DataGridGamesView> Filter(int systemId, string search) //, CountryFilter countryFilter)
    {
        List<DataGridGamesView> results = new List<DataGridGamesView>();
        App _App = ((App)Application.Current);

        MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        List<ColumnInfo> cis = new List<ColumnInfo>();
        DataGrid dgGameList = new DataGrid();
        List<SortDescription> sdc = new List<SortDescription>();

        ICollectionView cView = CollectionViewSource.GetDefaultView(dgGameList.ItemsSource);
        if (cView != null)
            sdc = cView.SortDescriptions.ToList();


        if (mw != null)
        {
            dgGameList = (DataGrid)mw.FindName("dgGameList");
            // get the current datagrid column layout (will re-apply it at the end of this method)

            cis = ColumnInfo.GetColumnInfo(dgGameList);                
        }

        if (_App.GamesList.UpdateRequired == true)
            GamesLibraryDataGridRefresh.Update();

        // system id selector
        switch (systemId)
        {
            case -1:        // favorites
                results = (from g in _App.GamesList.AllGames
                           where g.Favorite == true
                         select g).ToList();

                results = RemoveHidden(results);

                break;
            case 0:         // all games
                results = _App.GamesList.AllGames;
                results = RemoveHidden(results);


                break;
            case -100:      // unscraped games

                results = (from a in _App.GamesList.AllGames
                           where a.Coop == null &&
                           a.Developer == null &&
                           a.ESRB == null &&
                           a.Players == null
                           select a).ToList();

                results = RemoveHidden(results);




                break;
            default:        // based on actual system id
                results = (from g in _App.GamesList.AllGames
                           where GSystem.GetSystemId(g.System) == systemId
                         select g).ToList();
                break;
        }

        // narrow search based on country/region filter


        // now we have results based on the system filter - process file search
        List<DataGridGamesView> srch = DoSearch(results, search);



        // trigger update in datagrid with sorting      
        _App.GamesList.FilteredSet = new ObservableCollection<DataGridGamesView>(srch);

        // setup cache
        _App.GamesList.FilteredSetCache = new List<DataGridGamesView>();
        _App.GamesList.FilteredSetCache = srch.ToList();

        // countryfilter
        _App.GamesList.CountryFilterUpdate();

        // re-apply datagrid column states            
        if (mw != null)
        {
            // Mainwindow has been found

            // re-apply column settings
            ColumnInfo.ApplyColumnInfo(dgGameList, cis);

            // refresh viewsource

            if (cView != null)
            {
                //cView.SortDescriptions = sdc;
                cView.Refresh();
            }

        }


        return srch;
    }

    public static List<DataGridGamesView> DoSearch(List<DataGridGamesView> list, string sStr)
    {
        // check whether we need to search the gdb columns
        GlobalSettings gs = GlobalSettings.GetGlobals();
        if (sStr == null || sStr == "")
            return list;

        List<DataGridGamesView> search = new List<DataGridGamesView>();

        search = (from g in list.Where(a => a.Game != null)
                 where g.Game.ToLower().Contains(sStr.ToLower())
                 select g).ToList();

        return search;
    }

    /// <summary>
    /// Set the update flag so that on the next local operation the data is refreshed from the database
    /// </summary>
    public static void UpdateFlag()
    {
        App _App = ((App)Application.Current);
        _App.GamesList.UpdateRequired = true;
    }






}

public enum CountryFilter
{
    ALL,
    USA,
    EUR,
    JPN
}
*/
    }
}
