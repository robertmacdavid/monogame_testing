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
        public Level Level;

        public Vector2 TileSize { get; private set; }

        public int ActualWidth { get { return (int)(TileSize.X * Level.Width); } }
        public int ActualHeight { get { return (int)(TileSize.Y * Level.Height); } }

        private Texture2D[] _tileTextures;

        public void LoadContent(ContentManager contentManager)
        {
            _tileTextures = contentManager.LoadAll<Texture2D>("tiles");
        }

        public void BuildLevel()
        {
            uint width = 30;
            uint height = 20;
            TileSize = new Vector2(16, 16);
            Level = new Level(width, height);

            for (uint y = 0; y < height; y++)
            {
                for (uint x = 0; x < width; x++)
                {
                    if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                    {
                        Level.SetTile(x, y, new Level.Tile(Level.TileTypes.Wall));
                    }
                    else
                    {
                        Level.SetTile(x, y, new Level.Tile(Level.TileTypes.Floor));
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
                    var tile = Level.GetTile(x, y);
                    
                    if (tile.HasValue)
                    {
                        var tileTexture = _tileTextures[(int)tile.Value.TileType];
                        spriteBatch.Draw(tileTexture, new Vector2(x * TileSize.X, y * TileSize.Y), Color.White);
                    }
                }
            }
        }

        public Vector2 WorldPositionToTilePosition(Vector2 worldPosition)
        {
            return new Vector2
            {
                X = (float)Math.Floor(worldPosition.X / TileSize.X) * TileSize.X,
                Y = (float)Math.Floor(worldPosition.Y / TileSize.Y) * TileSize.Y
            };
        }
    }
}
