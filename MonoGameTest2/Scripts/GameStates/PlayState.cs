using Microsoft.Xna.Framework;

using MonoGameTest2.Entities;
using MonoGameTest2.Managers;
using MonoGameTest2.UI;

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
            Player = new Player(_playerSpawn);
            Player.LoadContent(GameManager.ContentManager);

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

            RealTimeDebug.Append("Player Position", Player.Position);
            RealTimeDebug.Append("Animation", Player.CurrentAnimation);

            spriteBatch.Begin(transformMatrix: GameManager.MainCamera.TranslationMatrix);
            GameManager.LevelManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            GameManager.ContentManager.Unload();
        }

        public override void LoadUI() { }

        public override void UnloadUI() { }
    }
}
