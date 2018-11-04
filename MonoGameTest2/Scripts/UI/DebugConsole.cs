using System.Collections.Generic;
using System.Linq;

namespace MonoGameTest2.UI
{
    public class DebugConsole : UIElement
    {
        private const int MAX_LINES = 1000;

        private Queue<string> _lines;
        private UIText _text;

        /// <summary>
        /// A console to display shit.
        /// TODO: Scrolling.
        /// TODO: Colors?
        /// TODO: Input?
        /// </summary>
        /// <param name="bounds"></param>
        public DebugConsole(UIRectangle bounds) : base(bounds)
        {
            var panel = new UIPanel(this, new UIRectangle(0, 0, 1, 1));
            _text = new UIText(panel, new UIRectangle(0, 0, 1, 1), wordWrap: false);
            _lines = new Queue<string>(MAX_LINES);
        }

        public void AddLine<T>(T newLine)
        {
            if (_lines.Count == MAX_LINES)
            {
                _lines.Dequeue();
            }

            _lines.Enqueue(newLine.ToString());

            var consoleText = "";
            foreach (var line in _lines.Skip(_lines.Count - 10))
            {
                consoleText += line + "\n";
            }

            _text.Value = consoleText.Substring(0, consoleText.Length - 1);
        }
    }
}
