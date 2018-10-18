using Microsoft.Xna.Framework;

using MonoGameTest2.Managers;

namespace MonoGameTest2
{
    public class Camera
    {
        public bool Active { get; set; }
        public Matrix TranslationMatrix { get; private set; }
        public Rectangle Viewport { get; private set; }
        public Rectangle? CameraBounds { get; set; }

        private Vector2 _position;
        private float _rotation;
        private float _zoom;

        public Vector2 Position {
            get { return _position; }
            set { Move(value); }
        }

        /// <summary>
        /// The angle of the camera in degrees.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { Rotate(value); }
        }

        public float Zoom
        {
            get { return _zoom; }
            set { SetZoom(value); }
        }

        public Camera(Rectangle viewport, Vector2 position, Rectangle? cameraBounds = null)
        {
            Active = true;
            Viewport = viewport;
            CameraBounds = cameraBounds;
            _position = position;
            _zoom = 1;

            CalculateViewport();
        }

        public void Reset()
        {
            Active = true;
            CameraBounds = null;
            _position = Vector2.Zero;
            _zoom = 1;

            CalculateViewport();
        }

        public Vector2 WorldToScreen(Vector2 worldPoint)
        {
            return Vector2.Transform(worldPoint, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPoint)
        {
            return Vector2.Transform(screenPoint, Matrix.Invert(TranslationMatrix));
        }

        private void CalculateViewport()
        {
            TranslationMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(-Rotation)) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(Viewport.Width / 2, Viewport.Height / 2, 0);
        }

        private void Move(Vector2 position)
        {
            if (!Active) return;

            Vector2 newPosition = position;

            if (CameraBounds.HasValue)
            {
                var newViewport = new Rectangle((int)newPosition.X - Viewport.Width / 2, (int)newPosition.Y - Viewport.Height / 2, Viewport.Width, Viewport.Height);
                var cameraBounds = CameraBounds.Value;
                if (newViewport.Left < cameraBounds.Left)
                {
                    newPosition.X = cameraBounds.Left + Viewport.Width/2;
                }
                else if (newViewport.Right > cameraBounds.Right)
                {
                    newPosition.X = cameraBounds.Right - Viewport.Width/2;
                }

                if (newViewport.Top < cameraBounds.Top)
                {
                    newPosition.Y = cameraBounds.Top + Viewport.Height/2;
                }
                else if (newViewport.Bottom > cameraBounds.Bottom)
                {
                    newPosition.Y = cameraBounds.Bottom - Viewport.Height/2;
                }
            }

            _position = newPosition;
            CalculateViewport();
        }

        private void Rotate(float angle)
        {
            if (!Active) return;

            _rotation = angle;
            CalculateViewport();
        }

        private void SetZoom(float zoom)
        {
            if (!Active) return;

            _zoom = zoom;
            CalculateViewport();
        }
    }
}
