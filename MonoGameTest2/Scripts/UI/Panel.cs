using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.UI
{
    public class Panel : UIElement
    {
        public Panel(Rectangle dimensions) : base(dimensions) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UIManager.PanelBackground, Dimensions, Color.White);
        }
    }
}
