﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Helpers;

namespace MonoGameTest2.UI
{
    public class InputBox : UIElement, IUIClickable, IUIFocusable
    {
        private Text _inputText;
        private Panel _background;

        public string Value {
            get { return _inputText.Value; }
            private set { _inputText.Value = value; }
        }

        private bool _focused;
        public bool Focused {
            get { return _focused; }
            set {
                _focused = value;
                if (value)
                {
                    Focus();
                }
                else
                {
                    Unfocus();
                }
            }
        }

        public delegate void ChangeEventHandler(string e);
        public ChangeEventHandler OnChange { get; set; }

        public InputBox(UIElement parent, UIRectangle bounds, bool multiline = false) : base(parent, bounds)
        {
            _background = new Panel(this, UIRectangle.Full, Color.Black);
            _inputText = new Text(_background, UIRectangle.Full, wordWrap: multiline);
        }

        public InputBox(UIRectangle bounds) : this(null, bounds) { }

        public bool CheckMouseOver(Vector2 mousePosition)
        {
            return AbsoluteBounds.Contains(mousePosition);
        }

        public bool CheckReleased(UIMouseEventData e)
        {
            return AbsoluteBounds.Contains(e.Position);
        }

        public bool MouseOver(UIMouseEventData e)
        {
            return true;
        }

        public void Click(UIMouseEventData e)
        {
            Focused = true;
        }

        public void Focus()
        {
            _background.Color = Color.Gray;
        }
        
        public void FocusUpdate()
        {
            var typedLetter = Keyboard.GetState().GetTypedLetter();

            if (typedLetter.Key == Keys.Back && Value.Length > 0)
            {
                Value = Value.Substring(0, Value.Length - 1);
                return;
            }
            
            if (typedLetter.String.Length == 0)
            {
                return;
            }

            Value += typedLetter.String;
        }

        public void Unfocus()
        {
            throw new System.NotImplementedException();
        }
    }
}
