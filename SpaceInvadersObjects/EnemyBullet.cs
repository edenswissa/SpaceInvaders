using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CollisionManager;

namespace SpaceInvadersObjects
{
    public class EnemyBullet : Bullet
    {
        public EnemyBullet(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Direction = new Vector2(0, 1f);
            m_TintColor = Color.Blue;
        }

        public override void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea)
        {
            base.Collided(i_Collidable, i_IntersectArea);
            if (i_IntersectArea.Count > 0)
            {
                if (i_Collidable is SpaceShip && !(i_Collidable as SpaceShip).Animations["Blink"].PrevFrameEnabledValue && !(i_Collidable as SpaceShip).Animations["Rotate"].PrevFrameEnabledValue)
                {
                    Dispose();
                }
            }
        }
    }
}
