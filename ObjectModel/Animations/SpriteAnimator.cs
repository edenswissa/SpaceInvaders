using System;
using Microsoft.Xna.Framework;

namespace ObjectModel.Animations
{
    public abstract class SpriteAnimator
    {
        private Sprite m_BoundSprite;
        private TimeSpan m_AnimationLength;
        private TimeSpan m_TimeLeft;
        private bool m_IsFinished = false;
        private bool m_Enabled = false;
        private bool m_PrevFrameEnabledValue;
        private bool m_Initialized = false;
        private string m_Name;
        protected bool m_ResetAfterFinish = true;
        protected internal Sprite m_OriginalSpriteInfo;

        public event EventHandler Finished;

        protected SpriteAnimator(string i_Name, TimeSpan i_AnimationLength)
        {
            m_Name = i_Name;
            m_AnimationLength = i_AnimationLength;
        }

        protected internal Sprite BoundSprite
        {
            get { return m_BoundSprite; }
            set { m_BoundSprite = value; }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        public bool PrevFrameEnabledValue
        {
            get { return m_PrevFrameEnabledValue; }
        }

        public bool IsFinite
        {
            get { return m_AnimationLength != TimeSpan.Zero; }
        }

        public bool ResetAfterFinish
        {
            get { return m_ResetAfterFinish; }
            set { m_ResetAfterFinish = value; }
        }

        protected virtual void CloneOriginalSpriteInfo()
        {
            if (m_OriginalSpriteInfo == null)
            {
               m_OriginalSpriteInfo = m_BoundSprite.ShalowClone();
            }
        }

        protected virtual void Initialize()
        {
            if (!m_Initialized)
            {
                m_Initialized = true;
                CloneOriginalSpriteInfo();
                Reset();
            }
        }

        public void Reset()
        {
            Reset(m_AnimationLength);
        }

        public void Reset(TimeSpan i_AnimationLength)
        {
            if (!m_Initialized)
            {
                Initialize();
            }
            else
            {
                m_AnimationLength = i_AnimationLength;
                m_TimeLeft = i_AnimationLength;
                m_IsFinished = false;
            }

            RevertToOriginal();
        }

        protected abstract void RevertToOriginal();

        public void Pause()
        {
            this.Enabled = false;
        }

        public void Resume()
        {
            m_Enabled = true;
        }

        public virtual void Restart()
        {
            Restart(m_AnimationLength);
        }

        public virtual void Restart(TimeSpan i_AnimationLength)
        {
            Reset(i_AnimationLength);
            Resume();
        }

        protected TimeSpan AnimationLength
        {
            get { return m_AnimationLength; }
        }

        public bool IsFinished
        {
            get { return this.m_IsFinished; }
            protected set
            {
                if (value != m_IsFinished)
                {
                    m_IsFinished = value;
                    if (m_IsFinished == true)
                    {
                        OnFinished();
                    }
                }
            }
        }

        protected virtual void OnFinished()
        {
            if (m_ResetAfterFinish)
            {
                Reset();
            }

            if (Finished != null)
            {
                Finished(this, EventArgs.Empty);
            }
        }

        public void Update(GameTime i_GameTime)
        {
            if (!m_Initialized)
            {
                Initialize();
            }

            m_PrevFrameEnabledValue = m_Enabled;

            if (this.Enabled && !this.IsFinished)
            {
                if (this.IsFinite)
                {
                    m_TimeLeft -= i_GameTime.ElapsedGameTime;

                    if (m_TimeLeft.TotalSeconds < 0)
                    {
                        this.IsFinished = true;
                        m_Enabled = false;
                    }
                }
                if (this.Enabled && !this.IsFinished)
                {
                    DoFrame(i_GameTime);
                }
            }
        }

        protected abstract void DoFrame(GameTime i_GameTime);
    }
}
