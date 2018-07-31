using System;
using Microsoft.Xna.Framework;
using ObjectModel.Animations;

namespace SpaceInvadersObjects
{
    public class PaleBlueEnemy : Enemy
    {
        public PaleBlueEnemy(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_TintColor = Color.CornflowerBlue;
        }

        protected override void InitAnimations()
        {
            base.InitAnimations();
            CellAnimator cellAnimator = this.Animations["CelAnimation"] as CellAnimator;
            cellAnimator.FirstCellIdx = 2;
            cellAnimator.LastCellIdx = 3;
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();
            this.SourceRectangle = new Rectangle((int)m_WidthBeforeScale, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }
    }
}