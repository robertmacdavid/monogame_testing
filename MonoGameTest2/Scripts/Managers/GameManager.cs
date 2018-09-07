using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Controllers;
using MonoGameTest2.GameStates;
using MonoGameTest2.Helpers;

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
        public CameraController CameraController; 
        public Camera MainCamera;

        public GameTime GameTime;
        public float DeltaTime;
        public double CurrentTimeMS;
        public KeyboardState PreviousKeyboardState;
        public MouseState PreviousMouseState;

        public SpriteFont DefaultFont;
        public bool ShowDebugInfo = true;
        private StringBuilder _debugInfo;

        public GameManager()
        {
            GameStateManager = new GameStateManager();
            _debugInfo = new StringBuilder();
        }

        public void Initialize(Game game)
        {
            Game = game;

            LevelManager = new LevelManager();
            LevelManager.BuildLevel();

            var screenWidth = Game.GraphicsDevice.Viewport.Width;
            var screenHeight = Game.GraphicsDevice.Viewport.Height;
            MainCamera = new Camera(new Rectangle(0, 0, screenWidth, screenHeight), new Vector2(screenWidth / 2, screenHeight / 2));

            CameraController = new CameraController();
            CameraController.SetDeadzoneDimensions(96, 96);
        }

        public void LoadContent(ContentManager contentManager)
        {
            ContentManager = contentManager;
            DefaultFont = contentManager.Load<SpriteFont>("default_font");

            LevelManager.LoadContent(contentManager);

            GameStateManager.AddState(new PlayState());
        }

        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CurrentTimeMS = gameTime.TotalGameTime.TotalMilliseconds;

            var keyboardState = Keyboard.GetState();
            if (keyboardState.GetKeyUp(Keys.F1))
            {
                ShowDebugInfo = !ShowDebugInfo;
            }

            if (keyboardState.GetKeyUp(Keys.F2))
            {
                if (!(GameStateManager.CurrentState is PlayState))
                {
                    GameStateManager.RemoveState();
                }
            }

            if (keyboardState.GetKeyUp(Keys.F3))
            {
                if (!(GameStateManager.CurrentState is EditorState))
                {
                    GameStateManager.AddState(new EditorState());
                }
            }

            GameStateManager.Update();
            CameraController.Update();

            PreviousKeyboardState = keyboardState;
            PreviousMouseState = Mouse.GetState();
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
            }

            GameStateManager.Draw();

            // Draw debug info
            if (ShowDebugInfo)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(DefaultFont, _debugInfo, new Vector2(0, 0), Color.Blue);
                spriteBatch.End();
            }
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
