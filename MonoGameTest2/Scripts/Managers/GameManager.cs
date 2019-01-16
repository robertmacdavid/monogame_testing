using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTest2.Controllers;
using MonoGameTest2.GameStates;
using MonoGameTest2.UI;
using MonoGameTest2.Desktop.Scripts.Entities;
using System;

namespace MonoGameTest2.Managers
{
    public class GameManager
    {
        public const int NATIVE_SCREEN_WIDTH = 480;
        public const int NATIVE_SCREEN_HEIGHT = 270;
        public static int ScreenWidth => NATIVE_SCREEN_WIDTH * _zoom;
        public static int ScreenHeight => NATIVE_SCREEN_HEIGHT * _zoom;

        public static Game1 Game;
        public static Texture2D PIXEL_TEXTURE;

        public static KeyboardState PreviousKeyboardState;
        public static MouseState PreviousMouseState;
        public static GamePadState PreviousGamePadState;

        private static GameManager _instance;
        public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }

        private static byte _zoom;
        public static byte Zoom { get { return _zoom; } set { SetZoom(value); } }

        private static Rectangle _actualScreenRectangle;

        public ContentManager ContentManager;
        public SpriteBatch SpriteBatch;

        public GameStateManager GameStateManager;
        public LevelManager LevelManager;
        public UIManager UIManager;

        public CameraController CameraController;
        public Camera MainCamera;

        public GameTime GameTime;
        public float DeltaTime;
        public double CurrentTimeMS;

        private RenderTarget2D _nativeRenderTarget;

        public CoroutineQueue CoroutineQueue;

        public bool ShowDebugInfo = true;
        public DebugConsole Console { get; private set; }
        public RealTimeDebug RealTimeDebug { get; private set; }

        public GameManager()
        {
            GameStateManager = new GameStateManager();
            UIManager = new UIManager();
            LevelManager = new LevelManager();
        }

        public void Initialize(Game1 game)
        {
            PIXEL_TEXTURE = new Texture2D(game.GraphicsDevice, 1, 1);
            PIXEL_TEXTURE.SetData(new[] { Color.White });

            Game = game;
            Zoom = 2;
            _nativeRenderTarget = new RenderTarget2D(Game.GraphicsDevice, NATIVE_SCREEN_WIDTH, NATIVE_SCREEN_HEIGHT);
            MainCamera = new Camera(
                new Rectangle(0, 0, NATIVE_SCREEN_WIDTH, NATIVE_SCREEN_HEIGHT), 
                new Vector2(NATIVE_SCREEN_WIDTH / 2, NATIVE_SCREEN_HEIGHT / 2)
            );
            CameraController = new CameraController();

            LevelManager.BuildLevel();
            CameraController.SetDeadzoneDimensions(96, 96);

            // TODO: Add a config file for key binds.
            InputController.RegisterInput(new ControllerInput(Keys.Z, ButtonStates.Down), InputActions.Accept);
            InputController.RegisterInput(new ControllerInput(Keys.X, ButtonStates.Down), InputActions.Cancel);
            InputController.RegisterInput(new ControllerInput(Keys.Z, ButtonStates.Down), InputActions.Talk);
            InputController.RegisterInput(new ControllerInput(Keys.A, ButtonStates.Pressed), InputActions.MoveLeft);
            InputController.RegisterInput(new ControllerInput(Keys.D, ButtonStates.Pressed), InputActions.MoveRight);
            InputController.RegisterInput(new ControllerInput(Keys.W, ButtonStates.Pressed), InputActions.MoveUp);
            InputController.RegisterInput(new ControllerInput(Keys.S, ButtonStates.Pressed), InputActions.MoveDown);
            InputController.RegisterInput(new ControllerInput(Keys.F1, ButtonStates.Down), InputActions.ToggleRealTimeDebug);
            InputController.RegisterInput(new ControllerInput(Keys.F2, ButtonStates.Down), InputActions.ToggleConsole);

            InputController.RegisterInput(new ControllerInput(Buttons.B, ButtonStates.Down), InputActions.Accept);
            InputController.RegisterInput(new ControllerInput(Buttons.A, ButtonStates.Down), InputActions.Cancel);
            InputController.RegisterInput(new ControllerInput(Buttons.LeftThumbstickLeft, ButtonStates.Pressed), InputActions.MoveLeft);
            InputController.RegisterInput(new ControllerInput(Buttons.LeftThumbstickRight, ButtonStates.Pressed), InputActions.MoveRight);
            InputController.RegisterInput(new ControllerInput(Buttons.LeftThumbstickUp, ButtonStates.Pressed), InputActions.MoveUp);
            InputController.RegisterInput(new ControllerInput(Buttons.LeftThumbstickDown, ButtonStates.Pressed), InputActions.MoveDown);

            CoroutineQueue = new CoroutineQueue();
        }

        public void LoadContent(ContentManager contentManager)
        {
            ContentManager = contentManager;

            LevelManager.LoadContent(contentManager);
            UIManager.LoadContent(contentManager);
            GameStateManager.AddState(new MainMenuState());

            RealTimeDebug = new RealTimeDebug(
                new UIDimension()
                {
                    WidthMode = UIDimensionModes.Stretch,
                    HeightMode = UIDimensionModes.Fixed,
                    Y = 49,
                    Height = 99,
                }
            )
            {
                Anchor = AnchorPoints.TopMiddle,
                Active = false
            };
            UIManager.AddElement(RealTimeDebug);

            Console = new DebugConsole(
                new UIDimension()
                {
                    WidthMode = UIDimensionModes.Stretch,
                    HeightMode = UIDimensionModes.Fixed,
                    Y = 49,
                    Height = 99,
                }
            )
            {
                Anchor = AnchorPoints.TopMiddle,
                Active = false
            };
            UIManager.AddElement(Console);
        }

        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CurrentTimeMS = gameTime.TotalGameTime.TotalMilliseconds;

            var keyboardState = Keyboard.GetState();

            CameraController.Update();
            var blockMouseUpdate = UIManager.Update();
            GameStateManager.Update(blockMouseUpdate);

            InputController.HandleInput();

            PreviousKeyboardState = keyboardState;
            PreviousMouseState = Mouse.GetState();
            PreviousGamePadState = GamePad.GetState(PlayerIndex.One);

            if (InputController.HasInput(InputActions.ToggleRealTimeDebug))
            {
                RealTimeDebug.Active = !RealTimeDebug.Active;
            }

            if (InputController.HasInput(InputActions.ToggleConsole))
            {
                Console.Active = !Console.Active;
            }

            CoroutineQueue.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;

            var fps = Math.Round(1 / DeltaTime);

            RealTimeDebug.Append("Game State", GameStateManager.CurrentState.Name);
            RealTimeDebug.Append("FPS", fps);
            RealTimeDebug.Append("Game Time", GameTime.TotalGameTime.TotalSeconds);

            Game.GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
            Game.GraphicsDevice.Clear(Color.Black);

            GameStateManager.Draw();
            UIManager.Draw(spriteBatch);

            Game.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
            spriteBatch.End();
        }

        public void UnloadContent()
        {
            ContentManager.Unload();
        }

        private static void SetZoom(byte value)
        {
            _zoom = value;
            var width = NATIVE_SCREEN_WIDTH * _zoom;
            var height = NATIVE_SCREEN_HEIGHT * _zoom;

            _actualScreenRectangle = new Rectangle(0, 0, width, height);

            Game.Graphics.PreferredBackBufferWidth = width;
            Game.Graphics.PreferredBackBufferHeight = height;
            Game.Graphics.ApplyChanges();
        }
    }
}
