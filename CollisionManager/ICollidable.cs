using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CollisionManager
{
    public interface ICollidable
    {
        event EventHandler<EventArgs> PositionChanged;

        event EventHandler<EventArgs> Disposed;

        List<Vector2> CheckCollision(ICollidable i_Source);

        void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea);

        Rectangle Bounds { get; }
    }
}