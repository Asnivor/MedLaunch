using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace TheGamesDBAPI {
    /// <summary>
    /// Contains the data for one platform in the database.
    /// </summary>
    public class Platform {
        /// <summary>
        /// Unique database ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// The name of the platform.
        /// </summary>
        public String Name;

        /// <summary>
        /// The max amount of controllers that can be connected to the device.
        /// </summary>
        public int MaxControllers;

        /// <summary>
        /// General overview of the platform.
        /// </summary>
        public String Overview;
        
        /// <summary>
        /// The developer(s) of the platform.
        /// </summary>
        public String Developer;

        /// <summary>
        /// The manufacturer(s) of the platform.
        /// </summary>
        public String Manufacturer;

        /// <summary>
        /// The CPU of the platform (for platforms which have one specific CPU).
        /// </summary>
        public String CPU;

        /// <summary>
        /// Information about the platform's memory.
        /// </summary>
        public String Memory;

        /// <summary>
        /// The platform's graphics card.
        /// </summary>
        public String Graphics;

        /// <summary>
        /// Information about the platform's sound capabilities.
        /// </summary>
        public String Sound;

        /// <summary>
        /// Display resolution (on the form: 'width'x'height')
        /// </summary>
        public String Display;

        /// <summary>
        /// The game media the platform reads (eg. 'Disc').
        /// </summary>
        public String Media;

        /// <summary>
        /// The average rating as rated by the users on TheGamesDB.net.
        /// </summary>
        public float Rating;

        /// <summary>
        /// A PlatformImages-object containing all the images for the platform.
        /// </summary>
        public PlatformImages Images;

        /// <summary>
        /// Creates a new Platform without any content.
        /// </summary>
        public Platform() {
            Images = new PlatformImages();
        }

        /// <summary>
        /// Represents the images for one platform in the database.
        /// </summary>
        public class PlatformImages {
            /// <summary>
            /// Path to the image of the console.
            /// </summary>
            public String ConsoleArt;

            /// <summary>
            /// Path to the image of the controller.
            /// </summary>
            public String ControllerArt;

            /// <summary>
            /// Boxart for the platform
            /// </summary>
            public PlatformImage Boxart;

            /// <summary>
            /// A list of all the fanart for the platform that have been uploaded to the database.
            /// </summary>
            public List<PlatformImage> Fanart;

            /// <summary>
            /// A list of all the banners for the platform that have been uploaded to the database.
            /// </summary>
            public List<PlatformImage> Banners;

            /// <summary>
            /// Creates a new PlatformImages without any content.
            /// </summary>
            public PlatformImages() {
                Fanart = new List<PlatformImage>();
                Banners = new List<PlatformImage>();
            }

            /// <summary>
            /// Adds all the images that can be found in the XmlNode
            /// </summary>
            /// <param name="node">the XmlNode to search through</param>
            public void FromXmlNode(XmlNode node) {
                IEnumerator ienumImages = node.GetEnumerator();
                while (ienumImages.MoveNext()) {
                    XmlNode imageNode = (XmlNode)ienumImages.Current;

                    switch (imageNode.Name) {
                        case "fanart":
                            Fanart.Add(new PlatformImage(imageNode.FirstChild));
                            break;
                        case "banner":
                            Banners.Add(new PlatformImage(imageNode));
                            break;
                        case "boxart":
                            Boxart = new PlatformImage(imageNode);
                            break;
                        case "consoleart":
                            ConsoleArt = imageNode.InnerText;
                            break;
                        case "controllerart":
                            ControllerArt = imageNode.InnerText;
                            break;
                    }
                }
            }

            /// <summary>
            /// Represents one image
            /// </summary>
            public class PlatformImage {
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
                public PlatformImage(XmlNode node) {
                    Path = node.InnerText;

                    int.TryParse(node.Attributes.GetNamedItem("width").InnerText, out Width);
                    int.TryParse(node.Attributes.GetNamedItem("height").InnerText, out Height);
                }
            }
        }
    }
}
