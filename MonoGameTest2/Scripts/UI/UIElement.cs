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

    public class UIRectangle
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
                var screenWidth = GameManager.Instance.ScreenWidth;
                var screenHeight = GameManager.Instance.ScreenHeight;
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

        public bool Contains(Vector2 point)
        {
            return point.X >= X && point.X <= (X + Width) && point.Y >= Y && point.Y <= (Y + Height); 
        }

        public override string ToString()
        {
            return $"UIRectangle: {{ X: {X}, Y: {Y}, Width:{Width}, Height:{Height} }}";
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

        public Texture2D Texture;

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
                parent.AddChild(this);
            }
        }

        public UIElement(UIRectangle bounds) : this(null, bounds) {}

        public void AddChild(UIElement uiElement)
        {
            uiElement.Parent = this;
            uiElement.Active = Active;
            Children.Add(uiElement);

            uiElement.Resize();
        }

        public void Resize()
        {
            var parentLeft = Parent?.AbsoluteBounds.Left ?? 0;
            var parentTop = Parent?.AbsoluteBounds.Top ?? 0;
            var parentWidth = Parent?.AbsoluteBounds.Width ?? 1;
            var parentHeight = Parent?.AbsoluteBounds.Height ?? 1;

            var left = parentLeft + RelativeBounds.Left * parentWidth + (Parent?.Padding.Left ?? 0) * parentWidth;
            var top = parentTop + RelativeBounds.Top * parentHeight + (Parent?.Padding.Top ?? 0) * parentHeight;
            var width = parentWidth * RelativeBounds.Width - ((Parent?.Padding.Left ?? 0) + (Parent?.Padding.Right ?? 0)) * parentWidth;
            var height = parentHeight * RelativeBounds.Height - ((Parent?.Padding.Top ?? 0) + (Parent?.Padding.Bottom ?? 0)) * parentHeight;

            /*if (FitToContent && Children.Count > 0)
            {
                var leftMost = Children[0].AbsoluteBounds.Left;
                var topMost = Children[0].AbsoluteBounds.Top;
                var rightMost = Children[0].AbsoluteBounds.Right;
                var bottomMost = Children[0].AbsoluteBounds.Bottom;

                foreach (var child in Children)
                {
                    leftMost = Math.Min(child.AbsoluteBounds.Left, leftMost);
                    topMost = Math.Min(child.AbsoluteBounds.Top, topMost);
                    rightMost = Math.Max(child.AbsoluteBounds.Right, rightMost);
                    bottomMost = Math.Max(child.AbsoluteBounds.Bottom, bottomMost);
                }

                var childrenWidth = rightMost - leftMost;
                var childrenHeight = bottomMost - topMost;

                if (!float.IsNaN(childrenWidth) && width < childrenWidth)
                {
                    width = childrenWidth + Padding.Left + Padding.Right;
                }

                if (!float.IsNaN(childrenHeight) && height < childrenHeight)
                {
                    height = childrenHeight + Padding.Top + Padding.Bottom;
                }
            }*/

            AbsoluteBounds = new UIRectangle(left, top, width, height);

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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, AbsoluteBounds.RealDimensions, Color.White);
            }
        }
    }
}
