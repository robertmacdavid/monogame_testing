using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameTest2.Physics
{
    public interface ICollidable
    {
        Rectangle Hitbox { get; set; }
    }
}
