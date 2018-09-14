using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.UI
{
    class Text : UIElement
    {
        public bool WordWrap { get; set; }
        public string Value { get; set; }
        public Color Color { get; set; }

        public Text(Rectangle dimensions, Color color, bool wordWrap = true) : base(dimensions)
        {
            Color = color;
            WordWrap = wordWrap;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var defaultFont = UIManager.DefaultFont;

            
            if (WordWrap)
            {
                var words = Value.Split(' ');
                var newValue = new StringBuilder();
                var lineWidth = 0f;
                var spaceWidth = defaultFont.MeasureString(" ").X;

                foreach (var word in words)
                {
                    Vector2 size = defaultFont.MeasureString(word);

                    if (lineWidth + size.X < Dimensions.Width)
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

                spriteBatch.DrawString(UIManager.DefaultFont, newValue, Dimensions.Location.ToVector2(), Color);
            }
            else
            {
                spriteBatch.DrawString(UIManager.DefaultFont, Value, Dimensions.Location.ToVector2(), Color);
            }
        }
    }
}
