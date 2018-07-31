using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;
using ObjectModel;
using GameServices;

namespace SpaceInvadersObjects
{
    public class ScoreBoard : DrawableGameComponent
    {
        protected IGameManager m_GameManager;
        protected SpriteFont m_ConsolasFont;
        protected SpriteBatch m_SpriteBatch;
        protected List<Vector2> m_Positions;

        public ScoreBoard(Game i_Game)
            : base(i_Game)
        {
            m_GameManager = Game.Services.GetService(typeof(IGameManager)) as IGameManager;
        }

        protected override void LoadContent()
        {
            m_ConsolasFont = Game.Content.Load<SpriteFont>(@"Fonts\Consolas");
        }

        public override void Initialize()
        {
            m_SpriteBatch = (Game.Services.GetService(typeof(ISpriteBashesManager)) as ISpriteBashesManager).GetSpriteBatch("Game");
            m_Positions = new List<Vector2>();

            foreach (int id in m_GameManager.UserIds)
            {
                m_Positions.Add(new Vector2(5, id * 15 + 5));

            }
            base.Initialize();
        }

        public override void Draw(GameTime i_GameTime)
        {
            m_SpriteBatch.Begin();
            foreach (int id in m_GameManager.UserIds)
            {
                m_SpriteBatch.DrawString(m_ConsolasFont, String.Format("P{0} Score: {1}", id + 1, m_GameManager.Score(id)), m_Positions[id], m_GameManager.UserColor(id));
            }

            m_SpriteBatch.End();
        }
    }
}

