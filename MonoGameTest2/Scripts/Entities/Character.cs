using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.Entities
{
    public class Character : AnimatedSprite
    {
        public float Speed;
        protected Vector2 TravelDirection;
        protected Vector2 FacingDirection;

        public Character(Texture2D texture, Vector2 spawnPosition, int rows, int columns) : base(texture, spawnPosition, rows, columns) { }

        public void NormalizeTravelDir()
        {
            if (TravelDirection.LengthSquared() - 1.0 > float.Epsilon)
                TravelDirection.Normalize();
        }
    }


    public class CharAction
    {
        public int AnimationID;
        // an action consists of a sprite animation, a movement modifier (?),
        // and effects on other entities
    }
}
