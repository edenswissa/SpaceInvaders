using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameServices;

namespace GameManager
{
    public enum eGameEvent
    {
        SpaceShipFiredByBullet,
        SpaceShipFiredByEnemy,
        MotherShipFired,
        PinkEnemyFired,
        PaleBlueEnemyFired,
        YellowEnemyFired,
    }

    public enum eMatElement
    {
        LeftColomn,
        RightColomn,
        TopRow,
        BottomRow
    }

    public class GameManager : GameService, IGameManager
    {
        private static Random s_Random = new Random();
        private static bool s_Initialized = false;
        private GameSettings m_Settings;
        private Dictionary<int, User> m_Users;
        private int m_MatRows;
        private int m_MatCols;
        private float m_SecondsBetweenJumps;

        public GameManager(Game i_Game)
            : base(i_Game, 1)
        {
            Initialize();
        }

        public static Random Random
        {
            get { return s_Random; }
        }

        public override void Initialize()
        {
            if (!s_Initialized)
            {
                User newUser;
                m_Settings = new GameSettings();
                int numOfSouls = m_Settings.SoulsLimit;
                m_Users = new Dictionary<int, User>();
                newUser = new User(numOfSouls, Color.Blue);
                m_Users.Add(newUser.ID, newUser);
                newUser = new User(numOfSouls, Color.Green);
                m_Users.Add(newUser.ID, newUser);
                m_MatRows = (int)m_Settings.MatDimentionsLengths.Y;
                m_MatCols = (int)m_Settings.MatDimentionsLengths.X;
                m_SecondsBetweenJumps = m_Settings.SecondsBetweenJumps;
                s_Initialized = true;
            }
        }

        public override void Update(GameTime i_GameTime)
        {
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(IGameManager), this);
        }

        public List<int> UserIds
        {
            get
            {
                List<int> keys = new List<int>();
                foreach (int key in m_Users.Keys)
                {
                    keys.Add(key);
                }

                return keys;
            }
        }

        public int SpaceShipBulletsLimit
        {
            get { return m_Settings.SpaceShipsBulletsLimit; }
        }

        public int Score(int i_UserId)
        {
            return m_Users[i_UserId].Score;
        }

        public int SoulsCounter(int i_UserId)
        {
            return m_Users[i_UserId].SoulsCounter;
        }

        public Color UserColor(int i_UserId)
        {
            return m_Users[i_UserId].Color;
        }

        public void UpdateGameState(eGameEvent i_Event, int i_UserId)
        {
            switch (i_Event)
            {
                case eGameEvent.SpaceShipFiredByBullet:
                    m_Users[i_UserId].Score -= m_Settings.ObjectsValues.SpaceShip;
                    m_Users[i_UserId].SoulsCounter--;
                    break;
                case eGameEvent.MotherShipFired:
                    m_Users[i_UserId].Score += m_Settings.ObjectsValues.MotherShip;
                    break;
                case eGameEvent.PinkEnemyFired:
                    m_Users[i_UserId].Score += m_Settings.ObjectsValues.PinkEnemy;
                    break;
                case eGameEvent.PaleBlueEnemyFired:
                    m_Users[i_UserId].Score += m_Settings.ObjectsValues.PaleBlueEnemy;
                    break;
                case eGameEvent.YellowEnemyFired:
                    m_Users[i_UserId].Score += m_Settings.ObjectsValues.YellowEnemy;
                    break;
                case eGameEvent.SpaceShipFiredByEnemy:
                    m_Users[i_UserId].SoulsCounter = 0;
                    break;
            }

            if (m_Users[i_UserId].Score < 0)
            {
                m_Users[i_UserId].Score = 0;
            }
        }

        public Vector2 EnemySize
        {
            get { return m_Settings.EnemySize; }
        }

        public Vector2 SpacesInEnemiesMat
        {
            get { return m_Settings.SpacesInEnemiesMat; }
        }

        public int MatRows
        {
            get { return m_MatRows; }
            set { m_MatRows = value; }
        }

        public int MatCols
        {
            get { return m_MatCols; }
            set { m_MatCols = value; }
        }

        public Vector2 EnemyMatSize
        {
            get
            {
                float width = (m_MatCols * EnemySize.X) + ((m_MatCols - 1) * SpacesInEnemiesMat.X);
                float height = (m_MatRows * EnemySize.Y) + ((m_MatRows - 1) * SpacesInEnemiesMat.Y);
                return new Vector2(width, height);
            }
        }

        public Vector2 MatStartPosition
        {
            get { return m_Settings.MatStartPosition; }
        }

        public Vector2 MatStartDirection
        {
            get { return m_Settings.MatStartDirection; }
        }

        public Vector2 MatJumpsSize
        {
            get { return new Vector2(m_Settings.MatJumpWidth, m_Settings.MatJumpHeight); }
        }

        public float SecBetweenJumps
        {
            get { return m_SecondsBetweenJumps; }
            set { m_SecondsBetweenJumps = value; }
        }

        public float AccelerationFactor
        {
            get { return m_Settings.AccelerationFactor; }
        }

        public Color BulletsColor(int i_UserId)
        {
            return m_Users[i_UserId].BulletsColor;
        }

        public int MotherShipRandomParameter
        {
            get { return m_Settings.MotherShipRandomParameter; }
        }

        public int ShootingRandomParameter
        {
            get { return m_Settings.ShootingRandomParameter; }
        }

        public Vector2 MotherShipStartDirection
        {
            get { return m_Settings.MotherShipStartDirection; }
        }

        public Vector2 MotherShipVelocity
        {
            get { return m_Settings.MotherShipVelocity; }
        }

        public Vector2 SpaceShipVelocity
        {
            get { return m_Settings.SpaceShipVelocity; }
        }

        public Vector2 BulletsVelocity
        {
            get { return m_Settings.BulletsVelocity; }
        }
    }
}