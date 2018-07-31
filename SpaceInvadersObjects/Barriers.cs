using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CollisionManager;
using ObjectModel;
using ObjectModel.Animations;

namespace SpaceInvadersObjects
{
    public class Barriers : TwoDGameComponent
    {
        private class BarrierAndOffset
        {
            public Barrier m_Barrier;
            public float m_Offset;

            public BarrierAndOffset(Barrier i_Barrier, float i_Offset)
            {
                m_Barrier = i_Barrier;
                m_Offset = i_Offset;
            }
        }

        private List<BarrierAndOffset> m_Barriers;
        private Vector2 m_BarrierSize;
        private float m_Height;
        private float m_Width;
        private float m_LeftLimit;
        private float m_RightLimit;

        public Barriers(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Draw(GameTime i_GameTime)
        {
            foreach (BarrierAndOffset barrier in m_Barriers)
            {
                if (barrier != null)
                {
                    barrier.m_Barrier.Draw(i_GameTime);
                }
            }
        }

        protected override void LoadContent()
        {
        }

        public override void Initialize()
        {
            Barrier newBarrier;
            m_Barriers = new List<BarrierAndOffset>();

            for (int i = 0; i < 4; i++)
            {
                newBarrier = new Barrier(Game);
                newBarrier.Initialize();
                newBarrier.ChangeSourceRectangle(i);
                if (i == 0)
                {
                    m_BarrierSize = new Vector2(newBarrier.Width, newBarrier.Height);
                }

                m_Barriers.Add(new BarrierAndOffset(newBarrier, i * 2 * m_BarrierSize.X + m_BarrierSize.X / 2));
            }

            m_Velocity = new Vector2(60, 0);
            m_Height = m_BarrierSize.Y;
            m_Width = m_BarrierSize.X * 7;
            InitOrigins();
            m_LeftLimit = m_Position.X - m_BarrierSize.X / 2;
            m_RightLimit = m_Position.X + m_BarrierSize.X / 2;
            m_Direction = new Vector2(1, 0);
            this.Animations = new CompositeAnimator(this);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (m_Position.X <= m_LeftLimit || m_Position.X >= m_RightLimit)
            {
                m_Direction *= -1;
            }

            updateBarriersPos(i_GameTime);
        }

        protected override void InitOrigins()
        {
            PositionOrigin = new Vector2(m_Width / 2, this.Bounds.Bottom);
            this.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - (48 + Height));
        }

        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)(m_Position.X - m_Width / 2), (int)(m_Position.Y - m_Height), (int)m_Width, (int)m_Height);
            }
        }

        private void updateBarriersPos(GameTime i_GameTime)
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_Barriers[i] != null)
                {
                    m_Barriers[i].m_Barrier.Position = new Vector2(Bounds.Left + m_Barriers[i].m_Offset, Bounds.Bottom);
                    m_Barriers[i].m_Barrier.Update(i_GameTime);
                }
            }
        }
    }
}

