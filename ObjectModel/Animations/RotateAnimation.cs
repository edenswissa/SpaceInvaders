using System;
using Microsoft.Xna.Framework;

namespace ObjectModel.Animations
{
    public class RotateAnimation : SpriteAnimator
    {
        private float m_NumOfRotateInSecond;

        public RotateAnimation(string i_Name, TimeSpan i_AnimationLength, float i_NumOfRotateInSecond)
            : base(i_Name, i_AnimationLength)
        {
            m_NumOfRotateInSecond = i_NumOfRotateInSecond;
        }

        public RotateAnimation(TimeSpan i_AnimationLength, float i_NumOfRotateInSecond)
            : this("Rotate", i_AnimationLength, i_NumOfRotateInSecond)
        {
            m_NumOfRotateInSecond = i_NumOfRotateInSecond;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.RotationOrigin = new Vector2(this.BoundSprite.SourceRectangle.Height / 2, this.BoundSprite.SourceRectangle.Width / 2);
            this.BoundSprite.Rotation += MathHelper.TwoPi / m_NumOfRotateInSecond;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.RotationOrigin = this.m_OriginalSpriteInfo.RotationOrigin;
            this.BoundSprite.Rotation = this.m_OriginalSpriteInfo.Rotation;
        }
    }
}
