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

        public MultipleFilterHandler(CollectionViewSource collection, MultipleFilterLogic operation)
        {
            this.collection = collection;
            this.Operation = operation;
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
    }

    public enum MultipleFilterLogic
    {
        And,
        Or
    }
}
