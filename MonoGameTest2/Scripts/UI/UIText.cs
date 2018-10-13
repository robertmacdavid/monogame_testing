using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.UI
{
    class UIText : UIElement
    {
        public bool WordWrap { get; set; }
        public string Value { get; set; }
        public Color Color { get; set; }

        public UIText(UIElement parent, UIRectangle bounds, Color? color = null, bool wordWrap = true) : base(parent, bounds)
        {
            Value = "";
            Color = color ?? Color.White;
            WordWrap = wordWrap;
        }

        public UIText(UIRectangle bounds, Color? color = null, bool wordWrap = true) : this(null, bounds, color, wordWrap) {  }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var defaultFont = UIManager.DefaultFont;
            var newValue = new StringBuilder(Value);

            if (WordWrap)
            {
                var words = Value.Split(' ');
                newValue = new StringBuilder();
                var lineWidth = 0f;
                var spaceWidth = defaultFont.MeasureString(" ").X;

                foreach (var word in words)
                {
                    Vector2 size = defaultFont.MeasureString(word);

                    if (lineWidth + size.X < RelativeBounds.Width)
                    {
                        lineWidth += size.X + spaceWidth;
                    }
                    else
                    {
                        newValue.Append("\n");
                        lineWidth = size.X + spaceWidth;
                    }

                    newValue.Append(word + " ");
                }

                spriteBatch.DrawString(UIManager.DefaultFont, newValue, AbsoluteBounds.RealLocation, Color);
            }
            else
            {
                spriteBatch.DrawString(UIManager.DefaultFont, newValue, AbsoluteBounds.RealLocation, Color);
            }

            var newSize = Managers.UIManager.PixelToUI(defaultFont.MeasureString(newValue));
            Resize(new UIRectangle(RelativeBounds.X, RelativeBounds.Y, newSize.X, newSize.Y));
        }
    }
}
