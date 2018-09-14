using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Entities;
using MonoGameTest2.Helpers;

namespace MonoGameTest2.GameStates
{
    public class EditorState : GameState
    {
        public override string Name => "Editor State";

        protected Camera MainCamera => GameManager.MainCamera;

        private Sprite _lastTarget;
        private Rectangle? _lastCameraBounds;

        private bool _dragging;
        private Vector2 _cameraDragStart;
        private Vector2 _mouseDragStart;
        private Vector2 _mouseTilePosition;

        private ContentManager _contentManager;
        private Texture2D _cursorTexture;
        
        public override void Initialize()
        {
            GameManager.Game.IsMouseVisible = true;
            MainCamera.Position = Vector2.Zero;

            _lastTarget = GameManager.CameraController.Target;
            _lastCameraBounds = MainCamera.CameraBounds;

            GameManager.CameraController.Target = null;
            MainCamera.CameraBounds = null;
        }

        public override void LoadContent()
        {
            _contentManager = new ContentManager(GameManager.Game.Services, GameManager.ContentManager.RootDirectory);
            _cursorTexture = _contentManager.Load<Texture2D>("level_editor/cursor");
        }

        public override void Update()
        {
            var mouseState = Mouse.GetState();

            _mouseTilePosition = GameManager.LevelManager.WorldPositionToTilePosition(MainCamera.ScreenToWorld(mouseState.Position.ToVector2()));

            if (mouseState.GetButtonDown(MouseButtons.MiddleButton))
            {
                StartDragging(mouseState);
            }

            if (_dragging)
            {
                Drag(mouseState);
            }

            if (mouseState.GetButtonUp(MouseButtons.MiddleButton))
            {
                StopDragging();
            }

            if (mouseState.GetScrollDelta() < 0)
            {
                MainCamera.Zoom /= 1.1f;
            }
            else if (mouseState.GetScrollDelta() > 0)
            {
                MainCamera.Zoom *= 1.1f;
            }
        }

        private void StartDragging(MouseState mouseState)
        {
            Mouse.SetCursor(MouseCursor.SizeAll);
            _mouseDragStart = mouseState.Position.ToVector2();
            _cameraDragStart = MainCamera.Position;
            _dragging = true;
        }

        private void Drag(MouseState mouseState)
        {
            MainCamera.Position = _cameraDragStart - ((mouseState.Position.ToVector2() - _mouseDragStart)/MainCamera.Zoom);
        }

        private void StopDragging()
        {
            Mouse.SetCursor(MouseCursor.Arrow);
            _dragging = false;
        }

        public override void Draw()
        {
            var spriteBatch = GameManager.SpriteBatch;

            if (GameManager.ShowDebugInfo)
            {
                GameManager.AppendDebug($"Scroll Wheel Value: {MainCamera.Zoom}");
                GameManager.AppendDebug($"Mouse Tile Position: {_mouseTilePosition}");
            }

            spriteBatch.Begin(transformMatrix: GameManager.MainCamera.TranslationMatrix);
            GameManager.LevelManager.Draw(spriteBatch);
            spriteBatch.Draw(_cursorTexture, _mouseTilePosition, Color.White);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            _contentManager.Unload();

            GameManager.Game.IsMouseVisible = false;
            GameManager.CameraController.Target = _lastTarget;
            MainCamera.CameraBounds = _lastCameraBounds;
            MainCamera.Zoom = 1;
        }
    }
}
