using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{
    public class User
    {
        private static int s_UsersId = 0;
        private int m_Id;
        private int m_Score;
        private int m_SoulsCounter;
        private Color m_BulletsColor;
        private Color m_Color;

        public User(int i_Souls, Color i_Color)
        {
            m_Id = s_UsersId++;
            m_Score = 0;
            m_SoulsCounter = i_Souls;
            m_BulletsColor = Color.Red;
            m_Color = i_Color;
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public int ID
        {
            get { return m_Id; }
        }

        public int SoulsCounter
        {
            get { return m_SoulsCounter; }
            set { m_SoulsCounter = value; }
        }

        public Color BulletsColor
        {
            get { return m_BulletsColor; }
            set { m_BulletsColor = value; }
        }

        public Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }
    }
}
