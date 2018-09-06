using System.Collections.Generic;
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

        public SpriteFont DefaultFont;
        public bool ShowDebugInfo = false;

        public GameManager()
        {
            GameStateManager = new GameStateManager();
        }

        public void Initialize(Game game)
        {
            Game = game;

            LevelManager = new LevelManager();
            LevelManager.BuildLevel();

            var screenWidth = Game.GraphicsDevice.Viewport.Width;
            var screenHeight = Game.GraphicsDevice.Viewport.Height;
            MainCamera = new Camera(new Rectangle(0, 0, screenWidth, screenHeight), new Vector2(screenWidth / 2, screenHeight / 2), new Rectangle(0, 0, LevelManager.ActualWidth, LevelManager.ActualHeight));

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

            if (KeyboardHelper.GetKeyUp(Keys.F1))
            {
                ShowDebugInfo = !ShowDebugInfo;
            }

            GameStateManager.Update();
            CameraController.Update();

            PreviousKeyboardState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;

            GameStateManager.Draw();
        }

        public void UnloadContent()
        {
            ContentManager.Unload();
        }
    }
}
