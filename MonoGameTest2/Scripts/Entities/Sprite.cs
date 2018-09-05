using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Design;

using MonoGameTest2.Managers;

namespace MonoGameTest2.Entities
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Rectangle Mask { get; set; }

        public Sprite(Texture2D texture, Vector2 initialPosition, Rectangle? Mask = null)
        {
            Texture = texture;
            Position = initialPosition;
            Width = texture.Width;
            Height = texture.Height;
            if (Mask == null)
                this.Mask = new Rectangle(0, 0, Width, Height);
            else
                this.Mask = (Rectangle)Mask;
        }

        public void Move(Vector2 velocity)
        {
            Position += velocity * GameManager.Instance.DeltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position - new Vector2(Width/2, Height/2), Color.White);
        }
    }
}
