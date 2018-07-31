using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameServices;

namespace CollisionManager
{
    public class CollisionsManager : GameService, ICollisionsManager
    {
        private struct CollideableAndIntersectedArea
        {
            public ICollidable m_Collidable;
            public List<Vector2> m_IntersectedArea;

            public CollideableAndIntersectedArea(ICollidable i_Collidable, List<Vector2> i_IntersectedArea)
            {
                m_Collidable = i_Collidable;
                m_IntersectedArea = i_IntersectedArea;
            }
        }

        protected readonly List<ICollidable> m_Collidables = new List<ICollidable>();

        public CollisionsManager(Game i_Game) :
            base(i_Game, int.MaxValue)
        {
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ICollisionsManager), this);
        }

        public void AddToCollidedComponents(ICollidable i_Collidable)
        {
            if (!this.m_Collidables.Contains(i_Collidable))
            {
                this.m_Collidables.Add(i_Collidable);
                i_Collidable.PositionChanged += collidable_Changed;
                i_Collidable.Disposed += collidable_Disposed;
            }
        }

        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;

            if (collidable != null && this.m_Collidables.Contains(collidable))
            {
                collidable.PositionChanged -= collidable_Changed;
                collidable.Disposed -= collidable_Disposed;
                m_Collidables.Remove(collidable);
            }
        }

        private void collidable_Changed(object sender, EventArgs e)
        {
            if (sender is ICollidable)
            {
                checkCollision(sender as ICollidable);
            }
        }

        private void checkCollision(ICollidable i_Source)
        {
            List<CollideableAndIntersectedArea> collidedComponents = new List<CollideableAndIntersectedArea>();
            List<Vector2> intersectedArea;

            foreach (ICollidable target in m_Collidables)
            {
                if (i_Source != target)
                {
                    intersectedArea = target.CheckCollision(i_Source);
                    if (intersectedArea.Count > 0)
                    {
                        collidedComponents.Add(new CollideableAndIntersectedArea(target, intersectedArea));
                    }
                }
            }

            foreach (CollideableAndIntersectedArea targetAndArea in collidedComponents)
            {
                ICollidable target = targetAndArea.m_Collidable;
                List<Vector2> area = targetAndArea.m_IntersectedArea;

                target.Collided(i_Source, area);
                i_Source.Collided(target, area);
            }
        }
    }
}