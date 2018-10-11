using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    public class UIPanel : UIElement
    {
        public Color Color;

        public UIPanel(UIElement parent, UIRectangle dimensions, Color? color = null) : base(parent, dimensions)
        {
            Color = color ?? new Color(0.2f, 0.2f, 0.2f, 0.7f);
            Texture = UIManager.PanelBackground;
        }

        public UIPanel(UIRectangle dimensions) : this(null, dimensions) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, AbsoluteBounds.RealDimensions, Color);
        }
    }
}
