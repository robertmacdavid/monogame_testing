using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.Managers;
using MonoGameTest2.UI;

namespace MonoGameTest2.GameStates
{
    class MainMenuState : GameState
    {
        public override string Name => "Editor State";

        protected Camera MainCamera => GameManager.MainCamera;
        protected LevelManager LevelManager => GameManager.LevelManager;

        private Panel _background;
        private Texture2D _logoTexture;
        private Image _logo;
        private TextButton _startButton;
        private TextButton _quitButton;

        public override void Initialize()
        {
            _logoTexture = GameManager.ContentManager.Load<Texture2D>("logo");
            GameManager.Game.IsMouseVisible = true;
        }

        public override void LoadUI()
        {
            _background = new Panel(
                UIDimension.Full,
                Color.White
            );

            _logo = new Image(
                _background,
                new UIDimension()
                {
                    WidthMode = UIDimensionModes.Fixed,
                    Width = 200,
                    HeightMode = UIDimensionModes.Fixed,
                    Height = 50,
                    Y = -75
                },
                _logoTexture,
                AnchorPoints.Middle
            );

            _startButton = new TextButton(
                _background,
                new UIDimension()
                {
                    WidthMode = UIDimensionModes.Fixed,
                    Width = 25,
                    HeightMode = UIDimensionModes.Fixed,
                    Height = 13,
                },
                "Start",
                new Color(0),
                textColor: Color.Black
            )
            {
                Anchor = AnchorPoints.Middle,
                OnMouseOver = (e) => { _startButton.Text.Color = Color.Gray; },
                OnMouseOut = (e) => { _startButton.Text.Color = Color.Black; },
                OnClick = (e) => { EnterPlayState(); }
            };

            _quitButton = new TextButton(
                _background,
                new UIDimension()
                {
                    WidthMode = UIDimensionModes.Fixed,
                    Width = 25,
                    HeightMode = UIDimensionModes.Fixed,
                    Y = 26,
                    Height = 13,
                },
                "Quit",
                new Color(0),
                textColor: Color.Black
            )
            {
                Anchor = AnchorPoints.Middle,
                OnMouseOver = (e) => { _quitButton.Text.Color = Color.Gray; },
                OnMouseOut = (e) => { _quitButton.Text.Color = Color.Black; },
                OnClick = (e) => { GameManager.Game.Exit(); }
            };

            UIManager.AddElement(_background);
        }

        public void EnterPlayState()
        {
            GameManager.GameStateManager.RemoveState();
            GameManager.GameStateManager.AddState(new PlayState());
        }

        public void EnterEditorState()
        {
            GameManager.GameStateManager.RemoveState();
            GameManager.GameStateManager.AddState(new EditorState());
        }

        public override void Update(bool blockMouseUpdates) { } 

        public override void Draw() { }

        public override void UnloadContent()
        {
            _logoTexture.Dispose();
            base.UnloadContent();
        }

        public override void UnloadUI()
        {
            UIManager.RemoveElement(_background);
        }
    }
}
