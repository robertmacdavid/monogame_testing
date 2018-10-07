using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
            var screenWidth = GameManager.Instance.Game.GraphicsDevice.Viewport.Width;
            var screenHeight = GameManager.Instance.Game.GraphicsDevice.Viewport.Height;

            return new Vector2(pixelCoords.X / screenWidth, pixelCoords.Y / screenHeight);
        }

        public static Vector2 UIToPixel(Vector2 uiCoords)
        {
            return UIToPixel(uiCoords.X, uiCoords.Y);
        }

        public static Vector2 UIToPixel(float X, float Y)
        {
            var screenWidth = GameManager.Instance.Game.GraphicsDevice.Viewport.Width;
            var screenHeight = GameManager.Instance.Game.GraphicsDevice.Viewport.Height;

            return new Vector2((int)(X * screenWidth), (int)(Y * screenHeight));
        }

        public void LoadContent(ContentManager contentManager)
        {
            DefaultFont = contentManager.Load<SpriteFont>("default_font");

            PanelBackground = new Texture2D(GameManager.Instance.Game.GraphicsDevice, 1, 1);
            PanelBackground.SetData(new Color[] { new Color(0.2f, 0.2f, 0.2f, 0.7f) });
        }

        public void AddElement(UIElement element)
        {
            _elements.Add(element);
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
  