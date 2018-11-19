using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameTest2.Managers;

namespace MonoGameTest2.UI
{
    public static class AnchorPoints
    {
        public static readonly Vector2 Middle = new Vector2(0.5f, 0.5f);
        public static readonly Vector2 TopLeft = new Vector2(0, 0);
        public static readonly Vector2 TopRight = new Vector2(1, 0);
        public static readonly Vector2 BottomLeft = new Vector2(0, 1);
        public static readonly Vector2 BottomRight = new Vector2(1, 1);
        public static readonly Vector2 MiddleLeft = new Vector2(0, 0.5f);
        public static readonly Vector2 MiddleRight = new Vector2(1, 0.5f);
        public static readonly Vector2 TopMiddle = new Vector2(0.5f, 0);
        public static readonly Vector2 BottomMiddle = new Vector2(0.5f, 1);
    }

    public enum UIDimensionModes
    {
        Stretch,
        Fixed
    }

    public struct UIDimension
    {
        public static UIDimension Full = new UIDimension
        {
            WidthMode = UIDimensionModes.Stretch,
            HeightMode = UIDimensionModes.Stretch,
            Left = 0,
            Right = 0,
            Top = 0,
            Bottom = 0,
        };

        public UIDimensionModes WidthMode;
        public UIDimensionModes HeightMode;
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;

        public int X;
        public int Y;
        public int Width;
        public int Height;
    }

    public abstract class UIElement
    {
        public UIManager UIManager => GameManager.Instance.UIManager;

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { SetActive(value); }
        }

        private Vector2 _pivot;
        public Vector2 Pivot {
            get { return _pivot; }
            set
            {
                _pivot = value;
                Resize();
            }
        }

        private Vector2 _anchor;
        public Vector2 Anchor {
            get { return _anchor; }
            set
            {
                _anchor = value;
                Resize();
            }
        }

        private UIDimension _dimensions;
        public UIDimension Dimensions
        {
            get { return _dimensions; }
            set
            {
                _dimensions = value;
                Resize();
            }
        }
        private UIElement _parent;
        public UIElement Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                Resize();
            }
        }

        public Texture2D Texture { get; set; }
        public List<UIElement> Children { get; private set; }
        public Rectangle AbsoluteDimensions { get; private set; }

        public UIElement(UIElement parent, UIDimension dimensions, Vector2? anchor = null)
        {
            _parent = parent;
            _dimensions = dimensions;
            _anchor = anchor ?? AnchorPoints.TopLeft;
            _pivot = new Vector2(0.5f, 0.5f);
            Children = new List<UIElement>();

            Resize();
            parent?.AddChild(this);
            Active = true;
        }

        public UIElement(UIDimension dimensions, Vector2? anchor = null) : this(null, dimensions, anchor) { }

        private void SetActive(bool active)
        {
            _active = active;

            foreach (var child in Children)
            {
                child.Active = _active;
            }
        }

        public virtual void AddChild(UIElement child)
        {
            child.Parent = this;
            child.Active = Active;
            Children.Add(child);
            child.Resize();
        }

        protected virtual void Resize()
        {
            var newDimensions = new Rectangle();
            var parentX = Parent != null ? Parent.AbsoluteDimensions.X : 0;
            var parentY = Parent != null ? Parent.AbsoluteDimensions.Y : 0;
            var parentWidth = Parent != null ? Parent.AbsoluteDimensions.Width : GameManager.NATIVE_SCREEN_WIDTH;
            var parentHeight = Parent != null ? Parent.AbsoluteDimensions.Height : GameManager.NATIVE_SCREEN_HEIGHT;

            if (Dimensions.WidthMode == UIDimensionModes.Stretch)
            {
                newDimensions.X = parentX + Dimensions.Left;
                newDimensions.Width = parentWidth - Dimensions.Left - Dimensions.Right;
            }
            else
            {
                newDimensions.X = parentX + (int)(Anchor.X * parentWidth) + (Dimensions.X - (int)(Pivot.X * Dimensions.Width));
                newDimensions.Width = Dimensions.Width;
            }

            if (Dimensions.HeightMode == UIDimensionModes.Stretch)
            {
                newDimensions.Y = parentY + Dimensions.Top;
                newDimensions.Height = parentHeight - Dimensions.Top - Dimensions.Bottom;
            }
            else
            {
                newDimensions.Y = parentY + (int)(Anchor.Y * parentHeight) + (Dimensions.Y - (int)(Pivot.Y * Dimensions.Height));
                newDimensions.Height = Dimensions.Height;
            }

            AbsoluteDimensions = newDimensions;

            foreach (var child in Children)
            {
                child.Resize();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, AbsoluteDimensions, Color.White);
            }
        }
    }
}
