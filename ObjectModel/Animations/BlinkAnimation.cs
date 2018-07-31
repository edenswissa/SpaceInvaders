using System;
using Microsoft.Xna.Framework;

namespace ObjectModel.Animations
{
    public class BlinkAnimation : SpriteAnimator
    {
        private TimeSpan m_BlinkLength;
        private TimeSpan m_TimeLeftForNextBlink;

        public BlinkAnimation(string i_Name, TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.m_BlinkLength = i_BlinkLength;
            this.m_TimeLeftForNextBlink = i_BlinkLength;
        }

        public BlinkAnimation(TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
            : this("Blink", i_BlinkLength, i_AnimationLength)
        {
            this.m_BlinkLength = i_BlinkLength;
            this.m_TimeLeftForNextBlink = i_BlinkLength;
        }

        public TimeSpan BlinkLength
        {
            get { return m_BlinkLength; }
            set { m_BlinkLength = value; }
        }

        public TimeSpan TimeLeftForNextBlink
        {
            get { return m_TimeLeftForNextBlink; }
            set { m_TimeLeftForNextBlink = value; }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextBlink -= i_GameTime.ElapsedGameTime;

            if (m_TimeLeftForNextBlink.TotalSeconds < 0)
            {
                this.BoundSprite.Visible = !this.BoundSprite.Visible;
                m_TimeLeftForNextBlink = m_BlinkLength;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
