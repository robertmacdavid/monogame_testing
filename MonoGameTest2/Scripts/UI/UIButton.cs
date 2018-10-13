using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Helpers;
using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    class UIButton : UIElement, IUIClickable
    {
        public delegate void ClickEventHandler(UIClickEventData e);
        public ClickEventHandler OnClick { get; set; }
        public ClickEventHandler OnMouseOver { get; set; }

        public UIButton(UIElement parent, UIRectangle bounds) : base(parent, bounds) { }
        public UIButton(UIRectangle bounds) : this(null, bounds) { }

        public bool CheckReleased(UIClickEventData e)
        {
            return AbsoluteBounds.Contains(e.Position);
        }

        public bool CheckMouseOver(Vector2 mousePosition)
        {
            return AbsoluteBounds.Contains(mousePosition);
        }

        public void MouseOver(UIClickEventData e)
        {
            OnMouseOver?.Invoke(e);
        }

        public void Click(UIClickEventData e)
        {
            OnClick?.Invoke(e);
        }
    }
}
