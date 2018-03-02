
namespace MedLaunch.Classes
{
    /// <summary>
    /// Game item class used when scraping
    /// </summary>
    public class GameListItem
    {
        public int GamesDBId { get; set; }
        public string GameName { get; set; }
        public int Matches { get; set; }
        public int Percentage { get; set; }
        public string Platform { get; set; }
    }
}
