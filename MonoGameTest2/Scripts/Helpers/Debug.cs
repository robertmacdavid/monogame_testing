using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTest2.Managers;
using System;

namespace MonoGameTest2.Helpers
{
    public static class Debug
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color? color = null, int thickness = 1)
        {
            var realColor = color ?? Color.White;
            var origin = new Vector2(0, 0.5f);
            var rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            var scale = new Vector2(Vector2.Distance(start, end), thickness);
            spriteBatch.Draw(GameManager.PIXEL_TEXTURE, start, null, realColor, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color? color = null, int thickness = 1)
        {
            var topLeft = new Vector2(rectangle.Left, rectangle.Top);
            var topRight = new Vector2(rectangle.Right, rectangle.Top);
            var bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
            var bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);


            spriteBatch.DrawLine(topLeft, topRight, color, thickness);
            spriteBatch.DrawLine(topRight, bottomRight, color, thickness);
            spriteBatch.DrawLine(bottomRight, bottomLeft, color, thickness);
            spriteBatch.DrawLine(bottomLeft, topLeft, color, thickness);
        }
    }
}
