using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Entities;
using MonoGameTest2.Managers;

namespace MonoGameTest2.GameStates
{
    public class PlayState : GameState
    {
        public Player Player;
        private Vector2 _playerSpawn;

        public override string Name => "Play State";

        public override void Initialize()
        {
            GameManager.Game.IsMouseVisible = false;

            _playerSpawn = new Vector2
            {
                X = GameManager.NATIVE_SCREEN_WIDTH / 2,
                Y = GameManager.NATIVE_SCREEN_HEIGHT / 2
            };

            GameManager.MainCamera.CameraBounds = new Rectangle(0, 0, GameManager.LevelManager.ActualWidth, GameManager.LevelManager.ActualHeight);
        }

        public override void LoadContent()
        {
            var contentManager = GameManager.ContentManager;
            var avatar = contentManager.Load<Texture2D>("images/SmileyWalk");

            Player = new Player(avatar, _playerSpawn);
            Player.LoadContent(contentManager);

            GameManager.CameraController.Target = Player;
        }

        public override void Update(bool blockMouseUpdates)
        {
            Player.HandleInput();
            Player.Update();
        }

        public override void Draw()
        {
            var spriteBatch = GameManager.SpriteBatch;

            GameManager.AppendDebug($"Player Position: {Player.Position}");

            spriteBatch.Begin(transformMatrix: GameManager.MainCamera.TranslationMatrix);
            GameManager.LevelManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            GameManager.ContentManager.Unload();
        }
    }
}
