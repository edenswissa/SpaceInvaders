using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CollisionManager;
using ObjectModel;

namespace SpaceInvadersObjects
{
    public class SpaceShipsBullet : Bullet
    {
        public SpaceShipsBullet(Game i_Game)
            : base(i_Game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            m_Direction = new Vector2(0, -1f);
            m_TintColor = Color.Red;
        }

        public override void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea)
        {
            bool dispose = false;

            base.Collided(i_Collidable, i_IntersectArea);
            if (i_IntersectArea.Count > 0)
            {
                if (i_Collidable is MotherShip || i_Collidable is Enemy)
                {
                    if (i_Collidable is MotherShip && !(i_Collidable as MotherShip).Animations["Shrink"].PrevFrameEnabledValue)
                    {
                        dispose = true;
                    }
                    else if (i_Collidable is Enemy && !(i_Collidable as Enemy).Animations["Shrink"].PrevFrameEnabledValue)
                    {
                        dispose = true;
                    }

                    if(dispose)
                    {
                        OnCollided(i_Collidable, EventArgs.Empty);
                        Dispose();
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
