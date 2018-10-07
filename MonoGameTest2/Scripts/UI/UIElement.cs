using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    public struct Padding
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public Padding(float all)
        {
            Left = Right = Top = Bottom = all;
        }

        public Padding(float leftRight, float topBottom)
        {
            Left = Right = leftRight;
            Top = Bottom = topBottom;
        }

        public Padding(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }

    public struct UIRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Vector2 RealLocation
        {
            get
            {
                return UIManager.UIToPixel(X, Y);
            }
        }

        public Rectangle RealDimensions
        {
            get
            {
                var screenWidth = GameManager.Instance.Game.GraphicsDevice.Viewport.Width;
                var screenHeight = GameManager.Instance.Game.GraphicsDevice.Viewport.Height;
                return new Rectangle((int)(screenWidth * X), (int)(screenHeight * Y), (int)(screenWidth * Width), (int)(screenHeight * Height));
            }
        }

        public float Left
        {
            get { return X; }
        }

        public float Right
        {
            get { return X + Width; }
        }

        public float Top
        {
            get { return Y; }
        }

        public float Bottom
        {
            get { return Y + Height; }
        }

        public UIRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public abstract class UIElement
    {
        private bool _active;
        public bool Active {
            get { return _active; }
            set
            {
                _active = value;

                foreach (var child in Children)
                {
                    child.Active = _active;
                }
            }
        }

        private UIRectangle _relativeBounds;
        public UIRectangle RelativeBounds
        {
            get { return _relativeBounds; }
            set { Resize(value); }
        }

        private bool _fitToContent;
        public bool FitToContent
        {
            get { return _fitToContent; }
            set
            {
                _fitToContent = value;
                Resize();
            }
        }


        public UIRectangle AbsoluteBounds { get; private set; }
        public List<UIElement> Children { get; private set; }
        public UIManager UIManager => GameManager.Instance.UIManager;

        public UIElement Parent;
        public Padding Padding;

        public UIElement(UIElement parent, UIRectangle bounds)
        {
            RelativeBounds = bounds;
            Children = new List<UIElement>();

            if (parent == null)
            {
                AbsoluteBounds = bounds;
                Active = true;
            }
            else
            {
                Resize();
                Active = parent.Active;
            }
        }

        public UIElement(UIRectangle bounds) : this(null, bounds) {}

        public void AddChild(UIElement uiElement)
        {
            uiElement.Parent = this;
            Children.Add(uiElement);

            uiElement.Resize();
        }

        public void Resize()
        {
            float left = AbsoluteBounds.Left;
            float right = AbsoluteBounds.Right;
            float top = AbsoluteBounds.Top;
            float bottom = AbsoluteBounds.Bottom;

            if (!FitToContent && Parent != null)
            {
                left = Parent.AbsoluteBounds.Left + RelativeBounds.Left * Parent.AbsoluteBounds.Width + Parent.Padding.Left * Parent.AbsoluteBounds.Width;
                right = Parent.AbsoluteBounds.Right + RelativeBounds.Right * Parent.AbsoluteBounds.Width - Parent.Padding.Right * Parent.AbsoluteBounds.Width;
                top = Parent.AbsoluteBounds.Top + RelativeBounds.Top * Parent.AbsoluteBounds.Height + Parent.Padding.Top * Parent.AbsoluteBounds.Height;
                bottom = Parent.AbsoluteBounds.Bottom + RelativeBounds.Bottom * Parent.AbsoluteBounds.Height - Parent.Padding.Bottom * Parent.AbsoluteBounds.Height;
            }
            /*else if (FitToContent)
            {
                foreach (var child in Children)
                {
                    left = Math.Min(child.AbsoluteBounds.Left, left);
                    right = Math.Max(child.AbsoluteBounds.Right, right);
                    top = Math.Min(child.AbsoluteBounds.Top, top);
                    bottom = Math.Max(child.AbsoluteBounds.Bottom, bottom);
                }

                left -= Padding.Left;
                right += Padding.Right;
                top -= Padding.Top;
                bottom += Padding.Bottom;
            }*/

            AbsoluteBounds = new UIRectangle(left, top, AbsoluteBounds.Width, AbsoluteBounds.Height);

            if (Parent != null)
            {
                Parent.Resize();
            }
        }

        public void Resize(UIRectangle newBounds)
        {
            _relativeBounds = newBounds;
            Resize();
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
