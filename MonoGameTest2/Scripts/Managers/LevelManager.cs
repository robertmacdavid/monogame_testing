using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Helpers;

namespace MonoGameTest2.Managers
{
    class LevelManager
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private Texture2D[] _tileTextures;
        private int[,] _tiles;

        public void LoadContent(ContentManager contentManager)
        {
            _tileTextures = contentManager.LoadAll<Texture2D>("tiles");
        }

        public void BuildLevel()
        {
            Width = 25;
            Height = 15;
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
    }
}
