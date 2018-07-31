using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CollisionManager;
using ObjectModel;

namespace SpaceInvadersObjects
{
    public abstract class Bullet : TwoDGameComponent, ICollidable
    {
        protected float m_HightWhenHitWall;

        public Bullet(Game i_Game)
            : base(i_Game)
        {
            m_AssetName = @"Sprites\Bullet";
        }

        public override void Initialize()
        {
            base.Initialize();
            AddTexturePixels("Bullet");
            m_Velocity = m_GameManager.BulletsVelocity;
            m_HightWhenHitWall = -1;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (m_Position.X + Width < 0 || m_Position.X > Game.GraphicsDevice.Viewport.Width || m_Position.Y + Height < 0 || m_Position.Y > Game.GraphicsDevice.Viewport.Height)
            {
                Dispose();
            }
        }

        public event EventHandler<EventArgs> CollidedEvent;

        protected virtual void OnCollided(object sender, EventArgs args)
        {
            if (CollidedEvent != null)
            {
                CollidedEvent.Invoke(sender, args);
            }
        }

        public override void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea)
        {
            if (i_IntersectArea.Count > 0)
            {
                if (i_Collidable is Barrier)
                {
                    if (m_HightWhenHitWall == -1)
                    {
                        m_HightWhenHitWall = Position.Y;
                    }
                    else if (Math.Abs(Position.Y - m_HightWhenHitWall) >= Height * .45)
                    {
                        Dispose();
                    }
                }
            }
        }
    }
}
