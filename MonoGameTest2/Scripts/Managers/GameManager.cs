using System;
using System.Collections.Generic;
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
        public const int NATIVE_SCREEN_WIDTH = 480;
        public const int NATIVE_SCREEN_HEIGHT = 270;

        //public int ScreenWidth => Game.GraphicsDevice.Viewport.Width;
        //public int ScreenHeight => Game.GraphicsDevice.Viewport.Height;

        private static GameManager _instance;
        public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }

        public Game1 Game;
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

        private RenderTarget2D _nativeRenderTarget;
        private Rectangle _actualScreenRectangle;
        private byte _zoom;
        public byte Zoom { get { return _zoom; } set { SetZoom(value); } }

        public bool ShowDebugInfo = true;
        private StringBuilder _debugInfo;
        private UIPanel _debugPanel;
        private UIText _debugText;

        public DebugConsole Console;

        public GameManager()
        {
            GameStateManager = new GameStateManager();
            UIManager = new UIManager();
            LevelManager = new LevelManager();

            _debugInfo = new StringBuilder();
        }

        public void Initialize(Game1 game)
        {
            Game = game;

            _nativeRenderTarget = new RenderTarget2D(Game.GraphicsDevice, NATIVE_SCREEN_WIDTH, NATIVE_SCREEN_HEIGHT);
            Zoom = 2;

            LevelManager.BuildLevel();
            MainCamera = new Camera(new Rectangle(0, 0, NATIVE_SCREEN_WIDTH, NATIVE_SCREEN_HEIGHT), new Vector2(NATIVE_SCREEN_WIDTH / 2, NATIVE_SCREEN_HEIGHT / 2));

            CameraController = new CameraController();
            CameraController.SetDeadzoneDimensions(96, 96);

            MainInputEventHandler = new InputEventHandler();

            MainInputEventHandler.AddKeyPressHandlers(new Keys[3] { Keys.F1, Keys.F2, Keys.F3 },
                                                new Action[3] { ToggleDebug, EnterPlayState, EnterEditorState });
        }

        public void LoadContent(ContentManager contentManager)
        {
            ContentManager = contentManager;

            LevelManager.LoadContent(contentManager);
            UIManager.LoadContent(contentManager);

            _debugPanel = new UIPanel(new UIRectangle(0.02f, 0.02f, 0.4f, 0.3f))
            {
                FitToContent = true,
                Padding = new Padding(0.05f),
            };

            _debugText = new UIText(_debugPanel, new UIRectangle(0, 0, 1, 1), Color.White, false);
            UIManager.AddElement(_debugPanel);

            Console = new DebugConsole(new UIRectangle(0.58f, 0.02f, 0.4f, 0.325f));
            UIManager.AddElement(Console);

            GameStateManager.AddState(new PlayState());
        }

        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CurrentTimeMS = gameTime.TotalGameTime.TotalMilliseconds;

            var keyboardState = Keyboard.GetState();

            CameraController.Update();

            var blockMouseUpdate = UIManager.Update();
            GameStateManager.Update(blockMouseUpdate);
      

            PreviousKeyboardState = keyboardState;
            PreviousMouseState = Mouse.GetState();

            MainInputEventHandler.HandleInput();
        }

        public void ToggleDebug()
        {
            _debugPanel.Active = !_debugPanel.Active;
            Console.Active = !Console.Active;
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

            Game.GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
            Game.GraphicsDevice.Clear(Color.Black);

            GameStateManager.Draw();
            _debugText.Value = _debugInfo.ToString().Substring(0, _debugInfo.Length - 1);
            UIManager.Draw(spriteBatch);

            Game.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
            spriteBatch.End();
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

        private void SetZoom(byte value)
        {
            _zoom = value;
            var width = NATIVE_SCREEN_WIDTH * _zoom;
            var height = NATIVE_SCREEN_HEIGHT * _zoom;

            _actualScreenRectangle = new Rectangle(0, 0, width, height);

            Game.Graphics.PreferredBackBufferWidth = width;
            Game.Graphics.PreferredBackBufferHeight = height;
            Game.Graphics.ApplyChanges();
        }
    }
}
