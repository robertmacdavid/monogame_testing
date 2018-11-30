using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    public class Panel : UIElement, IUIClickable
    {
        public Color Color;

        public bool MousedOver { get; set; }

        public Panel(UIElement parent, UIDimension dimension, Color? color = null) : base(parent, dimension)
        {
            Color = color ?? new Color(0.2f, 0.2f, 0.2f, 0.7f);
            Texture = UIManager.PanelBackground;
        }

        public Panel(UIDimension dimension, Color? color = null) : this(null, dimension, color) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, AbsoluteDimensions, Color);
        }

        public bool CheckMouseOver(Vector2 mousePosition)
        {
            return AbsoluteDimensions.Contains(mousePosition);
        }

        public bool CheckReleased(UIMouseEventData e)
        {
            return false;
        }

        public bool MouseOver(UIMouseEventData e)
        {
            return true;
        }

        public void Click(UIMouseEventData e)
        {
            throw new System.Exception("This shouldn't execute.");
        }

        public void MouseOut(UIMouseEventData e) { }
    }
}
