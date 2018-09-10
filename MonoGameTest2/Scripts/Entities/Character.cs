using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest2.Entities
{
    public class Character : AnimatedSprite
    {
        public float Speed;

        public Character(Texture2D texture, Vector2 spawnPosition, int rows, int columns) : base(texture, spawnPosition, rows, columns) {}


    }


    public class CharAction
    {
        // an action consists of a sprite animation, a movement modifier (?),
        // and effects on other entities
    }
}
