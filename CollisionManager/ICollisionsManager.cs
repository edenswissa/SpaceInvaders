using System;

namespace CollisionManager
{
    public interface ICollisionsManager
    {
        void AddToCollidedComponents(ICollidable i_Collidable);
    }
}