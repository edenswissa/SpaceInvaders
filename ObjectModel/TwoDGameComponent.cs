using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameManager;
using CollisionManager;
using ObjectModel.Animations;

namespace ObjectModel
{
    public abstract class TwoDGameComponent : Sprite
    {
        protected IGameManager m_GameManager;
        protected ICollisionsManager m_CollisionsManager;
        protected float m_SecUntilRemove;

        protected CompositeAnimator m_Animations;

        public TwoDGameComponent(Game i_Game)
            : base(i_Game)
        {
            m_GameManager = Game.Services.GetService(typeof(IGameManager)) as IGameManager;
            m_CollisionsManager = Game.Services.GetService(typeof(ICollisionsManager)) as ICollisionsManager;
        }

        public override void Initialize()
        {
            base.Initialize();
            if (this is ICollidable)
            {
                m_CollisionsManager.AddToCollidedComponents(this as ICollidable);
            }
            this.Animations = new CompositeAnimator(this);
            InitAnimations();
        }

        public override void Update(GameTime i_GameTime)
        {
            float deltaX = m_Direction.X * m_Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            float deltaY = m_Direction.Y * m_Velocity.Y * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            Position += new Vector2(deltaX, deltaY);
            base.Update(i_GameTime);
            this.Animations.Update(i_GameTime);
        }

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        public override Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        public override Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnPositionChanged();
                }
            }
        }

        public event EventHandler<EventArgs> Disposed;

        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            if (Disposed != null && !(sender as Sprite).IsClone)
            {
                Disposed.Invoke(sender, args);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            OnDisposed(this, EventArgs.Empty);

            if (Game.Components != null)
            {
                Game.Components.Remove(this);
            }
        }

        public event EventHandler<EventArgs> PositionChanged;

        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, EventArgs.Empty);
            }
        }

        public virtual List<Vector2> CheckCollision(ICollidable i_Source)
        {
            List<Vector2> intersectPoints = new List<Vector2>();
            Rectangle boundIntersectArea;
            ICollidable source = i_Source as ICollidable;
            Color sourcePixel;
            Color targetPixel;

            if (source != null)
            {
                boundIntersectArea = Rectangle.Intersect(source.Bounds, this.Bounds);
                for (int i = boundIntersectArea.Top; i < boundIntersectArea.Bottom; i++)
                {
                    for (int j = boundIntersectArea.Left; j < boundIntersectArea.Right; j++)
                    {
                        if ((source as Sprite).PixelAtXY(j, i, out sourcePixel) && this.PixelAtXY(j, i, out targetPixel))
                        {
                            if (sourcePixel.A != 0 && targetPixel.A != 0)
                            {
                                intersectPoints.Add(new Vector2(j, i));
                            }
                        }
                    }
                }
            }

            return intersectPoints;
        }

        public virtual void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea)
        {
        }

        protected virtual void InitAnimations()
        {
        }
    }
}
