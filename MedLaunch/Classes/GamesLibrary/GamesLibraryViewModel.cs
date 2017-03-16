using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GamesLibraryViewModel : GamesLibraryViewModelBase
    {
        ObservableCollection<GamesLibraryModel> _DataCollection;
        CollectionViewSource _ViewSource;
        ICommand _command;

        public bool IsDirty;

        public GamesLibraryViewModel()
        {
            _DataCollection = new ObservableCollection<GamesLibraryModel>();
            _ViewSource = new CollectionViewSource();
            _ViewSource.Source = _DataCollection;

            IsDirty = true;
            Update();
        }        

        public ObservableCollection<GamesLibraryModel> DataCollection
        {
            get
            {
                return _DataCollection;
            }
            set
            {
                _DataCollection = value;
                OnPropertyChanged("DataCollection");
            }
        }

        public CollectionViewSource ViewSource
        {
            get
            {
                return _ViewSource;
            }
            set
            {
                _ViewSource = value;
                OnPropertyChanged("ViewSource");
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                if (_command == null)
                {
                    _command = new DelegateCommand(CanExecute, Execute);
                }
                return _command;
            }
        }

        private void Execute(object parameter)
        {
            int index = DataCollection.IndexOf(parameter as GamesLibraryModel);
            if (index > -1 && index < DataCollection.Count)
            {
                DataCollection.RemoveAt(index);
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        //  filters
        private bool SystemFilter(object system)
        {
            GamesLibraryModel model = system as GamesLibraryModel;
        }

        public void Update()
        {
            if (!IsDirty)
                return;

            DataCollection = new ObservableCollection<GamesLibraryModel>();

            using (var cnt = new MyDbContext())
            {
                List<LibraryDataGDBLink> links = LibraryDataGDBLink.GetLibraryData().ToList();

                var games = (from g in cnt.Game
                             where g.hidden != true
                             select g).ToList();
                foreach (var game in games)
                {
                    GamesLibraryModel d = new GamesLibraryModel();
                    d.ID = game.gameId;

                    d.System = GSystem.GetSystemName(game.systemId);
                    d.LastPlayed = DbEF.FormatDate(game.gameLastPlayed);
                    d.Favorite = game.isFavorite;

                    d.Country = game.Country;

                    if (game.romNameFromDAT != null)
                    {
                        if (game.romNameFromDAT.Contains("(USA)"))
                            d.Country = "US";
                        if (game.romNameFromDAT.Contains("(Europe)"))
                            d.Country = "EU";
                    }


                    d.Flags = game.OtherFlags;
                    d.Language = game.Language;
                    d.Publisher = game.Publisher;
                    d.Developer = game.Developer;
                    d.Year = game.Year;

                    if (game.gameNameFromDAT != null && game.gameNameFromDAT != "")
                        d.Game = game.gameNameFromDAT;
                    else
                        d.Game = game.gameName;

                    //d.DatName = game.gameNameFromDAT;
                    d.DatRom = game.romNameFromDAT;

                    if (game.gdbId != null && game.gdbId > 0)
                    {
                        var link = links.Where(x => x.GDBId == game.gdbId).SingleOrDefault(); // LibraryDataGDBLink.GetLibraryData(game.gdbId.Value);
                        if (link != null)
                        {
                            if (link.Publisher != null && link.Publisher != "")
                                d.Publisher = link.Publisher;

                            d.Developer = link.Developer;

                            if (link.Year != null && link.Year != "")
                                d.Year = DbEF.ReturnYear(link.Year);
                            d.Players = link.Players;
                            d.Coop = link.Coop;
                            d.ESRB = link.ESRB;
                        }
                    }

                    DataCollection.Add(d);
                }

                IsDirty = false;
            }
        }




        /*
        public static CollectionViewSource ViewSource { get; set; }
        public static ObservableCollection<DataGridGamesView> Collection { get; set; }

        public GamesLibraryViewModel()
        {
            Collection = new ObservableCollection<DataGridGamesView>();
            ViewSource = new CollectionViewSource();
            ViewSource.Source = Collection;

            Collection = GamesLibraryDataGridRefresh.Update(Collection);
        }
        */
    }
}
