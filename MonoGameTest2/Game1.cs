using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTest2.Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D venus;
        private Texture2D earth;
        private Texture2D mars;
        private Texture2D avatar;
        private AnimatedSprite animatedSprite;
        private SpriteFont font;
        private int score = 0;
        private float earthAngle = 0.0f;
        // testing git

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Content is of type "ContentManager". We should make multiple
            // since unloading content is done at the granularity of CMs
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            venus = this.Content.Load<Texture2D>("images/venus");
            earth = this.Content.Load<Texture2D>("images/earth");
            mars = this.Content.Load<Texture2D>("images/mars");
            font = Content.Load<SpriteFont>("default_font");
            avatar = this.Content.Load<Texture2D>("images/SmileyWalk");
            animatedSprite = new AnimatedSprite(avatar, 4, 4);
            // unload everything that has been loaded to the CM "Content"
            // this.Content.Unload()
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            score++;
            animatedSprite.Update();
            earthAngle += 0.01f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            this.spriteBatch.Begin();

            //this.spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.Black);

            spriteBatch.Draw(venus, new Vector2(400, 240), Color.White);
            //spriteBatch.Draw(earth, new Vector2(450, 240), Color.White);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(100, 100), Color.White);

            Vector2 location = new Vector2(400, 240);
            Rectangle sourceRectangle = new Rectangle(0, 0, earth.Width, earth.Height);
            Vector2 origin = new Vector2(earth.Width/2, earth.Height/2);

            spriteBatch.Draw(earth, location, sourceRectangle, Color.White, 
                             earthAngle, origin, 1.0f, SpriteEffects.None, 1);


            this.spriteBatch.End();

            animatedSprite.Draw(spriteBatch, new Vector2(400, 200));

            base.Draw(gameTime);
        }
    }
}
