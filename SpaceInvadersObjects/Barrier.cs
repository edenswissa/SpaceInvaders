using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CollisionManager;
using ObjectModel;

namespace SpaceInvadersObjects
{
    public class Barrier : TwoDGameComponent, ICollidable
    {
        public Barrier(Game i_Game)
            : base(i_Game)
        {
            m_AssetName = @"Sprites\Barriers";
        }

        public override void Initialize()
        {
            base.Initialize();
            AddTexturePixels("Barrier");
            m_NumOfFrames = 4;
            InitSourceRectangle();
        }

        public override void Collided(ICollidable i_Collidable, List<Vector2> i_IntersectArea)
        {
            if (i_IntersectArea.Count > 0)
            {
                if (i_Collidable is Bullet || i_Collidable is Enemy)
                {
                    TransparentPixels(i_IntersectArea);
                    if (this.SourceRectangleIsEmpty())
                    {
                        Dispose();
                    }
                }
            }
        }

        protected override void InitOrigins()
        {
            PositionOrigin = new Vector2(Width / 2, this.Bounds.Bottom);
            this.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - (48 + Height));
        }

        protected override void InitSourceRectangle()
        {
            m_WidthBeforeScale = 44;
            m_HeightBeforeScale = 32;
            m_SourceRectangle = new Rectangle(0, 0, 44, 32);
        }
    }
}
