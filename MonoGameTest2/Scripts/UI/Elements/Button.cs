﻿using Microsoft.Xna.Framework;

namespace MonoGameTest2.UI
{
    public class Button : UIElement, IUIClickable
    {
        public delegate void ClickEventHandler(UIMouseEventData e);
        public ClickEventHandler OnClick { get; set; }
        public ClickEventHandler OnMouseOver { get; set; }

        public Button(UIElement parent, UIDimension dimensions, Vector2? anchor = null) : base(parent, dimensions, anchor) { }
        public Button(UIDimension dimensions, Vector2? anchor = null) : this(null, dimensions, anchor) { }

        public bool CheckReleased(UIMouseEventData e)
        {
            DebugConsole.WriteLine(AbsoluteDimensions);
            DebugConsole.WriteLine(e.Position);

            return AbsoluteDimensions.Contains(e.Position);
        }

        public bool CheckMouseOver(Vector2 mousePosition)
        {
            return AbsoluteDimensions.Contains(mousePosition);
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
