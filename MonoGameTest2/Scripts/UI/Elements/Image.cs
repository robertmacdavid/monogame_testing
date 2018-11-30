using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.UI;

namespace MonoGameTest2.UI
{
    public class Image : UIElement
    {
        public Image(UIDimension dimensions, Texture2D texture, Vector2? anchor = null) : this(null, dimensions, texture, anchor) { }

        public Image(UIElement parent, UIDimension dimensions, Texture2D texture, Vector2? anchor = null) : base(parent, dimensions, anchor)
        {
            Texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, AbsoluteDimensions, Color.White);
        }
    }
}
