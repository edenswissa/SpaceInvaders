using System;
using Microsoft.Xna.Framework;

namespace GameServices
{
    public interface ITexturePixelsManager
    {
        void AddTexturePixels(string i_Name, Color[] i_TexturePixels);
        bool ContainsName(string i_Name);
        Color[] GetTexturePixels(string i_Name);
    }
}