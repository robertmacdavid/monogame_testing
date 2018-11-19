using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.UI
{
    public class Text : UIElement
    {
        private string _value;
        public string Value
        {
            get { return _value; }
            set { SetValue(value); }
        }
        public bool WordWrap { get; set; }
        public Color Color { get; set; }

        private StringBuilder _displayString;

        public Text(UIElement parent, UIDimension dimensions, string value, Vector2? anchor = null, Color? color = null, bool wordWrap = true) : base(parent, dimensions, anchor)
        {
            Color = color ?? Color.White;
            WordWrap = wordWrap;
            _displayString = new StringBuilder();
            Value = value;
        }

        public Text(UIDimension dimensions, string value, Vector2? anchor = null, Color? color = null, bool wordWrap = true) : this(null, dimensions, value, anchor, color, wordWrap) {  }

        private void SetValue(string value)
        {
            _displayString.Clear();
            _value = value;
            var defaultFont = UIManager.DefaultFont;

            if (WordWrap)
            {
                var lines = Value.Split('\n');
                var currentLineWidth = 0;
                var spaceWidth = (int)defaultFont.MeasureString(" ").X;
                _displayString.Clear();

                foreach (var line in lines)
                {
                    var words = line.Split(' ');
                    foreach (var word in words)
                    {
                        var wordSize = defaultFont.MeasureString(word);

                        if (currentLineWidth + wordSize.X <= AbsoluteDimensions.Width)
                        {
                            currentLineWidth += (int)wordSize.X + spaceWidth;
                        }
                        else
                        {
                            _displayString.AppendLine();
                            currentLineWidth = (int)wordSize.X + spaceWidth;
                        }

                        _displayString.Append(word + " ");
                    }

                    _displayString.AppendLine();
                    currentLineWidth = 0;
                }
            }
            else
            {
                _displayString.Append(value);
            }

            var newDimensions = defaultFont.MeasureString(_displayString.ToString());
            Dimensions = new UIDimension()
            {
                WidthMode = UIDimensionModes.Fixed,
                Width = (int)newDimensions.X,
                HeightMode = UIDimensionModes.Stretch,
                Height = (int)newDimensions.Y
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(UIManager.DefaultFont, _displayString.ToString(), AbsoluteDimensions.Location.ToVector2(), Color);
        }
    }
}
