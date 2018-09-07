using Microsoft.Xna.Framework;
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
            
        }

        public override void Update()
        {
            var mouseState = Mouse.GetState();

            if (mouseState.GetButtonDown(MouseButtons.LeftButton))
            {
                StartDragging(mouseState);
            }

            if (_dragging)
            {
                Drag(mouseState);
            }

            if (mouseState.GetButtonUp(MouseButtons.LeftButton))
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
            }

            spriteBatch.Begin(transformMatrix: GameManager.MainCamera.TranslationMatrix);
            GameManager.LevelManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void UnloadContent()
        {
            GameManager.Game.IsMouseVisible = false;
            GameManager.CameraController.Target = _lastTarget;
            MainCamera.CameraBounds = _lastCameraBounds;
            MainCamera.Zoom = 1;
        }
    }
}
