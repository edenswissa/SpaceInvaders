using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameServices
{
    public interface ISpriteBashesManager
    {
        void AddSpriteBash(string i_Name, SpriteBatch i_SpriteBatch);
        SpriteBatch GetSpriteBatch(string i_Name);
        bool ContainsName(string i_Name);
    }
}