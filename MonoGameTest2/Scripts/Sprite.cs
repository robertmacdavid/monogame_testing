﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Design;

namespace MonoGameTest2
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Sprite(Texture2D texture, Vector2 initialPosition)
        {
            Texture = texture;
            Position = initialPosition;
            Width = texture.Width;
            Height = texture.Height;
        }

        // TODO: Don't pass deltaTime? Read from GameManager???
        public void Move(Vector2 velocity, float deltaTime)
        {
            
            Position += velocity * deltaTime;
        }

        // TODO: Don't pass spriteBatch. Handle it in GameManager.
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.End();
        }
    }
}
