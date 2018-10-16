﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    public class UIPanel : UIElement, IUIClickable
    {
        public Color Color;

        public UIPanel(UIElement parent, UIRectangle bounds, Color? color = null) : base(parent, bounds)
        {
            Color = color ?? new Color(0.2f, 0.2f, 0.2f, 0.7f);
            Texture = UIManager.PanelBackground;
        }

        public UIPanel(UIRectangle bounds) : this(null, bounds) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, AbsoluteBounds.RealDimensions, Color);
        }

        public bool CheckMouseOver(Vector2 mousePosition)
        {
            return AbsoluteBounds.Contains(mousePosition);
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
            throw new System.Exception("This shouldn't ever execute.");
        }
    }
}
