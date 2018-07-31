using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameServices
{
    public class TexturePixelsManager : GameService, ITexturePixelsManager
    {
        protected readonly Dictionary<string, Color[]> m_TexturePixelsDictionary = new Dictionary<string, Color[]>();

        public TexturePixelsManager(Game i_Game) :
            base(i_Game, int.MaxValue)
        {
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ITexturePixelsManager), this);
        }

        public void AddTexturePixels(string i_Name, Color[] i_TexturePixels)
        {
            if (!m_TexturePixelsDictionary.ContainsKey(i_Name))
            {
                m_TexturePixelsDictionary.Add(i_Name, i_TexturePixels);
            }
        }

        public Color[] GetTexturePixels(string i_Name)
        {
            return m_TexturePixelsDictionary[i_Name];
        }

        public bool ContainsName(string i_Name)
        {
            return m_TexturePixelsDictionary.ContainsKey(i_Name);
        }
    }
}
