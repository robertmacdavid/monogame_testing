using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Entites;
using MonoGameTest2.Helpers;

namespace MonoGameTest2.Managers
{
    class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }

        public Game Game;
        public LevelManager LevelManager;
        public Camera Camera;
        public Player Player;

        public GameTime GameTime;
        public float DeltaTime;
        public double CurrentTimeMS;
        public KeyboardState PreviousKeyboardState;

        private SpriteFont _font;
        private bool _showDebugInfo = false;

        public void Initialize(Game game)
        {
            Game = game;
        }

        public void LoadContent(ContentManager contentManager)
        {
            var avatar = contentManager.Load<Texture2D>("images/SmileyWalk");
            _font = contentManager.Load<SpriteFont>("default_font");


            var screenWidth = Game.GraphicsDevice.Viewport.Width;
            var screenHeight = Game.GraphicsDevice.Viewport.Height;

            var playerSpawn = new Vector2
            {
                X = screenWidth / 2,
                Y = screenHeight / 2
            };

            Player = new Player(avatar, playerSpawn);
            Player.LoadContent(contentManager);

            LevelManager = new LevelManager();
            LevelManager.LoadContent(contentManager);
            LevelManager.BuildLevel();

            Camera = new Camera(new Rectangle(0, 0, screenWidth, screenHeight), new Vector2(screenWidth/2, screenHeight/2), new Rectangle(0, 0, LevelManager.ActualWidth, LevelManager.ActualHeight));
        }

        public void Update(GameTime gameTime)
        {
            if (KeyboardHelper.GetKeyUp(Keys.F1))
            {
                _showDebugInfo = !_showDebugInfo;
            }

            GameTime = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CurrentTimeMS = gameTime.TotalGameTime.TotalMilliseconds;

            Player.HandleInput();
            Player.Update();

            Camera.Move(Player.Position);

            PreviousKeyboardState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var fps = Math.Round(1 / DeltaTime);

            // TODO: Put this string somewhere else, like content.
            var debugInfo = "Debug Info\n" +
                            $"FPS: {fps}\n" +
                            $"Player Position: {Player.Position}\n" +
                            $"Player Animation: {Player._activeAnimations.Max.ToString()}\n" +
                            $"Camera Viewport: {Camera.Viewport}\n";

            // Draw game
            spriteBatch.Begin(transformMatrix: Camera.TranslationMatrix);
            LevelManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            spriteBatch.End();

            // Draw UI

            // Draw debug info
            if (_showDebugInfo)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(_font, debugInfo, new Vector2(0, 0), Color.Red);
                spriteBatch.End();
            }

        }
    }
}
