using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using InputManager;
using ObjectModel;

namespace SpaceInvadersObjects
{
    public class SpaceShip1 : SpaceShip
    {
        public SpaceShip1(Game i_Game, int i_UserId)
            : base(i_Game, i_UserId)
        {

        }

        public override void Update(GameTime i_GameTime)
        {
            float x;

            if (m_InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            x = m_Position.X + m_InputManager.MousePositionDelta.X;
            Position = new Vector2(x, m_Position.Y);

            if (m_InputManager.KeyPressed(Keys.Left) || m_InputManager.KeyHeld(Keys.Left))
            {
                x = m_Position.X - (m_Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
                Position = new Vector2(x, m_Position.Y);
            }
            else if (m_InputManager.KeyPressed(Keys.Right) || m_InputManager.KeyHeld(Keys.Right))
            {
                x = m_Position.X + (m_Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
                Position = new Vector2(x, m_Position.Y);
            }

            if ((m_InputManager.MouseLeftBtnPressed() || m_InputManager.KeyPressed(Keys.Up)) && m_Bullets > 0)
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
            this.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - Height / 2);
        }
    }
}
