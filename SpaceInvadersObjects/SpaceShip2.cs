using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using InputManager;
using ObjectModel;

namespace SpaceInvadersObjects
{
    public class SpaceShip2 : SpaceShip
    {
        public SpaceShip2(Game i_Game, int i_UserId)
            : base(i_Game, i_UserId)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            ChangeToTheNextSourceRectangle();
        }

        public override void Update(GameTime i_GameTime)
        {
            float x;

            if (m_InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            if (m_InputManager.KeyPressed(Keys.D) || m_InputManager.KeyHeld(Keys.D))
            {
                x = m_Position.X - (m_Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
                Position = new Vector2(x, m_Position.Y);
            }
            else if (m_InputManager.KeyPressed(Keys.G) || m_InputManager.KeyHeld(Keys.G))
            {
                x = m_Position.X + (m_Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
                Position = new Vector2(x, m_Position.Y);
            }

            if (m_InputManager.KeyPressed(Keys.R) && m_Bullets > 0)
            {
                shoot();
            }

            x = MathHelper.Clamp(m_Position.X, PositionOrigin.X, Game.GraphicsDevice.Viewport.Width - PositionOrigin.X);
            Position = new Vector2(x, m_Position.Y);
            base.Update(i_GameTime);
        }

        protected override void InitOrigins()
        {
            PositionOrigin = new Vector2(Width / 2, this.Bounds.Bottom);
            this.Position = new Vector2(this.Width / 2, GraphicsDevice.Viewport.Height - Height / 2);
        }
    }
}
