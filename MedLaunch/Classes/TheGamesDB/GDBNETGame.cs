using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections;

namespace MedLaunch.Classes.TheGamesDB
{
    /// <summary>
    /// Contains the data for one game in the database.
    /// </summary>
    public class GDBNETGame
    {
        /// <summary>
        /// Unique database ID
        /// </summary>
        public int ID;

        /// <summary>
        /// Title of the game.
        /// </summary>
        public String Title;

        /// <summary>
        /// Which platform the game is for.
        /// </summary>
        public String Platform;

        /// <summary>
        /// Which date the game was first released on.
        /// </summary>
        public String ReleaseDate;

        /// <summary>
        /// A general description of the game.
        /// </summary>
        public String Overview;

        /// <summary>
        /// ESRB rating for the game.
        /// </summary>
        public String ESRB;

        /// <summary>
        /// How many players the game supports. "1","2","3" or "4+".
        /// </summary>
        public String Players;

        /// <summary>
        /// Whether the game is Co-op or not
        /// </summary>
        public String Coop;

        /// <summary>
        /// The publisher(s) of the game.
        /// </summary>
        public String Publisher;

        /// <summary>
        /// The developer(s) of the game.
        /// </summary>
        public String Developer;

        /// <summary>
        /// The overall rating of the game as rated by users on TheGamesDB.net.
        /// </summary>
        public String Rating;

        /// <summary>
        /// A list of all the alternative titles of the game.
        /// </summary>
        public List<String> AlternateTitles;

        /// <summary>
        /// A list of all the game's genres.
        /// </summary>
        public List<String> Genres;

        /// <summary>
        /// A GameImages-object containing all the images for the game.
        /// </summary>
        public GameImages Images;

        /// <summary>
        /// Creates a new Game without any content.
        /// </summary>
        public GDBNETGame()
        {
            AlternateTitles = new List<String>();
            Genres = new List<String>();
            Images = new GameImages();
        }

        /// <summary>
        /// Represents the images for one game in the database.
        /// </summary>
        public class GameImages
        {
            /// <summary>
            /// The art on the back of the box.
            /// </summary>
            public GameImage BoxartBack;

            /// <summary>
            /// The art on the front of the box.
            /// </summary>
            public GameImage BoxartFront;

            /// <summary>
            /// A list of all the fanart for the game that have been uploaded to the database.
            /// </summary>
            public List<GameImage> Fanart;

            /// <summary>
            /// A list of all the banners for the game that have been uploaded to the database.
            /// </summary>
            public List<GameImage> Banners;

            /// <summary>
            /// A list of all the screenshots for the game that have been uploaded to the database.
            /// </summary>
            public List<GameImage> Screenshots;

            /// <summary>
            /// Creates a new GameImages without any content.
            /// </summary>
            public GameImages()
            {
                Fanart = new List<GameImage>();
                Banners = new List<GameImage>();
                Screenshots = new List<GameImage>();
            }

            /// <summary>
            /// Adds all the images that can be found in the XmlNode
            /// </summary>
            /// <param name="node">the XmlNode to search through</param>
            public void FromXmlNode(XmlNode node)
            {
                IEnumerator ienumImages = node.GetEnumerator();
                while (ienumImages.MoveNext())
                {
                    XmlNode imageNode = (XmlNode)ienumImages.Current;

                    switch (imageNode.Name)
                    {
                        case "fanart":
                            Fanart.Add(new GameImage(imageNode.FirstChild));
                            break;
                        case "banner":
                            Banners.Add(new GameImage(imageNode));
                            break;
                        case "screenshot":
                            Screenshots.Add(new GameImage(imageNode.FirstChild));
                            break;
                        case "boxart":
                            if (imageNode.Attributes.GetNamedItem("side").InnerText == "front")
                            {
                                BoxartFront = new GameImage(imageNode);
                            }
                            else
                            {
                                BoxartBack = new GameImage(imageNode);
                            }

                            break;
                    }
                }
            }

            /// <summary>
            /// Represents one image
            /// </summary>
            public class GameImage
            {
                /// <summary>
                /// The width of the image in pixels.
                /// </summary>
                public int Width;

                /// <summary>
                /// The height of the image in pixels.
                /// </summary>
                public int Height;

                /// <summary>
                /// The relative path to the image.
                /// </summary>
                /// <seealso cref="GamesDB.BaseImgURL"/>
                public String Path;

                /// <summary>
                /// Creates an image from an XmlNode.
                /// </summary>
                /// <param name="node">XmlNode to get data from</param>
                public GameImage(XmlNode node)
                {
                    Path = node.InnerText;

                    int.TryParse(node.Attributes.GetNamedItem("width").InnerText, out Width);
                    int.TryParse(node.Attributes.GetNamedItem("height").InnerText, out Height);
                }
            }
        }
    }
}
