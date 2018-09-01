using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGameTest2.Entites;

namespace MonoGameTest2.Managers
{
    class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }

        public float DeltaTime;
        public Player Player;

        private SpriteFont _font;

        public void LoadContent(ContentManager contentManager)
        {
            var avatar = contentManager.Load<Texture2D>("images/SmileyWalk");
            _font = contentManager.Load<SpriteFont>("default_font");

            Player = new Player(avatar, new Vector2(0, 0));
            Player.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Player.HandleInput();
            Player.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.DrawString(_font, string.Format("Debug Info\nDelta Time: {0}\nPlayer Position: {1}", DeltaTime, Player.Position), new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }
    }
}
