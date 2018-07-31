using System;
using Microsoft.Xna.Framework;

namespace ObjectModel.Animations
{
    public class CellAnimator : SpriteAnimator
    {
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrCellIdx;
        private int m_FirstCellIdx;
        private int m_LastCellIdx;

        // CTORs
        public CellAnimator(TimeSpan i_CellTime, TimeSpan i_AnimationLength)
            : base("CelAnimation", i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        private void goToNextFrame()
        {
            m_CurrCellIdx++;
            if (m_CurrCellIdx > m_LastCellIdx)
            {
                if (m_Loop)
                {
                    m_CurrCellIdx = m_FirstCellIdx;
                }
                else
                {
                    m_CurrCellIdx = m_LastCellIdx; /// lets stop at the last frame
                    this.IsFinished = true;
                }
            }
        }

        public int FirstCellIdx
        {
            get { return m_FirstCellIdx; }
            set { m_FirstCellIdx = value; }
        }

        public int LastCellIdx
        {
            get { return m_LastCellIdx; }
            set { m_LastCellIdx = value; }
        }

        public int CurrCellIdx
        {
            get { return m_CurrCellIdx; }
            set { m_CurrCellIdx = value; }
        }

        public TimeSpan CellTime
        {
            get { return m_CellTime; }
            set { m_CellTime = value; }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    /// we have elapsed, so change
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
                m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Top,
                this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Height);
        }
    }
}
