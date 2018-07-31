using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;
using ObjectModel;

namespace SpaceInvadersObjects
{
    public class SoulsBoard : TwoDGameComponent
    {
        public SoulsBoard(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            m_AssetName = @"Sprites\SpaceShips";
            base.Initialize();
            m_NumOfFrames = 2;
            Scales = new Vector2(0.5f, 0.5f);
            Opacity = 0.5f;
        }

        public override void Draw(GameTime i_GameTime)
        {
            int row = 0;
            int offsetX, offsetY;

            m_SpriteBatch.Begin();

            foreach (int id in m_GameManager.UserIds)
            {
                offsetY = row * (int)Height * 3 / 2;
                for (int i = 0; i < m_GameManager.SoulsCounter(id); i++)
                {
                    offsetX = i * (int)Width * 3 / 2;
                    m_SpriteBatch.Draw(m_Texture, new Vector2(PositionForDraw.X - offsetX, PositionForDraw.Y + offsetY),
                            this.SourceRectangle, this.TintColor,
                                this.Rotation, this.RotationOrigin, this.Scales,
                                        SpriteEffects.None, this.LayerDepth);
                }

                row++;
                ChangeToTheNextSourceRectangle();
            }

            m_SpriteBatch.End();
        }

        protected override void InitSourceRectangle()
        {
            m_WidthBeforeScale = 32;
            m_HeightBeforeScale = 32;
            m_SourceRectangle = new Rectangle(0, 0, 32, 32);
        }

        protected override void InitOrigins()
        {
            PositionOrigin = new Vector2(this.Width, 0);
            this.Position = new Vector2(GraphicsDevice.Viewport.Width - 5, 5);
        }
    }
}

