﻿using System.Collections.Generic;
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
            PanelBackground = GameManager.PIXEL_TEXTURE;
        }

        public void AddElement(UIElement element)
        {
            _elements.Add(element);
        }

        public void RemoveElement(UIElement element)
        {
            element.Deleted = true;
        }

        /// <summary>
        /// Updates any UI elements.
        /// </summary>
        /// <returns>Whether or not we should block mouse actions when updating entities.</returns>
        public bool Update()
        {
            foreach (var element in _elements)
            {
                if (element.Active && element is IUIFocusable)
                {
                    var focusable = element as IUIFocusable;

                    if (focusable.Focused)
                    {
                        focusable.FocusUpdate();
                    }
                }
            }

            // TODO: Check other mouse buttons.
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.GetPosition();
            var mouseButton = mouseState.GetButtonUp(MouseButtons.LeftButton) ? MouseButtons.LeftButton : MouseButtons.None;
            var e = new UIMouseEventData(mousePosition, mouseButton);
            
            var block = false;
            foreach (var element in _elements) 
            {
                block |= UpdateHelper(element, e);
            }

            _elements.RemoveAll(element => element.Deleted);

            return block;
        }

        private bool UpdateHelper(UIElement element, UIMouseEventData e)
        {
            var block = false;

            if (element.Active && !element.Deleted)
            {
                if (element is IUIClickable)
                {
                    var clickable = element as IUIClickable;

                    if (clickable.CheckMouseOver(e.Position))
                    {
                        clickable.MousedOver = true;
                        block = clickable.MouseOver(e);
                    }
                    else
                    {
                        if (clickable.MousedOver)
                        {
                            clickable.MouseOut(e);
                        }
                        clickable.MousedOver = false;
                    }
                    

                    if (e.Button == MouseButtons.LeftButton && clickable.CheckReleased(e))
                    {
                        clickable.Click(e);
                    }
                }

                if (element is IUIFocusable)
                {
                    var focusable = element as IUIFocusable;

                    if (focusable.Focused)
                    {
                        focusable.FocusUpdate();
                    }
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
  