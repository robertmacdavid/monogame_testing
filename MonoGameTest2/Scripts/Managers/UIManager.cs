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

        public void LoadContent(ContentManager contentManager)
        {
            DefaultFont = contentManager.Load<SpriteFont>("default_font");

            PanelBackground = new Texture2D(GameManager.Instance.Game.GraphicsDevice, 1, 1);
            PanelBackground.SetData(new Color[] { new Color(Color.Black, 0.5f) });
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
  