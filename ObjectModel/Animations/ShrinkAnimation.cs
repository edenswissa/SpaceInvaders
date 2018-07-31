using System;
using Microsoft.Xna.Framework;

namespace ObjectModel.Animations
{
    public class ShrinkAnimation : SpriteAnimator
    {
        private Vector2 m_velocityOfShrinking;

        public ShrinkAnimation(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_velocityOfShrinking = new Vector2(-0.01f);
        }

        public ShrinkAnimation(TimeSpan i_AnimationLength)
            : this("Shrink", i_AnimationLength)
        {
            m_velocityOfShrinking = new Vector2(-0.01f);
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (this.BoundSprite.Scales.X > 0 && this.BoundSprite.Scales.Y > 0)
            {
                this.BoundSprite.Scales += m_velocityOfShrinking;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = this.m_OriginalSpriteInfo.Scales;
        }
    }
}
