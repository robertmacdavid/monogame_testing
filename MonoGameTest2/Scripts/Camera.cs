using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameTest2
{
    class Camera
    {
        public Matrix TranslationMatrix { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Camera(int width, int height)
        {
            TranslationMatrix = new Matrix();
            Width = width;
            Height = height;
        }

        public void Move(Vector2 position)
        {
            TranslationMatrix = Matrix.CreateTranslation(new Vector3(-position.X + Width/2, -position.Y + Height/2, 0));
        }
    }
}
