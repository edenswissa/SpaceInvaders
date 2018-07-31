using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CollisionManager;
using GameServices;

namespace ObjectModel
{
    public class Sprite : DrawableGameComponent
    {
        protected ITexturePixelsManager m_TexturePixelsManager;
        protected ISpriteBashesManager m_SpriteBashesManager;
        protected Color[] m_TexturePixels;

        protected SpriteBatch m_SpriteBatch;
        protected string m_AssetName;
        protected Texture2D m_Texture;
        protected Rectangle m_SourceRectangle;
        protected Vector2 m_Position;
        protected Color m_TintColor;
        protected Vector2 m_Velocity;
        protected Vector2 m_Direction;

        protected float m_WidthBeforeScale;
        protected float m_HeightBeforeScale;
        protected int m_CurrentFrameIdx;
        protected float m_SourceRectangleOffset;
        protected int m_NumOfFrames;
        public Vector2 m_PositionOrigin;
        public Vector2 m_RotationOrigin;
        protected Vector2 m_Scales;
        protected float m_LayerDepth;
        private bool m_IsClone;

        public Sprite(Game i_Game)
            : base(i_Game)
        {
            m_TexturePixelsManager = Game.Services.GetService(typeof(ITexturePixelsManager)) as ITexturePixelsManager;
            m_SpriteBashesManager = Game.Services.GetService(typeof(ISpriteBashesManager)) as ISpriteBashesManager;
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            m_SpriteBatch.Draw(m_Texture, this.PositionForDraw,
                 this.SourceRectangle, this.TintColor,
                this.Rotation, this.RotationOrigin, this.Scales,
                SpriteEffects.None, this.LayerDepth);
            m_SpriteBatch.End();
        }

        public override void Initialize()
        {
            base.Initialize();
            m_SpriteBatch = m_SpriteBashesManager.GetSpriteBatch("Game");
            m_Position = Vector2.Zero;
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            m_Velocity = Vector2.Zero;
            m_Direction = Vector2.Zero;
            m_TintColor = Color.White;
            m_CurrentFrameIdx = 0;
            m_SourceRectangleOffset = 0;
            m_NumOfFrames = 1;
            m_RotationOrigin = Vector2.Zero;
            m_Scales = Vector2.One;
            m_IsClone = false;

            InitSourceRectangle();
            InitOrigins();
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
        }

        protected void AddTexturePixels(string i_Name)
        {
            if (m_TexturePixelsManager.ContainsName(i_Name))
            {
                m_TexturePixels = m_TexturePixelsManager.GetTexturePixels(i_Name);
            }
            else
            {
                m_TexturePixels = new Color[m_Texture.Width * m_Texture.Height];
                m_Texture.GetData<Color>(m_TexturePixels);
                m_TexturePixelsManager.AddTexturePixels(i_Name, m_TexturePixels);
            }
        }

        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set { m_SpriteBatch = value; }
        }

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        public int IndexFrame { get { return m_CurrentFrameIdx; } }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        protected Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public virtual Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public virtual Vector2 Scales
        {
            get { return m_Scales; }
            set { m_Scales = value; }
        }

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        public virtual void ChangeToTheNextSourceRectangle()
        {
            if (++m_CurrentFrameIdx == m_NumOfFrames)
            {
                m_CurrentFrameIdx = 0;
            }

            ChangeSourceRectangle(m_CurrentFrameIdx);
        }

        public virtual void ChangeSourceRectangle(int i_FrameInd)
        {
            if (i_FrameInd < m_NumOfFrames)
            {
                m_CurrentFrameIdx = i_FrameInd;
                m_SourceRectangleOffset = m_CurrentFrameIdx * m_WidthBeforeScale;
                m_SourceRectangle = new Rectangle((int)m_WidthBeforeScale * i_FrameInd, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
            }
        }


        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.Width,
                    (int)this.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        public virtual bool PixelAtXY(int i_X, int i_Y, out Color i_Color)
        {
            bool res = false;
            int ind;

            i_Color = new Color();

            if (this.Bounds.Contains(i_X, i_Y))
            {
                ind = (int)((i_X - this.Bounds.Left + m_SourceRectangleOffset) + (i_Y - this.Bounds.Top) * Texture.Width);
                i_Color = m_TexturePixels[ind];
                res = true;
            }

            return res;
        }

        protected virtual void TransparentPixels(List<Vector2> i_Pixels)
        {
            int ind;

            foreach (Vector2 pixel in i_Pixels)
            {
                ind = (int)(pixel.X - Bounds.Left + m_SourceRectangleOffset + (pixel.Y - Bounds.Top) * Texture.Width);
                m_TexturePixels[ind] = new Color();
            }

            m_Texture.SetData(m_TexturePixels);
        }

        protected float m_Rotation = 0;
        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        protected virtual void InitOrigins()
        {
        }

        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        protected virtual bool SourceRectangleIsEmpty()
        {
            bool res = true;
            int ind;

            for (int i = 0; i < m_HeightBeforeScale; i++)
            {
                for (int j = 0; j < m_WidthBeforeScale; j++)
                {
                    ind = (int)((j + m_SourceRectangleOffset) + i * Texture.Width);
                    if (m_TexturePixels[ind].A != 0)
                    {
                        res = false;
                        break;
                    }
                }
            }

            return res;
        }

        public bool IsClone
        {
            get { return m_IsClone; }
            set { m_IsClone = value; }
        }

        public Sprite ShalowClone()
        {
            Sprite clone = this.MemberwiseClone() as Sprite;

            clone.IsClone = true;

            return clone;
        }
    }
}
