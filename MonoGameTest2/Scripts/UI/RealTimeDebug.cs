using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.UI
{
    public class RealTimeDebug : UIElement
    {
        const string HEADER = "Debug Info\n";

        private StringBuilder _debugInfo;
        private UIText _debugText;

        public RealTimeDebug(UIRectangle bounds) : base(bounds)
        {
            var debugPanel = new UIPanel(this, new UIRectangle(0, 0, 1, 1))
            {
                FitToContent = true,
                Padding = new Padding(0.05f),
            };

            _debugText = new UIText(debugPanel, new UIRectangle(0, 0, 1, 1), Color.White, true);
            _debugInfo = new StringBuilder(HEADER);
        }

        public void Append<T>(string label, T value)
        {
            _debugInfo.AppendLine($"{label}: {value.ToString()}");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _debugText.Value = _debugInfo.ToString().Substring(0, _debugInfo.Length - 1);
            _debugInfo.Clear();
            _debugInfo.Append(HEADER);
        }
    }
}
