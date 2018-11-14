using System.Collections.Generic;
using System.Linq;

namespace MonoGameTest2.UI
{
    public class DebugConsole : UIElement
    {
        private const int DRAWN_LINES = 10;
        private const int MAX_LINES = 1000;

        private static Queue<string> _lines;
        private static Text _text;

        private static DebugConsole _instance;

        /// <summary>
        /// A console to display shit.
        /// TODO: Scrolling.
        /// TODO: Colors?
        /// TODO: Input?
        /// </summary>
        /// <param name="bounds"></param>
        public DebugConsole(UIRectangle bounds) : base(bounds)
        {
            if (_instance != null)
            {
                throw new System.Exception("Cannot create multiple instances of debug console.");
            }

            bounds.Height = DRAWN_LINES * 0.05f;

            var panel = new Panel(this, new UIRectangle(0, 0, 1, 1));
            _text = new Text(panel, new UIRectangle(0, 0, 1, 1), wordWrap: false);
            _lines = new Queue<string>(MAX_LINES);

            _instance = this;
        }

        public static void WriteLine<T>(T newLine)
        {
            if (_lines.Count == MAX_LINES)
            {
                _lines.Dequeue();
            }

            _lines.Enqueue(newLine.ToString());

            var consoleText = "";
            foreach (var line in _lines.Skip(_lines.Count - DRAWN_LINES))
            {
                consoleText += line + "\n";
            }

            _text.Value = consoleText.Substring(0, consoleText.Length - 1);
        }
    }
}
