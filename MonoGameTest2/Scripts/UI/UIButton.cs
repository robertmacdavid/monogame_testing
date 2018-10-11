using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Helpers;
using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    class UIButton : UIElement, IUIClickable
    {
        public delegate void ClickEventHandler(UIClickEventData e);
        public ClickEventHandler OnClick;
        public ClickEventHandler OnMouseOver;

        public UIButton(UIElement parent, UIRectangle dimensions) : base(parent, dimensions) { }
        public UIButton(UIRectangle dimensions) : this(null, dimensions) { }

        public bool CheckReleased(MouseButtons mouseButton, Vector2 mousePosition)
        {
            return AbsoluteBounds.Contains(mousePosition);
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
