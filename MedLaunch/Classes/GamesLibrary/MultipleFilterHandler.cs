using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MedLaunch.Classes.GamesLibrary
{
    public class MultipleFilterHandler
    {
        private readonly CollectionViewSource collection;

        public MultipleFilterLogic Operation { get; set; }

        public Filters filters { get; set; }

        public MultipleFilterHandler(CollectionViewSource collection, MultipleFilterLogic operation)
        {
            this.collection = collection;
            this.Operation = operation;
            this.filters = new Filters();
        }

        public MultipleFilterHandler(CollectionViewSource collection) :
            this(collection, MultipleFilterLogic.Or)
        {
        }

        private event FilterEventHandler _filter;
        public event FilterEventHandler Filter
        {
            add
            {
                _filter += value;

                collection.Filter -= new FilterEventHandler(CollectionViewFilter);
                collection.Filter += new FilterEventHandler(CollectionViewFilter);
            }
            remove
            {
                _filter -= value;

                collection.Filter -= new FilterEventHandler(CollectionViewFilter);
                collection.Filter += new FilterEventHandler(CollectionViewFilter);
            }
        }

        private void CollectionViewFilter(object sender, FilterEventArgs e)
        {
            if (_filter == null)
                return;

            foreach (FilterEventHandler invocation in _filter.GetInvocationList())
            {
                invocation(sender, e);

                if ((Operation == MultipleFilterLogic.And && !e.Accepted) || (Operation == MultipleFilterLogic.Or && e.Accepted))
                    return;
            }
        }

        public void SetMainFilter(int FilterNumber)
        {
            // remove all system filters first
            this.Filter -= new FilterEventHandler(filters.ShowFavorites);
            this.Filter -= new FilterEventHandler(filters.ShowUnscraped); 
            this.Filter -= new FilterEventHandler(filters.ShowNes); 
            this.Filter -= new FilterEventHandler(filters.ShowSnes); 
            this.Filter -= new FilterEventHandler(filters.ShowSms); 
            this.Filter -= new FilterEventHandler(filters.ShowMd); 
            this.Filter -= new FilterEventHandler(filters.ShowPce); 
            this.Filter -= new FilterEventHandler(filters.ShowVb); 
            this.Filter -= new FilterEventHandler(filters.ShowNgp); 
            this.Filter -= new FilterEventHandler(filters.ShowWswan); 
            this.Filter -= new FilterEventHandler(filters.ShowGb); 
            this.Filter -= new FilterEventHandler(filters.ShowGba); 
            this.Filter -= new FilterEventHandler(filters.ShowGg); 
            this.Filter -= new FilterEventHandler(filters.ShowLynx); 
            this.Filter -= new FilterEventHandler(filters.ShowSs); 
            this.Filter -= new FilterEventHandler(filters.ShowPsx); 
            this.Filter -= new FilterEventHandler(filters.ShowPcecd); 
            this.Filter -= new FilterEventHandler(filters.ShowPcfx); 

            // now add the (AND) the system filter we are interested in
            switch (FilterNumber)
            {
                case 1:
                    // no system filter
                    break;
                case 2: this.Filter += new FilterEventHandler(filters.ShowFavorites); break;
                case 3: this.Filter += new FilterEventHandler(filters.ShowUnscraped); break;
                case 4: this.Filter += new FilterEventHandler(filters.ShowNes); break;
                case 5: this.Filter += new FilterEventHandler(filters.ShowSnes); break;
                case 6: this.Filter += new FilterEventHandler(filters.ShowSms); break;
                case 7: this.Filter += new FilterEventHandler(filters.ShowMd); break;
                case 8: this.Filter += new FilterEventHandler(filters.ShowPce); break;
                case 9: this.Filter += new FilterEventHandler(filters.ShowVb); break;
                case 10: this.Filter += new FilterEventHandler(filters.ShowNgp); break;
                case 11: this.Filter += new FilterEventHandler(filters.ShowWswan); break;
                case 12: this.Filter += new FilterEventHandler(filters.ShowGb); break;
                case 13: this.Filter += new FilterEventHandler(filters.ShowGba); break;
                case 14: this.Filter += new FilterEventHandler(filters.ShowGg); break;
                case 15: this.Filter += new FilterEventHandler(filters.ShowLynx); break;
                case 16: this.Filter += new FilterEventHandler(filters.ShowSs); break;
                case 17: this.Filter += new FilterEventHandler(filters.ShowPsx); break;
                case 18: this.Filter += new FilterEventHandler(filters.ShowPcecd); break;
                case 19: this.Filter += new FilterEventHandler(filters.ShowPcfx); break;
            }
        }
    }

    public enum MultipleFilterLogic
    {
        And,
        Or
    }
}
