using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGameTest2
{
    public class AnimatedSprite : Sprite
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        private int _width;
        private int _height;
        private int _currentFrame;
        private readonly int _totalFrames;

        public AnimatedSprite(Texture2D texture, Vector2 initialPosition, int rows, int columns, int frameCount) : base(texture, initialPosition)
        {
            Rows = rows;
            Columns = columns;

            _width = texture.Width / columns;
            _height = texture.Height / rows;
            _currentFrame = 0;
            _totalFrames = frameCount;
        }

        public AnimatedSprite(Texture2D texture, Vector2 initialPosition, int rows, int columns) : this(texture, initialPosition, rows, columns, rows*columns) { }

        public void Update()
        {
            _currentFrame++;
            if (_currentFrame >= _totalFrames)
                _currentFrame = 0;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            int row = (int)((float)_currentFrame / (float)Columns);
            int column = _currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(_width * column, _height * row, _width, _height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, _width, _height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}