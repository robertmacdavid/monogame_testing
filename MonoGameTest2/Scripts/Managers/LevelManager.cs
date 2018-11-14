using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.Helpers;
using MonoGameTest2.Levels;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MonoGameTest2.Managers
{
    public class LevelManager
    {
        public static readonly Point TileSize = new Point(16, 16);
        public static readonly string RelativeLevelFilesPath = "levels" + Path.DirectorySeparatorChar;

        public Level Level;

        public int ActualWidth { get { return (int)(TileSize.X * Level.Width); } }
        public int ActualHeight { get { return (int)(TileSize.Y * Level.Height); } }
        public int[] TileCounts { get; private set; }

        private Texture2D[] _tileSpriteSheets;
        private XmlSerializer _serializer;

        public void LoadContent(ContentManager contentManager)
        {
            _tileSpriteSheets = contentManager.LoadAll<Texture2D>("tiles");
            TileCounts = new int[(int)TileTypes.TileTypeCount];

            for (var i = 0; i < _tileSpriteSheets.Length; i++)
            {
                var tileTexture = _tileSpriteSheets[i];
                var columns = tileTexture.Width / TileSize.X;
                var rows = tileTexture.Height / TileSize.Y;

                TileCounts[i] = columns * rows;
            }

            _serializer = new XmlSerializer(typeof(Level));
        }

        public void BuildLevel()
        {
            uint width = 30;
            uint height = 20;
            Level = new Level(width, height);

            for (uint y = 0; y < height; y++)
            {
                for (uint x = 0; x < width; x++)
                {
                    if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                    {
                        Level.SetTile(x, y, new Tile(x, y, TileTypes.Wall));
                    }
                    else
                    {
                        Level.SetTile(x, y, new Tile(x, y, TileTypes.Floor));
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (uint y = 0; y < Level.Height; y++)
            {
                for (uint x = 0; x < Level.Width; x++)
                {
                    var tileOrNull = Level.GetTile(x, y);
                    
                    if (tileOrNull.HasValue)
                    {
                        var tile = tileOrNull.Value;
                        var tileTexture = _tileSpriteSheets[(int)tile.TileType];

                        var columns = tileTexture.Width / TileSize.X;
                        var column = (int)tile.TextureIndex % columns;
                        var row = (int)tile.TextureIndex / columns;

                        var sourceRectangle = new Rectangle(TileSize.X * column, TileSize.Y * row, TileSize.X, TileSize.Y);
                        spriteBatch.Draw(tileTexture, new Vector2(x * TileSize.X, y * TileSize.Y), sourceRectangle, Color.White);
                    }
                }
            }
        }

        public void SaveLevel(string fileName)
        {
            if (fileName == null || fileName.Length == 0 || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                return;
            }

            using (var writer = XmlWriter.Create(Path.Combine("levels", fileName + ".xml"), new XmlWriterSettings() { Indent = true }))
            {
                _serializer.Serialize(writer, Level);
            }
        }

        public void LoadLevel(string path)
        {
            if (path == null || path.Length == 0 || path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                return;
            }

            var serializer = new XmlSerializer(typeof(Level));
            using (var reader = File.OpenText(path))
            {
                Level = (Level)_serializer.Deserialize(reader);
            }
        }

        public static string[] GetLevelFiles()
        {
            return Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), RelativeLevelFilesPath), "*.xml");
        }

        public static Vector2 WorldPositionToTile(Vector2 worldPosition)
        {
            return new Vector2
            {
                X = (float)Math.Floor(worldPosition.X / TileSize.X),
                Y = (float)Math.Floor(worldPosition.Y / TileSize.Y)
            };
        }

        // TODO: Refactor this to return a point.
        public static Vector2 WorldPositionToTile(Point worldPosition)
        {
            return WorldPositionToTile(worldPosition.ToVector2());
        }

        public static Vector2 WorldPositionToTilePosition(Vector2 worldPosition)
        {
            return WorldPositionToTile(worldPosition) * TileSize.ToVector2();
        }

        public static Vector2 WorldPositionToTilePosition(Point worldPosition)
        {
            return WorldPositionToTile(worldPosition) * TileSize.ToVector2();
        }
    }
}
