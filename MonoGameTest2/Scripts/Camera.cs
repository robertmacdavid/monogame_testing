using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameTest2
{
    class Camera
    {
        public bool Active { get; set; }
        public Matrix TranslationMatrix { get; private set; }
        public Rectangle Viewport { get; private set; }
        public Rectangle? CameraBounds { get; private set; }

        private Vector2 _position;
        public Vector2 Position {
            get
            {
                return _position;
            }
            set
            {
                Move(value);
            }
        }

        private float _rotation;

        /// <summary>
        /// The angle of the camera in degrees.
        /// </summary>
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                Rotate(value);
            }
        }

        private float _zoom;
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                SetZoom(value);
            }
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

        private void CalculateViewport()
        {
            TranslationMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(-Rotation)) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(Viewport.Width / 2, Viewport.Height / 2, 0);
        }

        public void Move(Vector2 position)
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

        /// <summary>
        /// Set the rotation of the camera.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        public void Rotate(float angle)
        {
            if (!Active) return;

            _rotation = angle;
            CalculateViewport();
        }

        public void SetZoom(float zoom)
        {
            if (!Active) return;

            _zoom = zoom;
            CalculateViewport();
        }
    }
}
