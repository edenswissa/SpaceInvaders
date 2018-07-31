using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameServices
{
    public class SpriteBashesManager : GameService, ISpriteBashesManager
    {
        protected readonly Dictionary<string, SpriteBatch> m_SpriteBashesDictionary = new Dictionary<string, SpriteBatch>();

        public SpriteBashesManager(Game i_Game) :
            base(i_Game, int.MaxValue)
        {

        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISpriteBashesManager), this);
        }

        public void AddSpriteBash(string i_Name, SpriteBatch i_SpriteBatch)
        {
            if (!m_SpriteBashesDictionary.ContainsKey(i_Name))
            {
                m_SpriteBashesDictionary.Add(i_Name, i_SpriteBatch);
            }
        }

        public SpriteBatch GetSpriteBatch(string i_Name)
        {
            return m_SpriteBashesDictionary[i_Name];
        }

        public bool ContainsName(string i_Name)
        {
            return m_SpriteBashesDictionary.ContainsKey(i_Name);
        }
    }
}
