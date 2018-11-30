using Microsoft.Xna.Framework;

namespace MonoGameTest2.UI
{
    /// <summary>
    /// A button with a background and text.
    /// </summary>
    public class TextButton : UIElement
    {
        public Text Text;

        private Panel _background;
        private Button _button;

        public string Value
        {
            get { return Text.Value; }
            set { Text.Value = value; }
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

        public Button.ClickEventHandler OnMouseOver
        {
            get { return _button.OnMouseOver; }
            set { _button.OnMouseOver = value; }
        }

        public Button.ClickEventHandler OnMouseOut
        {
            get { return _button.OnMouseOut; }
            set { _button.OnMouseOut = value; }
        }

        public TextButton(UIElement parent, UIDimension dimensions, string text, Color color, Vector2? textAnchor = null, UIDimension? textDimensions = null, Color? textColor = null) : base(parent, dimensions)
        {
            _button = new Button(this, UIDimension.Full);
            _background = new Panel(_button, UIDimension.Full, color);
            Text = new Text(
                _background, 
                textDimensions ?? new UIDimension()
                {
                    WidthMode = UIDimensionModes.Fixed,
                    HeightMode = UIDimensionModes.Fixed,
                },
                text,
                anchor: textAnchor ?? AnchorPoints.Middle,
                color: textColor,
                wordWrap: false
            );
        }

        public TextButton(UIDimension dimensions, string text, Color color, Vector2? textAnchor = null, Color? textColor = null) : this(null, dimensions, text, color, textAnchor) { }
    }
}
