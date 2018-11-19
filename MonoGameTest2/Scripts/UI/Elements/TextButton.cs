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

        public TextButton(UIElement parent, UIDimension dimensions, string text, Color color) : base(parent, dimensions)
        {
            _button = new Button(this, UIDimension.Full);
            _background = new Panel(_button, UIDimension.Full, color);
            _text = new Text(_background, UIDimension.Full, wordWrap: false)
            {
                Anchor = AnchorPoints.Middle,
                Value = text
            };
        }

        public TextButton(UIDimension dimensions, string text, Color color) : this(null, dimensions, text, color) { }
    }
}
