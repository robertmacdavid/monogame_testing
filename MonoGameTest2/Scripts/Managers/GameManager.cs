using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Controllers;
using MonoGameTest2.GameStates;
using MonoGameTest2.UI;

namespace MonoGameTest2.Managers
{
    public class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }

        public Game Game;
        public ContentManager ContentManager;
        public SpriteBatch SpriteBatch;

        public GameStateManager GameStateManager;
        public LevelManager LevelManager;
        public UIManager UIManager;

        public CameraController CameraController;
        public InputEventHandler MainInputEventHandler;
        public Camera MainCamera;

        public GameTime GameTime;
        public float DeltaTime;
        public double CurrentTimeMS;
        public KeyboardState PreviousKeyboardState;
        public MouseState PreviousMouseState;

        public bool ShowDebugInfo = true;
        private StringBuilder _debugInfo;
        private Panel _debugPanel;
        private Text _debugText;

        public GameManager()
        {
            GameStateManager = new GameStateManager();
            UIManager = new UIManager();
            LevelManager = new LevelManager();

            _debugInfo = new StringBuilder();
        }

        public void Initialize(Game game)
        {
            Game = game;

            LevelManager.BuildLevel();

            var screenWidth = Game.GraphicsDevice.Viewport.Width;
            var screenHeight = Game.GraphicsDevice.Viewport.Height;
            MainCamera = new Camera(new Rectangle(0, 0, screenWidth, screenHeight), new Vector2(screenWidth / 2, screenHeight / 2));

            CameraController = new CameraController();
            CameraController.SetDeadzoneDimensions(96, 96);

            _debugPanel = new Panel(new UIRectangle(0.02f, 0.02f, 0.4f, 0.2f))
            {
                FitToContent = true,
                Padding = new Padding(0.05f)
            };

            _debugText = new Text(new UIRectangle(0, 0, 1, 1), Color.White, false);
            _debugPanel.AddChild(_debugText);
            UIManager.AddElement(_debugPanel);


            MainInputEventHandler = new InputEventHandler();

            MainInputEventHandler.AddKeyPressHandlers(new Keys[3] { Keys.F1, Keys.F2, Keys.F3 },
                                                new Action[3] { ToggleDebug, EnterPlayState, EnterEditorState });
                                                

        }

        public void LoadContent(ContentManager contentManager)
        {
            ContentManager = contentManager;

            LevelManager.LoadContent(contentManager);
            UIManager.LoadContent(contentManager);

            GameStateManager.AddState(new PlayState());
        }

        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CurrentTimeMS = gameTime.TotalGameTime.TotalMilliseconds;

            var keyboardState = Keyboard.GetState();

            GameStateManager.Update();
            CameraController.Update();

            PreviousKeyboardState = keyboardState;
            PreviousMouseState = Mouse.GetState();

            MainInputEventHandler.HandleInput();
        }

        public void ToggleDebug()
        {
            _debugPanel.Active = !_debugPanel.Active;
        }

        public void EnterPlayState()
        {
            if (!(GameStateManager.CurrentState is PlayState))
            {
                GameStateManager.RemoveState();
            }
        }
        public void EnterEditorState()
        {
            if (!(GameStateManager.CurrentState is EditorState))
            {
                GameStateManager.AddState(new EditorState());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;

            var fps = Math.Round(1 / DeltaTime);

            if (ShowDebugInfo)
            {
                _debugInfo.Clear();
                _debugInfo.AppendLine("Debug Info");
                _debugInfo.AppendLine($"Game State: {GameStateManager.CurrentState.Name}");
                _debugInfo.AppendLine($"FPS: {fps}");
                _debugInfo.AppendLine($"GameTime: {GameTime.TotalGameTime.TotalSeconds}");
            }

            GameStateManager.Draw();

            _debugText.Value = _debugInfo.ToString().Substring(0, _debugInfo.Length - 1);
            UIManager.Draw(spriteBatch);
        }

        public void UnloadContent()
        {
            ContentManager.Unload();
        }

        public void AppendDebug(string info)
        {
            if (!ShowDebugInfo)
            {
                return;
            }

            _debugInfo.AppendLine(info);
        }
    }
}
