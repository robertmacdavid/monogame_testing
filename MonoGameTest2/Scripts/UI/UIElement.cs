using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    public abstract class UIElement
    {
        private bool _active;
        public bool Active {
            get
            {
                return _active;
            }

            set
            {
                _active = value;

                foreach (var child in Children)
                {
                    child.Active = _active;
                }
            }
        }

        public UIManager UIManager => GameManager.Instance.UIManager;
        public List<UIElement> Children { get; private set; }
        public Rectangle Dimensions;

        public UIElement(Rectangle dimensions)
        {
            Dimensions = dimensions;
            Children = new List<UIElement>();
            Active = true;
        }

        public void AddChild(UIElement uiElement)
        {
            Children.Add(uiElement);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
