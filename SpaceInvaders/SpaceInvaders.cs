using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;
using ObjectModel;
using GameServices;
using InputManager;
using CollisionManager;
using SpaceInvadersObjects;

namespace SpaceInvaders
{
    public class SpaceInvaders : Game
    {
        private GameManager.GameManager m_GameManager;
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private List<SpaceShip> m_SpaceShips;
        private EnemiesMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;
        private SpriteBashesManager m_SpriteBashesMgr;
        private bool m_MotherShipInTheGame;
        private bool m_GameOver;

        public SpaceInvaders()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            InputManager.InputManager inputMgr = new InputManager.InputManager(this);
            m_GameManager = new GameManager.GameManager(this);
            CollisionsManager CollisionsMgr = new CollisionsManager(this);
            m_SpaceShips = new List<SpaceShip>();
            TexturePixelsManager TexturePixelsMgr = new TexturePixelsManager(this);

        }

        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
        }

        protected override void Initialize()
        {
            m_SpriteBashesMgr = new SpriteBashesManager(this);
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            m_SpriteBashesMgr.AddSpriteBash("Game", m_SpriteBatch);
            this.Window.Title = "SpaceInvaders!!";
            this.Components.Add(new BackGround(this));
            SpaceShip spaceShip = new SpaceShip1(this, 0);
            m_SpaceShips.Add(spaceShip);
            this.Components.Add(spaceShip);
            spaceShip.Disposed += spaceShip_Disposed;
            spaceShip = new SpaceShip2(this, 1);
            m_SpaceShips.Add(spaceShip);
            this.Components.Add(m_SpaceShips[1]);
            spaceShip.Disposed += spaceShip_Disposed;
            m_MotherShipInTheGame = false;
            m_GameOver = false;
            m_EnemiesMatrix = new EnemiesMatrix(this);
            m_EnemiesMatrix.Disposed += enemiesMatrix_Disposed;
            m_EnemiesMatrix.ReachedBottom += enemiesMatrix_Disposed;
            m_EnemiesMatrix.Position = m_GameManager.MatStartPosition;
            this.Components.Add(m_EnemiesMatrix);
            this.Components.Add(new ScoreBoard(this));
            this.Components.Add(new SoulsBoard(this));
            this.Components.Add(new Barriers(this));
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        public Game Game
        {
            get { return this; }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            this.Window.Title = "SpaceInvaders!!!";

            if (!m_MotherShipInTheGame)
            {
                if (GameManager.GameManager.Random.Next(1, m_GameManager.MotherShipRandomParameter) == 1)
                {
                    m_MotherShip = new MotherShip(this);
                    this.Components.Add(m_MotherShip);
                    m_MotherShip.Disposed += motherShip_Disposed;
                    m_MotherShipInTheGame = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void spaceShip_Disposed(object sender, EventArgs e)
        {
            SpaceShip ship = sender as SpaceShip;

            if(Components != null)
            {
                m_SpaceShips.Remove(ship);
                this.Components.Remove(ship);
            }
            
            if (m_SpaceShips.Count == 0)
            {
                if (!m_GameOver)
                {
                    gameOver();
                }
            }
        }

        private void enemiesMatrix_Disposed(object sender, EventArgs e)
        {
            if(!m_GameOver)
            {
                gameOver();
            } 
        }

        private void motherShip_Disposed(object sender, EventArgs e)
        {
            m_MotherShipInTheGame = false;
        }

        private void gameOver()
        {
            m_GameOver = true;
            int spaceShip1Score = m_GameManager.Score(m_GameManager.UserIds[0]);
            int spaceShip2Score = m_GameManager.Score(m_GameManager.UserIds[1]);
            string winnerMsg;

            if(spaceShip1Score > spaceShip2Score)
            {
                winnerMsg = "The Winner is SpaceShip1!!!";
            }
            else if(spaceShip1Score < spaceShip2Score)
            {
                winnerMsg = "The Winner is SpaceShip2!!!";
            }
            else
            {
                winnerMsg = "No Winner in  this game...";
            }

            string finalMsg = string.Format("GAME OVER!!!{0}SpaceShip1 score is: {1}{0}SpaceShip2 score is: {2}{0}{3}",
                                            Environment.NewLine, spaceShip1Score, spaceShip2Score, winnerMsg);
            System.Windows.Forms.MessageBox.Show(finalMsg, "GameOver", System.Windows.Forms.MessageBoxButtons.OK);
            this.Exit();
        }
    }
}
