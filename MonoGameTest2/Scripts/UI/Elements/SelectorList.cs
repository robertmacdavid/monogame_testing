using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGameTest2.UI
{
    public class SelectorList : UIElement
    {
        private static readonly Color backgroundColor = Color.Black;
        private static readonly Color highlightColor = Color.Gray;
        private const int PixelsPerLine = 13;

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

        public SelectorList(UIElement parent, UIDimension dimensions) : base(parent, dimensions)
        {
            _optionButtons = new List<TextButton>();
            Options = new List<Option>();

            var background = new Panel(this, UIDimension.Full, backgroundColor);
            _maxEntries = AbsoluteDimensions.Height / PixelsPerLine;
        }

        public SelectorList(UIDimension bounds) : this(null, bounds) { }

        public void AddOption(Option option)
        {
            Options.Add(option);
            _optionButtons.Add(
                new TextButton(
                    this, 
                    new UIDimension()
                    {
                        WidthMode = UIDimensionModes.Stretch,
                        HeightMode = UIDimensionModes.Fixed,
                        Y = _optionButtons.Count*PixelsPerLine + PixelsPerLine/2,
                        Height = PixelsPerLine,
                    },
                    option.Name, 
                    backgroundColor
                )
                {
                    Anchor = AnchorPoints.TopMiddle,
                    OnClick = (e) => ChangeOption(option.Value)
                }
            );
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
