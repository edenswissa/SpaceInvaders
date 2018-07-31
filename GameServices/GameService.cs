using System;
using Microsoft.Xna.Framework;

namespace GameServices
{
    public abstract class GameService : GameComponent
    {
        public GameService(Game i_Game, int i_UpdateOrder)
            : base(i_Game)
        {
            this.UpdateOrder = i_UpdateOrder;
            Game.Components.Add(this);
            RegisterAsService();
        }

        protected virtual void RegisterAsService()
        {
            this.Game.Services.AddService(this.GetType(), this);
        }
    }
}
