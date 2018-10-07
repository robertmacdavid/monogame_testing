using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    public class Panel : UIElement
    {
        public Panel(UIRectangle dimensions) : base(dimensions) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UIManager.PanelBackground, AbsoluteBounds.RealDimensions, Color.White);
        }
    }
}
