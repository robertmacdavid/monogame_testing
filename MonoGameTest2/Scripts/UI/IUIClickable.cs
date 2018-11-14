using Microsoft.Xna.Framework;
using MonoGameTest2.Helpers;

namespace MonoGameTest2.UI
{
    public class UIMouseEventData
    {
        public MouseButtons Button { get; set; }
        public Vector2 Position { get; set; }

        public UIMouseEventData(Vector2 position, MouseButtons button)
        {
            Button = button;
            Position = position;
        }
    }

    public interface IUIClickable
    {
        bool CheckMouseOver(Vector2 mousePosition);

        bool CheckReleased(UIMouseEventData e);

        /// <summary>
        /// The action to do when this UI element is moused over.
        /// </summary>
        /// <param name="e">The event data for the mouse event.</param>
        /// <returns>Does this element block entity updates?</returns>
        bool MouseOver(UIMouseEventData e);

        void Click(UIMouseEventData e);

        // TODO: CheckPressed
    }
}
