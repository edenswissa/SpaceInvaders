using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameManager;
using CollisionManager;
using ObjectModel;
using ObjectModel.Animations;

namespace SpaceInvadersObjects
{
    public class EnemiesMatrix : TwoDGameComponent
    {
        private class EnemyAndOffset
        {
            public Enemy m_Enemy;
            public Vector2 m_Offset;

            public EnemyAndOffset(Enemy i_Enemy, Vector2 i_Offset)
            {
                m_Enemy = i_Enemy;
                m_Offset = i_Offset;
            }
        }

        private List<List<EnemyAndOffset>> m_Matrix;
        private int m_Rows;
        private int m_Cols;
        private int m_EnemiesCount;
        private Vector2 m_EnemySize;
        private float m_SpaceWidth;
        private float m_SpaceHeight;
        private Vector2 m_MatrixSize;
        protected float m_JumpWidth;
        protected float m_JumpHeight;
        protected TimeSpan m_SecondsBetweenJumps;
        protected TimeSpan m_TimeToNextJump;
        protected bool m_ChangedDirection;
        private bool m_SomeEnemyDisposed;
        private bool m_EnemyReachedBottom;

        public EnemiesMatrix(Game i_Game)
            : base(i_Game)
        {
        }

        public int Count
        {
            get { return m_EnemiesCount; }
        }

        public Vector2 EnemySize
        {
            get { return m_EnemySize; }
        }

        public override void Draw(GameTime i_GameTime)
        {
            EnemyAndOffset enemy;
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    enemy = m_Matrix[i][j];
                    if (enemy != null)
                    {
                        enemy.m_Enemy.Draw(i_GameTime);
                    }
                }
            }
        }

        protected override void LoadContent()
        {
        }

        public override void Initialize()
        {
            m_Position = m_GameManager.MatStartPosition;
            Enemy newEnemy = null;
            m_Matrix = new List<List<EnemyAndOffset>>();
            m_Rows = m_GameManager.MatRows;
            m_Cols = m_GameManager.MatCols;
            m_EnemiesCount = m_Rows * m_Cols;
            m_EnemySize = m_GameManager.EnemySize;
            m_SpaceHeight = m_GameManager.SpacesInEnemiesMat.Y;
            m_SpaceWidth = m_GameManager.SpacesInEnemiesMat.X;
            m_MatrixSize = m_GameManager.EnemyMatSize;
            m_SomeEnemyDisposed = false;
            m_EnemyReachedBottom = false;

            for (int i = 0; i < m_Rows; i++)
            {
                m_Matrix.Add(new List<EnemyAndOffset>());
                for (int j = 0; j < m_Cols; j++)
                {
                    if (i == 0)
                    {
                        newEnemy = new PinkEnemy(Game);
                    }
                    else if (i < 3)
                    {
                        newEnemy = new PaleBlueEnemy(Game);
                    }
                    else
                    {
                        newEnemy = new YellowEnemy(Game);
                    }

                    newEnemy.Initialize();

                    if (i == 2 || i == 4)
                    {
                        newEnemy.ChangeToTheNextSourceRectangle();
                        (newEnemy.Animations["CelAnimation"] as CellAnimator).CurrCellIdx++;
                    }

                    newEnemy.Animations["CelAnimation"].Enabled = true;
                    newEnemy.Disposed += enemy_Disposed;
                    newEnemy.ReachedBottom += enemy_ReachedBottom;
                    m_Matrix[i].Add(new EnemyAndOffset(newEnemy, Vector2.Zero));
                }
            }

            updateOffsets();

            m_Direction = m_GameManager.MatStartDirection;
            m_JumpWidth = m_GameManager.MatJumpsSize.X;
            m_JumpHeight = m_GameManager.MatJumpsSize.Y;
            m_SecondsBetweenJumps = m_TimeToNextJump = TimeSpan.FromSeconds(m_GameManager.SecBetweenJumps);
            m_ChangedDirection = false;
        }

        public override void Update(GameTime i_GameTime)
        {
            bool secondsBetweenJumpsChanged = false;

            if(m_EnemyReachedBottom)
            {
                OnReachedBottom();
            }

            if (m_SomeEnemyDisposed)
            {
                updateMatSize();
                m_SomeEnemyDisposed = false;
            }

            m_TimeToNextJump -= i_GameTime.ElapsedGameTime;
            if (m_TimeToNextJump.TotalSeconds <= 0)
            {
                if (m_ChangedDirection)
                {
                    m_Position.Y += m_JumpHeight;
                    m_ChangedDirection = false;
                }
                else
                {
                    m_TimeToNextJump += m_SecondsBetweenJumps;
                    if (m_Direction.X > 0)
                    {
                        if ((Game.GraphicsDevice.Viewport.Width - (m_MatrixSize.X + m_Position.X)) <= m_JumpWidth)
                        {
                            m_Position.X = Game.GraphicsDevice.Viewport.Width - m_MatrixSize.X;
                            m_SecondsBetweenJumps = TimeSpan.FromSeconds(m_SecondsBetweenJumps.TotalSeconds * (1 - m_GameManager.AccelerationFactor));
                            secondsBetweenJumpsChanged = true;
                            changeDirection();
                        }
                        else
                        {
                            m_Position.X += m_JumpWidth;
                        }
                    }
                    else
                    {
                        if (m_Position.X <= m_JumpWidth)
                        {
                            m_Position.X = 0;
                            changeDirection();
                        }
                        else
                        {
                            m_Position.X -= m_JumpWidth;
                        }
                    }
                }
            }

            updateEnemiesPos(i_GameTime);

            if(secondsBetweenJumpsChanged)
            {
                changeEnemiesTimeCell(m_SecondsBetweenJumps);
            }
        }

        private void enemy_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;
            EnemyAndOffset currEnemy;

            if (collidable != null)
            {
                for (int i = 0; i < m_Rows; i++)
                {
                    for (int j = 0; j < m_Cols; j++)
                    {
                        currEnemy = m_Matrix[i][j];
                        if (currEnemy != null && currEnemy.m_Enemy.Equals(collidable))
                        {
                            m_SomeEnemyDisposed = true;
                            collidable.Disposed -= enemy_Disposed;
                            m_Matrix[i][j] = null;

                            if (--m_EnemiesCount == 0)
                            {
                                this.Dispose();
                            }
                        }
                    }
                }
            }
        }

        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)m_Position.X, (int)m_Position.Y, (int)m_MatrixSize.X, (int)m_MatrixSize.Y);
            }
        }

        private void changeDirection()
        {
            m_Direction *= -1;
            m_ChangedDirection = true;
        }

        private void updateEnemiesPos(GameTime i_GameTime)
        {
            EnemyAndOffset enemyAndOffset;
            float x, y;

            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    enemyAndOffset = m_Matrix[i][j];
                    if (enemyAndOffset != null)
                    {
                        x = Position.X + enemyAndOffset.m_Offset.X;
                        y = Position.Y + enemyAndOffset.m_Offset.Y;
                        enemyAndOffset.m_Enemy.Position = new Vector2(x, y);
                        enemyAndOffset.m_Enemy.Update(i_GameTime);
                    }
                }
            }
        }

        private void changeEnemiesTimeCell(TimeSpan i_CellTime)
        {
            EnemyAndOffset enemyAndOffset;

            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    enemyAndOffset = m_Matrix[i][j];
                    if (enemyAndOffset != null)
                    {
                        (enemyAndOffset.m_Enemy.Animations["CelAnimation"] as CellAnimator).CellTime = i_CellTime;
                    }
                }
            }
        }

        private void updateMatSize()
        {
            checkIfResizeNeeded(eMatElement.BottomRow);
            checkIfResizeNeeded(eMatElement.LeftColomn);
            checkIfResizeNeeded(eMatElement.RightColomn);
            checkIfResizeNeeded(eMatElement.TopRow);
        }

        private void checkIfResizeNeeded(eMatElement i_MatElement)
        {
            bool elementIsEmpty = false;

            if (m_Matrix.Count > 0)
            {
                switch (i_MatElement)
                {
                    case eMatElement.BottomRow:
                        elementIsEmpty = matElementIsEmpty(m_Cols - 1, m_Rows - 1, true);
                        break;
                    case eMatElement.RightColomn:
                        elementIsEmpty = matElementIsEmpty(m_Rows - 1, m_Cols - 1, false);
                        break;
                    case eMatElement.TopRow:
                        elementIsEmpty = matElementIsEmpty(m_Cols - 1, 0, true);
                        break;
                    default:
                        elementIsEmpty = matElementIsEmpty(m_Rows - 1, 0, false);
                        break;
                }

                if (elementIsEmpty)
                {
                    removeElementFromMat(i_MatElement);
                    m_MatrixSize = m_GameManager.EnemyMatSize;
                    checkIfResizeNeeded(i_MatElement);
                }
            }
        }

        private bool matElementIsEmpty(int i_To, int i_NumOfRowOrCol, bool i_RowDimention)
        {
            bool foundFullCell = false;
            EnemyAndOffset currEnemy;

            for (int i = 0; i <= i_To && !foundFullCell; i++)
            {
                if (i_RowDimention)
                {
                    currEnemy = m_Matrix[i_NumOfRowOrCol][i];
                }
                else
                {
                    currEnemy = m_Matrix[i][i_NumOfRowOrCol];
                }

                if (currEnemy != null)
                {
                    foundFullCell = true;
                }
            }

            return !foundFullCell;
        }

        private void removeElementFromMat(eMatElement i_MatElement)
        {
            switch (i_MatElement)
            {
                case eMatElement.LeftColomn:
                    for (int i = 0; i < m_Rows; i++)
                    {
                        m_Matrix[i].RemoveAt(0);
                    }
                    m_Position.X += m_EnemySize.X + m_SpaceWidth;
                    m_Cols--;
                    m_GameManager.MatCols--;
                    updateOffsets();
                    break;
                case eMatElement.RightColomn:
                    for (int i = 0; i < m_Rows; i++)
                    {
                        m_Matrix[i].RemoveAt(m_Cols - 1);
                    }
                    m_Cols--;
                    m_GameManager.MatCols--;
                    break;
                case eMatElement.TopRow:
                    m_Matrix.RemoveAt(0);
                    m_Position.Y += m_EnemySize.Y + m_SpaceHeight;
                    m_Rows--;
                    m_GameManager.MatRows--;
                    updateOffsets();
                    break;
                default:
                    m_Matrix.RemoveAt(m_Rows - 1);
                    m_Rows--;
                    m_GameManager.MatRows--;
                    break;
            }
        }

        private void updateOffsets()
        {
            float x = 0;
            float y = 0;
            for (int i = 0; i < m_Rows; i++)
            {
                y = i * (m_EnemySize.Y + m_SpaceHeight) + m_EnemySize.Y / 2;
                for (int j = 0; j < m_Cols; j++)
                {
                    x = j * (m_EnemySize.X + m_SpaceWidth) + m_EnemySize.X / 2;
                    if (m_Matrix[i][j] != null)
                    {
                        m_Matrix[i][j].m_Offset = new Vector2(x, y);
                    }
                }
            }
        }

        public event EventHandler<EventArgs> ReachedBottom;

        protected virtual void OnReachedBottom()
        {
            if (ReachedBottom != null)
            {
                ReachedBottom(this, EventArgs.Empty);
            }
        }

        private void enemy_ReachedBottom(object sender, EventArgs e)
        {
            m_EnemyReachedBottom = true;
        }
    }
}
