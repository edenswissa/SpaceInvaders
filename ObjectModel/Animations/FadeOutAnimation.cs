using System;
using Microsoft.Xna.Framework;

namespace ObjectModel.Animations
{
    public class FadeOutAnimation : SpriteAnimator
    {
        private float m_FadeOutVelocity;

        public FadeOutAnimation(string i_Name, TimeSpan i_AnimationLength, float i_FadeOutVelocity)
            : base(i_Name, i_AnimationLength)
        {
            m_FadeOutVelocity = i_FadeOutVelocity;
        }

        public FadeOutAnimation(TimeSpan i_AnimationLength, float i_FadeOutVelocity)
            : this("FadeOut", i_AnimationLength, i_FadeOutVelocity)
        {
            m_FadeOutVelocity = i_FadeOutVelocity;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            int alpha = (int)this.BoundSprite.TintColor.A - (int)(i_GameTime.ElapsedGameTime.TotalSeconds * m_FadeOutVelocity);
            this.BoundSprite.TintColor = new Color(BoundSprite.TintColor, alpha);
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.TintColor = this.m_OriginalSpriteInfo.TintColor;
        }
    }
}
