using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ObjectModel.Animations
{
    public class CompositeAnimator : SpriteAnimator
    {
        private readonly Dictionary<string, SpriteAnimator> m_AnimationsDictionary = new Dictionary<string, SpriteAnimator>();
        protected readonly List<SpriteAnimator> m_AnimationsList = new List<SpriteAnimator>();

        public CompositeAnimator(string i_Name, TimeSpan i_AnimationLength, Sprite i_BoundSprite, params SpriteAnimator[] i_Animations)
            : base(i_Name, i_AnimationLength)
        {
            this.BoundSprite = i_BoundSprite;

            foreach (SpriteAnimator animation in i_Animations)
            {
                this.Add(animation);
            }
        }

        public CompositeAnimator(Sprite I_BoundSprite)
            : this("AbimationsManager", TimeSpan.Zero, I_BoundSprite, new SpriteAnimator[] { })
        {
            this.Enabled = true;
        }

        public void Add(SpriteAnimator i_Animation)
        {
            i_Animation.BoundSprite = this.BoundSprite;
            m_AnimationsDictionary.Add(i_Animation.Name, i_Animation);
            m_AnimationsList.Add(i_Animation);
        }

        public void Remove(string i_AnimatonName)
        {
            SpriteAnimator animationToRemove;
            m_AnimationsDictionary.TryGetValue(i_AnimatonName, out animationToRemove);
            if (animationToRemove != null)
            {
                m_AnimationsDictionary.Remove(i_AnimatonName);
                m_AnimationsList.Remove(animationToRemove);
            }
        }

        public SpriteAnimator this[string i_Name]
        {
            get
            {
                SpriteAnimator returnValue = null;
                m_AnimationsDictionary.TryGetValue(i_Name, out returnValue);
                return returnValue;
            }
        }

        public override void Restart()
        {
            base.Restart();

            foreach (SpriteAnimator animation in m_AnimationsList)
            {
                animation.Restart();
            }
        }

        public override void Restart(TimeSpan i_AnimationLength)
        {
            base.Restart(i_AnimationLength);

            foreach (SpriteAnimator animation in m_AnimationsList)
            {
                animation.Restart();
            }
        }

        protected override void RevertToOriginal()
        {
            foreach (SpriteAnimator animation in m_AnimationsList)
            {
                animation.Reset();
            }
        }

        protected override void CloneOriginalSpriteInfo()
        {
            base.CloneOriginalSpriteInfo();

            foreach (SpriteAnimator animation in m_AnimationsList)
            {
                animation.m_OriginalSpriteInfo = m_OriginalSpriteInfo;
            }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            foreach (SpriteAnimator animation in m_AnimationsList)
            {
                animation.Update(i_GameTime);
            }
        }

        public bool isEmpty()
        {
            return m_AnimationsList.Count == 0;
        }
    }
}
