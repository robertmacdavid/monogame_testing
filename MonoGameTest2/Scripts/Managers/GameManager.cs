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

        public LevelManager LevelManager;

        public GameTime GameTime;
        public float DeltaTime;
        public double CurrentTimeMS;
        public Player Player;
        public KeyboardState PreviousKeyboardState;

        private SpriteFont _font;
        private bool _showDebugInfo = false;

        public void LoadContent(ContentManager contentManager)
        {
            var avatar = contentManager.Load<Texture2D>("images/SmileyWalk");
            _font = contentManager.Load<SpriteFont>("default_font");

            Player = new Player(avatar, new Vector2(0, 0));
            Player.LoadContent(contentManager);

            LevelManager = new LevelManager();
            LevelManager.LoadContent(contentManager);
            LevelManager.BuildLevel();
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

            PreviousKeyboardState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var fps = Math.Round(1 / DeltaTime);

            // TODO: Put this string somewhere else, like content.
            var debugInfo = "Debug Info\n" +
                            $"FPS: {fps}\n" +
                            $"Delta Time: {DeltaTime}\n" +
                            $"Player Position: {Player.Position}\n" +
                            $"Player Animation: {Player._activeAnimations.Max.ToString()}\n";

            spriteBatch.Begin();

            LevelManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);

            if (_showDebugInfo)
            {
                spriteBatch.DrawString(_font, debugInfo, new Vector2(0, 0), Color.Red);
            }

            spriteBatch.End();
        }
    }
}
