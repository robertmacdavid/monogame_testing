using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Helpers;
using MonoGameTest2.Levels;
using System;

namespace MonoGameTest2.Managers
{
    public class LevelManager
    {
        public static readonly Point TileSize = new Point(16, 16);

        public Level Level;

        public int ActualWidth { get { return (int)(TileSize.X * Level.Width); } }
        public int ActualHeight { get { return (int)(TileSize.Y * Level.Height); } }
        public int[] TileCounts { get; private set; }

        private Texture2D[] _tileSpriteSheets;

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

            foreach (var tileTexture in _tileSpriteSheets)
            {
                GameManager.Instance.Console.AddLine(tileTexture.Name);
            }

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
