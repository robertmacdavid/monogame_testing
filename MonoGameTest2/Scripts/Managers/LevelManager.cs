using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Helpers;
using System;

namespace MonoGameTest2.Managers
{
    public class LevelManager
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Vector2 TileSize { get; private set; }

        public int ActualWidth { get { return (int)TileSize.X * Width; } }
        public int ActualHeight { get { return (int)TileSize.Y * Height; } }

        private Texture2D[] _tileTextures;
        private int[,] _tiles;

        public void LoadContent(ContentManager contentManager)
        {
            _tileTextures = contentManager.LoadAll<Texture2D>("tiles");
        }

        public void BuildLevel()
        {
            TileSize = new Vector2(32, 32);
            Width = 30;
            Height = 20;
            _tiles = new int[Width, Height];
            
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (y == 0 || y == Height - 1 || x == 0 || x == Width - 1)
                    {
                        _tiles[x, y] = 1;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var tile = _tiles[x, y];
                    var tileTexture = _tileTextures[tile];
                    spriteBatch.Draw(tileTexture, new Vector2(x * 32, y * 32), Color.White);
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
