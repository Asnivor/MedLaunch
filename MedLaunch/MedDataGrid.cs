using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace MedLaunch
{
    public class MedDataGrid : DataGrid
    {
        public MedDataGrid()
        {
            _App = (App)System.Windows.Application.Current;
            SortDescriptions = new List<SortDescription>();
            Sorting += DataGridSorting;
        }

        private MedLaunch.App _App { get; set; }

        protected List<SortDescription> SortDescriptions { get; private set; }

        public event EventHandler<SortConstructedEventArgs> SortConstructed;

        public void OnSortConstructed(SortConstructedEventArgs e)
        {
            var handler = SortConstructed;
            if (handler != null) handler(this, e);
        }

        private void DataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            e.Column.SortDirection = e.Column.SortDirection != ListSortDirection.Ascending
                                         ? ListSortDirection.Ascending
                                         : ListSortDirection.Descending;

            var sd = new SortDescription(e.Column.SortMemberPath, e.Column.SortDirection.Value);

            if (ShiftPressed)
                SortDescriptions =
                    SortDescriptions.Where(x => x.PropertyName != sd.PropertyName).ToList();
            else
            {
                SortDescriptions.Clear();
                _App.GamesLibrary.LibraryView.SortDescriptions.Clear();
            }
                

            using (_App.GamesLibrary.LibraryView.DeferRefresh())
            {
                SortDescriptions.Add(sd);

            
                foreach (var s in SortDescriptions)
                {
                    _App.GamesLibrary.LibraryView.SortDescriptions.Add(s);
                }
            }

            _App.GamesLibrary.LibraryView.View.Refresh();

            OnSortConstructed(new SortConstructedEventArgs(SortDescriptions));

            
               
            //_App.GamesLibrary.LibraryView.View.Refresh();
            
        }

        private bool ShiftPressed
        {
            get { return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift; }
        }
    }

    public class SortConstructedEventArgs : EventArgs
    {
        public List<SortDescription> SortDescriptions { get; private set; }

        public SortConstructedEventArgs(List<SortDescription> sortStack)
        {
            SortDescriptions = sortStack;
        }

        public IOrderedQueryable<T> Order<T>(IQueryable<T> queryable)
        {
            if (SortDescriptions == null || SortDescriptions.Count == 0)
                return queryable.OrderBy(x => 0);

            IOrderedQueryable<T> result;


            var first = SortDescriptions.First();
            switch (first.Direction)
            {
                case ListSortDirection.Ascending:
                    result = queryable.OrderBy(first.PropertyName); // uses orderby extension methods!
                    break;

                case ListSortDirection.Descending:
                    result = queryable.OrderByDescending(first.PropertyName);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var sort in SortDescriptions.Skip(1))
            {
                switch (sort.Direction)
                {
                    case ListSortDirection.Ascending:
                        result = result.ThenBy(sort.PropertyName);
                        break;
                    case ListSortDirection.Descending:
                        result = result.ThenByDescending(sort.PropertyName);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }
    }

    public class SortDescriptor
    {
        public string Property { get; set; }
        public bool Descending { get; set; }

        public SortDescriptor()
        {
            // for serializers
        }

        public SortDescriptor(string property, bool descending = false)
        {
            Property = property;
            Descending = descending;
        }

        // parses strings like "Name DESC" or "Whatever ASC"
        // ascending is the default
        public static SortDescriptor Parse(string s)
        {
            var parts = s.Split().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var property = parts.FirstOrDefault();
            var desc = parts.Skip(1).Take(1).Any(p => p.StartsWith("d", StringComparison.OrdinalIgnoreCase));
            return new SortDescriptor(property, desc);
        }
    }

    public static class SortingExtensions
    {

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, params string[] sortExpressions)
        {
            return source.OrderBy(sortExpressions.Select(SortDescriptor.Parse));
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<SortDescriptor> sorts)
        {
            if (sorts == null) throw new ArgumentNullException("sorts");

            IOrderedQueryable<T> sorted = null;
            var i = 0;
            foreach (var sort in sorts)
            {
                sorted = i == 0
                    ? source.OrderBy(sort.Property, sort.Descending)
                    : sorted.ThenBy(sort.Property, sort.Descending);

                i++;
            }

            return sorted ?? source.OrderBy(x => 0);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property, bool desc)
        {
            return desc ? source.OrderByDescending(property) : source.OrderBy(property);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property, bool desc)
        {
            return desc ? source.ThenByDescending(property) : source.ThenBy(property);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split('.');
            var type = typeof(T);
            var arg = System.Linq.Expressions.Expression.Parameter(type, "x");
            System.Linq.Expressions.Expression expr = arg;
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop);
                expr = System.Linq.Expressions.Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = System.Linq.Expressions.Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                          && method.IsGenericMethodDefinition
                          && method.GetGenericArguments().Length == 2
                          && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result;
        }
    }
    
}
