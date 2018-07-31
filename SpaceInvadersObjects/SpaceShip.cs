using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameManager;
using InputManager;
using CollisionManager;
using ObjectModel;
using ObjectModel.Animations;

namespace SpaceInvadersObjects
{
    public class SpaceShip : TwoDGameComponent, ICollidable
    {
        protected IInputManager m_InputManager;
        protected int m_UserId;
        protected int m_Bullets;

        public SpaceShip(Game i_Game, int i_UserId)
            : base(i_Game)
        {
            m_UserId = i_UserId;
            m_AssetName = @"Sprites\SpaceShips";
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddTexturePixels("SpaceShips");
            m_Velocity = m_GameManager.SpaceShipVelocity;
            m_Bullets = m_GameManager.SpaceShipBulletsLimit;
            m_NumOfFrames = 2;
        }

        protected override void InitSourceRectangle()
        {
            m_WidthBeforeScale = 32;
            m_HeightBeforeScale = 32;
            m_SourceRectangle = new Rectangle(0, 0, 32, 32);
        }

        protected virtual void shoot()
        {
            if (this.Animations["Blink"].Enabled != true && this.Animations["Rotate"].Enabled != true)
            {
                Bullet bullet = new SpaceShipsBullet(Game);
                Game.Components.Add(bullet);
                initBulletPos(bullet);
                m_Bullets--;
                bullet.CollidedEvent += bullet_hitted;
                bullet.Disposed += bullet_Disposed;
            }
        }

        private void SpaceShip_Finished(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void bullet_hitted(object sender, EventArgs e)
        {
            ICollidable hitted = sender as ICollidable;

            if (hitted is PinkEnemy)
            {
                m_GameManager.UpdateGameState(eGameEvent.PinkEnemyFired, m_UserId);
            }
            else if (hitted is PaleBlueEnemy)
            {
                m_GameManager.UpdateGameState(eGameEvent.PaleBlueEnemyFired, m_UserId);
            }
            else if (hitted is YellowEnemy)
            {
                m_GameManager.UpdateGameState(eGameEvent.PaleBlueEnemyFired, m_UserId);
            }
            else if (hitted is MotherShip)
            {
                m_GameManager.UpdateGameState(eGameEvent.MotherShipFired, m_UserId);
            }

            bullet_Disposed(hitted, e);
        }

        private void bullet_Disposed(object sender, EventArgs e)
        {
            if (sender is SpaceShipsBullet)
            {
                m_Bullets++;
            }
        }

        public int Bullets { get { return m_Bullets; } }

        private void initBulletPos(Bullet i_Bullet)
        {
            float x = m_Position.X - (i_Bullet.Bounds.Width / 2);
            float y = this.Bounds.Top - i_Bullet.Bounds.Height;
            i_Bullet.Position = new Vector2(x, y);
        }

        public override void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea)
        {
            if (i_IntersectArea.Count > 0)
            {
                if (i_Collidable is EnemyBullet)
                {
                    if (this.Animations["Blink"].Enabled != true && this.Animations["Rotate"].Enabled != true)
                    {
                        m_GameManager.UpdateGameState(eGameEvent.SpaceShipFiredByBullet, m_UserId);
                        if (m_GameManager.SoulsCounter(m_UserId) == 0)
                        {
                            this.Animations["Rotate"].Enabled = true;
                            this.Animations["FadeOut"].Enabled = true;
                        }
                        else
                        {
                            this.Animations["Blink"].Enabled = true;
                        }
                    }
                }
                else if (i_Collidable is Enemy)
                {
                    m_GameManager.UpdateGameState(eGameEvent.SpaceShipFiredByEnemy, m_UserId);
                    this.Dispose();
                }
            }
        }

        public int UserId { get { return m_UserId; } }

        protected override void InitAnimations()
        {
            Animations.Add(new BlinkAnimation(TimeSpan.FromSeconds(0.14), TimeSpan.FromSeconds(2.4)));
            Animations.Add(new RotateAnimation(TimeSpan.FromSeconds(2.4), 6));
            Animations.Add(new FadeOutAnimation(TimeSpan.FromSeconds(2.4), (255f / 2.4f)));
            Animations["FadeOut"].Finished += SpaceShip_Finished;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
        }
    }
}
