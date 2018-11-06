using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameTest2.Entities;
using MonoGameTest2.Helpers;
using MonoGameTest2.Levels;
using MonoGameTest2.Managers;
using MonoGameTest2.UI;

namespace MonoGameTest2.GameStates
{
    public class EditorState : GameState
    {
        private struct EditorCursor
        {
            public Vector2 MousePosition;
            public Vector2 TilePosition;
        }

        public override string Name => "Editor State";

        protected Camera MainCamera => GameManager.MainCamera;
        protected LevelManager LevelManager => GameManager.LevelManager;

        private Sprite _lastTarget;
        private Rectangle? _lastCameraBounds;

        private bool _dragging;
        private Vector2 _cameraDragStart;
        private Vector2 _mouseDragStart;
        private EditorCursor _editorCursor;

        private ContentManager _contentManager;
        private Texture2D _cursorTexture;

        private TileTypes _currentTile;
        private uint _currentTexture;
        
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
            var panel = new UIPanel(new UIRectangle(0, 0.8f, 1, 0.2f));
            UIManager.AddElement(panel);

            _contentManager = new ContentManager(GameManager.Game.Services, GameManager.ContentManager.RootDirectory);
            _cursorTexture = _contentManager.Load<Texture2D>("level_editor/cursor");
        }

        public override void Update(bool blockMouseUpdates)
        {
            var mouseState = Mouse.GetState();

            _editorCursor.MousePosition = LevelManager.WorldPositionToTilePosition(MainCamera.ScreenToWorld(mouseState.GetPosition()));
            _editorCursor.TilePosition = new Vector2(_editorCursor.MousePosition.X / LevelManager.TileSize.X, _editorCursor.MousePosition.Y / LevelManager.TileSize.Y);

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

            if (mouseState.GetButtonUp(MouseButtons.RightButton))
            {
                _currentTexture++;
                
                if (_currentTexture >= LevelManager.TileCounts[(int)_currentTile]) {
                    _currentTile = (TileTypes)((int)(_currentTile + 1) % (int)TileTypes.TileTypeCount);
                    _currentTexture = 0;
                }                
            }

            if (!blockMouseUpdates && mouseState.GetButtonPressed(MouseButtons.LeftButton))
            {
                var x = (uint)_editorCursor.TilePosition.X;
                var y = (uint)_editorCursor.TilePosition.Y;
                LevelManager.Level.SetTile(x, y, new Tile(x, y, _currentTile, _currentTexture));
            }
        }

        private void StartDragging(MouseState mouseState)
        {
            Mouse.SetCursor(MouseCursor.SizeAll);
            _mouseDragStart = mouseState.GetPosition();
            _cameraDragStart = MainCamera.Position;
            _dragging = true;
        }

        private void Drag(MouseState mouseState)
        {
            MainCamera.Position = _cameraDragStart - ((mouseState.GetPosition() - _mouseDragStart)/MainCamera.Zoom);
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
                GameManager.RealTimeDebug.Append("Scroll Wheel Value", MainCamera.Zoom);
                GameManager.RealTimeDebug.Append("Tile Position", _editorCursor.TilePosition);
            }

            spriteBatch.Begin(transformMatrix: GameManager.MainCamera.TranslationMatrix);
            GameManager.LevelManager.Draw(spriteBatch);
            spriteBatch.Draw(_cursorTexture, _editorCursor.MousePosition, Color.White);
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
