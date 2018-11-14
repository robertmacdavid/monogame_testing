using Microsoft.Xna.Framework;

namespace MonoGameTest2.UI
{
    /// <summary>
    /// A button with a background and text.
    /// </summary>
    public class TextButton : UIElement
    {
        private Panel _background;
        private Text _text;
        private Button _button;

        public string Value
        {
            get { return _text.Value; }
            set { _text.Value = value; }
        }

        public Color Color
        {
            get { return _background.Color; }
            set { _background.Color = value; }
        }

        public Button.ClickEventHandler OnClick
        {
            get { return _button.OnClick; }
            set { _button.OnClick = value; }
        }

        public TextButton(UIElement parent, UIRectangle bounds, string text, Color color) : base(parent, bounds)
        {
            _button = new Button(this, UIRectangle.Full);
            _background = new Panel(_button, UIRectangle.Full, color);
            _text = new Text(_background, UIRectangle.Full, wordWrap: false)
            {
                Value = text
            };
        }

        public TextButton(UIRectangle bounds, string text, Color color) : this(null, bounds, text, color) { }
    }
}
