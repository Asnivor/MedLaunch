using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGamesDBAPI;

namespace ConsoleApplication1 {
    class Program {
        // User's account identifier. Required for user methods (like getting a user's favorites). Can be found at http://thegamesdb.net/userinfo/.
        static String AccountIdentifier;

        static void Main(string[] args) {
            // Get account identifier
            Console.WriteLine("Enter user's account identifier (leave empty to not show user methods):");
            AccountIdentifier = Console.In.ReadLine();

            // Test getting platform
            Console.WriteLine("=Getting platform=");
            Platform Xbox360 = GamesDB.GetPlatform(15);
            Console.WriteLine("Overview: " + Xbox360.Overview);
            Console.WriteLine("Developer: " + Xbox360.Developer);
            Console.WriteLine("Manufacturer: " + Xbox360.Manufacturer);
            Console.WriteLine("CPU: " + Xbox360.CPU);
            Console.WriteLine("Memory: " + Xbox360.Memory);
            Console.WriteLine("Graphics: " + Xbox360.Graphics);
            Console.WriteLine("Sound: " + Xbox360.Sound);
            Console.WriteLine("Display: " + Xbox360.Display);
            Console.WriteLine("Media: " + Xbox360.Media);
            Console.WriteLine("Max controllers: " + Xbox360.MaxControllers);
            Console.WriteLine("Rating: " + Xbox360.Rating);

            // Art
            Console.WriteLine("Console art: " + Xbox360.Images.ConsoleArt);
            Console.WriteLine("Controller art: " + Xbox360.Images.ControllerArt);
            Console.WriteLine("Boxart:");
            if (Xbox360.Images.Boxart != null) {
                Console.WriteLine(Xbox360.Images.Boxart.Path + " : " + Xbox360.Images.Boxart.Width + "x" + Xbox360.Images.Boxart.Height);
            }
            Console.WriteLine("Fanart:");
            foreach (Platform.PlatformImages.PlatformImage image in Xbox360.Images.Fanart) {
                Console.WriteLine(image.Path + " : " + image.Width + "x" + image.Height);
            }
            Console.WriteLine("Banners:");
            foreach (Platform.PlatformImages.PlatformImage image in Xbox360.Images.Banners) {
                Console.WriteLine(image.Path + " : " + image.Width + "x" + image.Height);
            }

            // Test getting all games for Xbox360
            Console.WriteLine("=Getting all Xbox360 games=");
            ICollection<GameSearchResult> games = GamesDB.GetPlatformGames(Xbox360);
            foreach (GameSearchResult game in games) {
                Console.WriteLine(game.Title + " : " + game.ReleaseDate + " : " + game.Platform);
            }

            // Test getting platforms
            Console.WriteLine("=Getting list of platforms=");
            foreach (PlatformSearchResult platform in GamesDB.GetPlatforms()) {
                Console.WriteLine(platform.Name + " : " + platform.Alias);
            }

            // Test getting game
            Console.WriteLine("=Getting game=");
            Game Mario = GamesDB.GetGame(170);
            Console.WriteLine("Overview: " + Mario.Overview);
            Console.WriteLine("Title: " + Mario.Title);
            Console.WriteLine("Platform: " + Mario.Platform);
            Console.WriteLine("Release date: " + Mario.ReleaseDate);
            Console.WriteLine("ESRB: " + Mario.ESRB);
            Console.WriteLine("Players: " + Mario.Platform);
            Console.WriteLine("Publisher: " + Mario.Publisher);
            Console.WriteLine("Developer: " + Mario.Developer);
            Console.WriteLine("Rating: " + Mario.Rating);
            Console.WriteLine("Alternate titles:");
            foreach (String title in Mario.AlternateTitles) {
                Console.WriteLine(title);
            }
            Console.WriteLine("Genres:");
            foreach (String genre in Mario.Genres) {
                Console.WriteLine(genre);
            }

            // Art
            Console.WriteLine("Boxart - front:");
            if (Mario.Images.BoxartFront != null) {
                Console.WriteLine(Mario.Images.BoxartFront.Path + " : " + Mario.Images.BoxartFront.Width + "x" + Mario.Images.BoxartFront.Height);
            }
            Console.WriteLine("Boxart - back:");
            if (Mario.Images.BoxartBack != null) {
                Console.WriteLine(Mario.Images.BoxartBack.Path + " : " + Mario.Images.BoxartBack.Width + "x" + Mario.Images.BoxartBack.Height);
            }
            Console.WriteLine("Fanart:");
            foreach (Game.GameImages.GameImage image in Mario.Images.Fanart) {
                Console.WriteLine(image.Path + " : " + image.Width + "x" + image.Height);
            }
            Console.WriteLine("Banners:");
            foreach (Game.GameImages.GameImage image in Mario.Images.Banners) {
                Console.WriteLine(image.Path + " : " + image.Width + "x" + image.Height);
            }
            Console.WriteLine("Screenshots:");
            foreach (Game.GameImages.GameImage image in Mario.Images.Screenshots) {
                Console.WriteLine(image.Path + " : " + image.Width + "x" + image.Height);
            }

            // Test getting games
            Console.WriteLine("=Searching games for 'crysis'=");
            foreach (GameSearchResult game in GamesDB.GetGames("crysis")) {
                Console.WriteLine(game.Title + " : " + game.ReleaseDate + " : " + game.Platform);
            }

            // Test getting updated games
            Console.WriteLine("=Getting list of updated games in the last 10000 seconds=");
            foreach (int game in GamesDB.GetUpdatedGames(10000)) {
                Console.WriteLine(game);
            }

            // Test getting a user's favorites
            if (AccountIdentifier != "")
            {
                Console.WriteLine("=Getting user favorites=");
                bool first = true;
                foreach (int favorite in GamesDB.GetUserFavorites(AccountIdentifier))
                {
                    if (!first)
                    {
                        Console.Write(", ");
                    }
                    Console.Write(favorite);
                    first = false;
                }
                Console.WriteLine();

                // Test favorites
                Console.WriteLine("=Add Crysis to favorites=");
                GamesDB.AddUserFavorite(AccountIdentifier, 2);
                Console.WriteLine("=Add StarFox to favorites=");
                GamesDB.AddUserFavorite(AccountIdentifier, 4);

                Console.WriteLine("=Remove Crysis from favorites=");
                GamesDB.RemoveUserFavorite(AccountIdentifier, 2);

                // Test getting user rating
                Console.WriteLine("=Getting rating for Crysis=");
                Console.WriteLine(GamesDB.GetUserRating(AccountIdentifier, 2));

                // Test user ratings
                Console.WriteLine("=Rate Crysis a 4=");
                GamesDB.SetUserRating(AccountIdentifier, 2, 4);
                Console.WriteLine("=Rate StarFox a 9=");
                GamesDB.SetUserRating(AccountIdentifier, 4, 9);

                Console.WriteLine("=Remove Crysis rating=");
                GamesDB.RemoveUserRating(AccountIdentifier, 2);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
