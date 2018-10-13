using Microsoft.Xna.Framework;
using MonoGameTest2.Helpers;

namespace MonoGameTest2.UI
{
    public class UIClickEventData
    {
        public MouseButtons Button { get; set; }
        public Vector2 Position { get; set; }

        public UIClickEventData(Vector2 position, MouseButtons button)
        {
            Button = button;
            Position = position;
        }
    }

    public interface IUIClickable
    {
        bool CheckMouseOver(Vector2 mousePosition);
        bool CheckReleased(UIClickEventData e);
        void MouseOver(UIClickEventData e);
        void Click(UIClickEventData e);

        // TODO: CheckPressed
    }
}
