using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.Entites;
using MonoGameTest2.Managers;

namespace MonoGameTest2.GameStates
{
    public class PlayState : GameState
    {
        public Player Player;
        private Vector2 _playerSpawn;

        public override void Initialize()
        {
            var screenWidth = GameManager.Game.GraphicsDevice.Viewport.Width;
            var screenHeight = GameManager.Game.GraphicsDevice.Viewport.Height;

            _playerSpawn = new Vector2
            {
                X = screenWidth / 2,
                Y = screenHeight / 2
            };
        }

        public override void LoadContent(ContentManager contentManager)
        {
            var avatar = contentManager.Load<Texture2D>("images/SmileyWalk");

            Player = new Player(avatar, _playerSpawn);
            Player.LoadContent(contentManager);

            GameManager.CameraController.Target = Player;
        }

        public override void Update()
        {
            Player.HandleInput();
            Player.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var fps = Math.Round(1 / GameManager.DeltaTime);

            // TODO: Put this string somewhere else, like content.
            var debugInfo = "Debug Info\n" +
                            $"FPS: {fps}\n" +
                            $"Player Position: {Player.Position}\n";

            spriteBatch.Begin(transformMatrix: GameManager.MainCamera.TranslationMatrix);
            GameManager.LevelManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            spriteBatch.End();

            // Draw debug info
            if (GameManager.ShowDebugInfo)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(GameManager.DefaultFont, debugInfo, new Vector2(0, 0), Color.Red);
                spriteBatch.End();
            }
        }
    }
}
