using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Helpers;
using MonoGameTest2.UI;

namespace MonoGameTest2.Managers
{
    public class UIManager
    {
        public SpriteFont DefaultFont;
        public Texture2D PanelBackground;

        private List<UIElement> _elements;

        public UIManager()
        {
            _elements = new List<UIElement>();
        }

        public static Vector2 PixelToUI(Vector2 pixelCoords)
        {
            return new Vector2(pixelCoords.X / GameManager.NATIVE_SCREEN_WIDTH, pixelCoords.Y / GameManager.NATIVE_SCREEN_HEIGHT);
        }

        public static Vector2 UIToPixel(Vector2 uiCoords)
        {
            return UIToPixel(uiCoords.X, uiCoords.Y);
        }

        public static Vector2 UIToPixel(float X, float Y)
        {
            return new Vector2((int)(X * GameManager.NATIVE_SCREEN_WIDTH), (int)(Y * GameManager.NATIVE_SCREEN_HEIGHT));
        }

        public void LoadContent(ContentManager contentManager)
        {
            DefaultFont = contentManager.Load<SpriteFont>("default_font");

            PanelBackground = new Texture2D(GameManager.Instance.Game.GraphicsDevice, 1, 1);
            PanelBackground.SetData(new Color[] { Color.White });
        }

        public void AddElement(UIElement element)
        {
            _elements.Add(element);
        }

        /// <summary>
        /// Updates any UI elements.
        /// </summary>
        /// <returns>Whether or not we should block mouse actions when updating entities.</returns>
        public bool Update()
        {
            // TODO: Check other mouse buttons.
            // TODO: Check if mouse is down over UI element.
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.GetScreenPosition();
            var mouseButton = mouseState.GetButtonUp(MouseButtons.LeftButton) ? MouseButtons.LeftButton : MouseButtons.None;
            var e = new UIMouseEventData(mousePosition, mouseButton);


            var block = false;
            foreach (var element in _elements)
            {
                block |= UpdateHelper(element, e);
            }

            return block;
        }

        private bool UpdateHelper(UIElement element, UIMouseEventData e)
        {
            var block = false;

            if (element is IUIClickable && element.Active)
            {
                var clickable = element as IUIClickable;

                if (clickable.CheckMouseOver(e.Position))
                {
                    block = clickable.MouseOver(e);
                }

                if (e.Button == MouseButtons.LeftButton && clickable.CheckReleased(e))
                {
                    clickable.Click(e);
                }
            }

            foreach (var child in element.Children)
            {
                block |= UpdateHelper(child, e); 
            }

            return block;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in _elements)
            {
                DrawHelper(element, spriteBatch);
            }

            spriteBatch.End();
        }

        private void DrawHelper(UIElement element, SpriteBatch spriteBatch)
        {
            if (element.Active)
            {
                element.Draw(spriteBatch);
            }

            foreach (var child in element.Children)
            {
                DrawHelper(child, spriteBatch);
            }
        }
    }
}
  