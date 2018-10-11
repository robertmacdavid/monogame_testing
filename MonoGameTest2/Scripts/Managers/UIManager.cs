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
            var screenWidth = GameManager.Instance.ScreenWidth;
            var screenHeight = GameManager.Instance.ScreenHeight;

            return new Vector2(pixelCoords.X / screenWidth, pixelCoords.Y / screenHeight);
        }

        public static Vector2 UIToPixel(Vector2 uiCoords)
        {
            return UIToPixel(uiCoords.X, uiCoords.Y);
        }

        public static Vector2 UIToPixel(float X, float Y)
        {
            var screenWidth = GameManager.Instance.ScreenWidth;
            var screenHeight = GameManager.Instance.ScreenHeight;

            return new Vector2((int)(X * screenWidth), (int)(Y * screenHeight));
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

        public void Update()
        {
            // TODO: Check other mouse buttons.
            // TODO: Check if mouse is down over UI element.
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.GetScreenPosition();
            var mouseButton = mouseState.GetButtonReleased(MouseButtons.LeftButton) ? MouseButtons.LeftButton : MouseButtons.None;
            var e = new UIClickEventData(mouseButton);

            foreach (var element in _elements)
            {
                if (element is IUIClickable)
                {
                    var clickable = element as IUIClickable;
                    if (clickable.CheckMouseOver(mousePosition))
                    {
                        clickable.MouseOver(e);
                    }

                    if (mouseButton == MouseButtons.LeftButton && clickable.CheckReleased(MouseButtons.LeftButton, mousePosition)) {
                        clickable.Click(e);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in _elements)
            {
                if (!element.Active) continue;

                element.Draw(spriteBatch);

                foreach (var child in element.Children)
                {
                    child.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }
    }
}
  