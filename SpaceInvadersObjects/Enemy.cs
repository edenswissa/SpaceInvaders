using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;
using GameServices;
using CollisionManager;
using ObjectModel;
using ObjectModel.Animations;

namespace SpaceInvadersObjects
{
    public abstract class Enemy : TwoDGameComponent, ICollidable
    {
        protected Vector2 m_ShootingProbability;
        private int m_Bullets;

        public Enemy(Game i_Game)
            : base(i_Game)
        {
            m_AssetName = @"Sprites\Enemies";
        }

        public override void Initialize()
        {
            base.Initialize();
            AddTexturePixels("Enemies");
            m_Bullets = 1;
            m_SecUntilRemove = 2;
            m_NumOfFrames = 6;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!Animations["Shrink"].Enabled && GameManager.GameManager.Random.Next(1, m_GameManager.ShootingRandomParameter) == 1 && m_Bullets > 0)
            {
                shoot();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void InitSourceRectangle()
        {
            m_WidthBeforeScale = 32;
            m_HeightBeforeScale = 32;
        }

        private void shoot()
        {
            Bullet bullet = new EnemyBullet(Game);
            m_Bullets--;
            Game.Components.Add(bullet);
            initBulletPos(bullet);
            bullet.Disposed += bullet_Disposed;
        }

        private void bullet_Disposed(object sender, EventArgs e)
        {
            m_Bullets++;
        }

        private void initBulletPos(Bullet i_Bullet)
        {
            float x = m_Position.X + (this.Bounds.Width / 2) - (i_Bullet.Bounds.Width / 2);
            float y = m_Position.Y + i_Bullet.Bounds.Height;
            i_Bullet.Position = new Vector2(x, y);
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();
            if (this.Position.Y + this.Height > Game.GraphicsDevice.Viewport.Height)
            {
                OnReachedBottom();
            }
        }

        public override void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea)
        {
            if (i_IntersectArea.Count > 0)
            {
                if (i_Collidable is SpaceShipsBullet)
                {
                    Animations["Shrink"].Enabled = true;
                    Animations["Rotate"].Enabled = true;
                }
            }
        }

        protected override void InitOrigins()
        {
            PositionOrigin = new Vector2(Width / 2, Height / 2);
        }

        protected override void InitAnimations()
        {
            Animations.Add(new CellAnimator(TimeSpan.FromSeconds(m_GameManager.SecBetweenJumps),TimeSpan.Zero));
            Animations.Add(new ShrinkAnimation(TimeSpan.FromSeconds(1.6)));
            Animations.Add(new RotateAnimation(TimeSpan.FromSeconds(1.6), 6));
            Animations["Shrink"].Finished += enemy_Shrinked;
        }

        private void enemy_Shrinked(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public event EventHandler<EventArgs> ReachedBottom;

        protected virtual void OnReachedBottom()
        {
            if (ReachedBottom != null)
            {
                ReachedBottom(this, EventArgs.Empty);
            }
        }
    }
}
