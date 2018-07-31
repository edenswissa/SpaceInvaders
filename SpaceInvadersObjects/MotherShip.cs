using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CollisionManager;
using ObjectModel;
using ObjectModel.Animations;

namespace SpaceInvadersObjects
{
    public class MotherShip : TwoDGameComponent, ICollidable
    {
        public MotherShip(Game i_Game)
            : base(i_Game)
        {
            m_AssetName = @"Sprites\MotherShip_32x120";
        }

        public override void Initialize()
        {
            base.Initialize();
            AddTexturePixels("MotherShip");
            float x = Game.GraphicsDevice.Viewport.Width - m_Texture.Width;
            float y = m_Texture.Height;
            m_Position = new Vector2(x, y);
            m_Velocity = m_GameManager.MotherShipVelocity;
            m_Direction = m_GameManager.MotherShipStartDirection;
            m_TintColor = Color.Red;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (m_Position.X + m_Texture.Width < 0)
            {
                Dispose();
            }
        }

        public override void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea)
        {
            if (i_IntersectArea.Count > 0)
            {
                if (i_Collidable is SpaceShipsBullet && !Animations["Shrink"].Enabled)
                {
                    Animations["Shrink"].Enabled = true;
                    Animations["FadeOut"].Enabled = true;
                    Animations["Blink"].Enabled = true;
                }
            }
        }

        protected override void InitAnimations()
        {
            Animations.Add(new ShrinkAnimation(TimeSpan.FromSeconds(2.4)));
            Animations.Add(new FadeOutAnimation(TimeSpan.FromSeconds(2.4), (255f / 2.4f)));
            Animations.Add(new BlinkAnimation(TimeSpan.FromSeconds(0.14), TimeSpan.FromSeconds(2.4)));
            Animations["Shrink"].Finished += motherShip_Shrinked;
        }

        private void motherShip_Shrinked(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
