using System;
using Microsoft.Xna.Framework;
using ObjectModel.Animations;

namespace SpaceInvadersObjects
{
    public class PinkEnemy : Enemy
    {
        public PinkEnemy(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_TintColor = Color.Pink;
        }

        protected override void InitAnimations()
        {
            base.InitAnimations();
            CellAnimator cellAnimator = this.Animations["CelAnimation"] as CellAnimator;
            cellAnimator.FirstCellIdx = 0;
            cellAnimator.LastCellIdx = 1;
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();
            this.SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }
    }
}
