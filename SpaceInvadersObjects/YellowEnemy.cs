using System;
using Microsoft.Xna.Framework;
using ObjectModel.Animations;

namespace SpaceInvadersObjects
{
    public class YellowEnemy : Enemy
    {
        public YellowEnemy(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_TintColor = Color.LightYellow;
        }

        protected override void InitAnimations()
        {
            base.InitAnimations();
            CellAnimator cellAnimator = this.Animations["CelAnimation"] as CellAnimator;
            cellAnimator.FirstCellIdx = 4;
            cellAnimator.LastCellIdx = 5;
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();
            this.SourceRectangle = new Rectangle(3 * (int)m_WidthBeforeScale, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }
    }
}