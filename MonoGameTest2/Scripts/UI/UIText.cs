using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.UI
{
    class UIText : UIElement
    {
        public bool WordWrap { get; set; }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        public Color Color { get; set; }

        private StringBuilder _displayString;

        public UIText(UIElement parent, UIRectangle bounds, Color? color = null, bool wordWrap = true) : base(parent, bounds)
        {
            Color = color ?? Color.White;
            WordWrap = wordWrap;
            _displayString = new StringBuilder();
        }

        public UIText(UIRectangle bounds, Color? color = null, bool wordWrap = true) : this(null, bounds, color, wordWrap) {  }

        private void SetValue(string value)
        {
            _displayString.Clear();
            _value = value;

            if (WordWrap)
            {
                var defaultFont = UIManager.DefaultFont;
                var words = Value.Split(' ', '\n');
                var currentLineWidth = 0;
                var spaceWidth = (int)defaultFont.MeasureString(" ").X;
                _displayString.Clear();

                foreach (var word in words)
                {
                    var wordSize = defaultFont.MeasureString(word);

                    if (currentLineWidth + wordSize.X <= AbsoluteBounds.RealDimensions.Width)
                    {
                        currentLineWidth += (int)wordSize.X + spaceWidth;
                    }
                    else
                    {
                        _displayString.Append("\n");
                        currentLineWidth = (int)wordSize.X + spaceWidth;
                    }

                    _displayString.Append(word + " ");
                }
            }
            else
            {
                _displayString.Append(value);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(UIManager.DefaultFont, _displayString.ToString(), AbsoluteBounds.RealLocation, Color);
            //var newSize = Managers.UIManager.PixelToUI(defaultFont.MeasureString(newValue));
            //Resize(new UIRectangle(RelativeBounds.X, RelativeBounds.Y, newSize.X, newSize.Y));
        }
    }
}
