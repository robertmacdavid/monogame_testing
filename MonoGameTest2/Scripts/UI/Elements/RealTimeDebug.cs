using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.UI
{
    public class RealTimeDebug : UIElement
    {
        const string HEADER = "Debug Info\n";

        private static StringBuilder _debugInfo;
        private Text _debugText;

        public RealTimeDebug(UIDimension bounds) : base(bounds)
        {
            var debugPanel = new Panel(this, UIDimension.Full);
            _debugText = new Text(
                debugPanel, 
                new UIDimension()
                {
                    WidthMode = UIDimensionModes.Stretch,
                    Left = 4,
                    Right = 4,
                    HeightMode = UIDimensionModes.Stretch,
                    Top = 4,
                    Bottom = 4,
                },
                ""
            );
            _debugInfo = new StringBuilder(HEADER);
        }

        public static void Append<T>(string label, T value)
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
