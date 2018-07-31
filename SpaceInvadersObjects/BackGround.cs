using System;
using Microsoft.Xna.Framework;
using ObjectModel;

namespace SpaceInvadersObjects
{
    public class BackGround : TwoDGameComponent
    {
        public BackGround(Game i_Game)
            : base(i_Game)
        {
            m_AssetName = @"Sprites\BG_Space01_1024x768";
        }

        public override void Draw(GameTime i_GameTime)
        {
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(m_Texture, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), m_TintColor);
            m_SpriteBatch.End();
        }
        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
