using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace MonoGameTest2.UI
{
    public class SelectorList : UIElement
    {
        private static readonly Color backgroundColor = Color.Black;
        private static readonly Color highlightColor = Color.Gray;
        private const int PixelsPerLine = 24;

        public struct Option
        {
            public string Name;
            public int Value;

            public Option(string name, int value)
            {
                Name = name;
                Value = value;
            }
        }

        public delegate void SelectorChangeEventHandler(Option option);
        public SelectorChangeEventHandler OnChange { get; set; }

        public List<Option> Options { get; private set; }
        public Option? Selected { get; private set; }

        private List<TextButton> _optionButtons;
        private readonly int _maxEntries;
        private readonly float _entryHeight;

        public SelectorList(UIElement parent, UIRectangle bounds) : base(parent, bounds)
        {
            _optionButtons = new List<TextButton>();
            Options = new List<Option>();

            var background = new Panel(this, UIRectangle.Full, backgroundColor);
            _maxEntries = bounds.RealDimensions.Height / PixelsPerLine;
            _entryHeight = RelativeBounds.Height / _maxEntries;
        }

        public SelectorList(UIRectangle bounds) : this(null, bounds) { }

        public void AddOption(Option option)
        {
            Options.Add(option);
            _optionButtons.Add(new TextButton(this, new UIRectangle(0, _optionButtons.Count * _entryHeight , 1, _entryHeight), option.Name, backgroundColor)
            {
                OnClick = (e) => ChangeOption(option.Value)
            });
        }

        public void ChangeOption(int value)
        {
            var index = Options.FindIndex((option) => option.Value == value);

            for (var i = 0; i < _optionButtons.Count; i++)
            {
                _optionButtons[i].Color = i == index ? highlightColor : backgroundColor;
            }

            var selectedOption = Options[index];
            Selected = selectedOption;
            OnChange?.Invoke(selectedOption);
        }

        public void Clear()
        {
            foreach (var optionButton in _optionButtons)
            {
                optionButton.Active = false;
            }

            Options.Clear();
            _optionButtons.Clear();
        }
    }
}
