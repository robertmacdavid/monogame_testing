using Microsoft.Xna.Framework;
using MonoGameTest2.Helpers;

namespace MonoGameTest2.UI
{
    public class UIClickEventData
    {
        MouseButtons Button { get; set; }

        public UIClickEventData(MouseButtons button)
        {
            Button = button;
        }

        public UIClickEventData() : this(MouseButtons.LeftButton) { }
    }

    public interface IUIClickable
    {
        bool CheckMouseOver(Vector2 mousePosition);
        bool CheckReleased(MouseButtons mouseButton, Vector2 mousePosition);
        void MouseOver(UIClickEventData e);
        void Click(UIClickEventData e);

        // TODO: CheckPressed
    }
}
