using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Helpers;
using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    class UIButton : UIElement, IUIClickable
    {
        public delegate void ClickEventHandler(UIMouseEventData e);
        public ClickEventHandler OnClick { get; set; }
        public ClickEventHandler OnMouseOver { get; set; }

        public UIButton(UIElement parent, UIRectangle bounds) : base(parent, bounds) { }
        public UIButton(UIRectangle bounds) : this(null, bounds) { }

        public bool CheckReleased(UIMouseEventData e)
        {
            return AbsoluteBounds.Contains(e.Position);
        }

        public bool CheckMouseOver(Vector2 mousePosition)
        {
            return AbsoluteBounds.Contains(mousePosition);
        }

        public bool MouseOver(UIMouseEventData e)
        {
            OnMouseOver?.Invoke(e);
            return true;
        }

        public void Click(UIMouseEventData e)
        {
            OnClick?.Invoke(e);
        }
    }
}
