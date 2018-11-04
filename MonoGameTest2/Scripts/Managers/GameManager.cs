﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Controllers;
using MonoGameTest2.GameStates;
using MonoGameTest2.UI;

namespace MonoGameTest2.Managers
{
    public class GameManager
    {
        public const int NATIVE_SCREEN_WIDTH = 480;
        public const int NATIVE_SCREEN_HEIGHT = 270;

        public static Texture2D PIXEL_TEXTURE;

        private static GameManager _instance;
        public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } }

        public Game1 Game;
        public ContentManager ContentManager;
        public SpriteBatch SpriteBatch;

        public GameStateManager GameStateManager;
        public LevelManager LevelManager;
        public UIManager UIManager;

        public CameraController CameraController;
        public InputEventHandler MainInputEventHandler;
        public Camera MainCamera;

        public GameTime GameTime;
        public float DeltaTime;
        public double CurrentTimeMS;
        public KeyboardState PreviousKeyboardState;
        public MouseState PreviousMouseState;

        private RenderTarget2D _nativeRenderTarget;
        private Rectangle _actualScreenRectangle;
        private byte _zoom;
        public byte Zoom { get { return _zoom; } set { SetZoom(value); } }

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
            MainInputEventHandler = new InputEventHandler();

            LevelManager.BuildLevel();
            CameraController.SetDeadzoneDimensions(96, 96);
            MainInputEventHandler.AddKeyPressHandlers(
                new Keys[] { Keys.F1, Keys.F2, Keys.F3, Keys.F4 },
                new Action[] 
                {
                    () => RealTimeDebug.Active = !RealTimeDebug.Active,
                    () => Console.Active = !Console.Active,
                    EnterPlayState,
                    EnterEditorState
                }
            );
        }

        public void LoadContent(ContentManager contentManager)
        {
            ContentManager = contentManager;

            LevelManager.LoadContent(contentManager);
            UIManager.LoadContent(contentManager);

            RealTimeDebug = new RealTimeDebug(new UIRectangle(0.02f, 0.02f, 0.96f, 0.4f))
            {
                Active = false
            };
            UIManager.AddElement(RealTimeDebug);

            Console = new DebugConsole(new UIRectangle(0.58f, 0.02f, 0.4f, 0.325f))
            {
                Active = false
            };
            UIManager.AddElement(Console);

            GameStateManager.AddState(new PlayState());
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

            PreviousKeyboardState = keyboardState;
            PreviousMouseState = Mouse.GetState();
            MainInputEventHandler.HandleInput();
        }

        public void EnterPlayState()
        {
            if (!(GameStateManager.CurrentState is PlayState))
            {
                GameStateManager.RemoveState();
            }
        }
        public void EnterEditorState()
        {
            if (!(GameStateManager.CurrentState is EditorState))
            {
                GameStateManager.AddState(new EditorState());
            }
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

        private void SetZoom(byte value)
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
