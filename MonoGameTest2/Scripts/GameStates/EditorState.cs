using Microsoft.Xna.Framework.Input;


namespace MonoGameTest2.GameStates
{
    public class EditorState : GameState
    {
        public override string Name => "Editor State";

        public override void Initialize()
        {
            GameManager.Game.IsMouseVisible = true;
        }

        public override void LoadContent()
        {
            
        }

        public override void Update()
        {

        }

        public override void Draw()
        {
            var spriteBatch = GameManager.SpriteBatch;

            spriteBatch.Begin(transformMatrix: GameManager.MainCamera.TranslationMatrix);
            GameManager.LevelManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            GameManager.Game.IsMouseVisible = false;
        }
    }
}
