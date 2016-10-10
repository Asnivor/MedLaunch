using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;

namespace MedLaunch.Classes
{
    public static class GamesLibData
    {
        private static List<Game> _games = new List<Game>();

        public static List<Game> Games
        {
            get { return _games; }
            private set { _games = value; }
        }

        static GamesLibData()
        {
            _forceUpdate();
        }

        public static void ForceUpdate()
        {
            _forceUpdate();
        }

        private static void _forceUpdate()
        {
            using (var context = new MyDbContext())
            {
                _games = (from c in context.Game
                                   select c).ToList();
            }
        }
    }
}
