using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameManager
{
    public interface IGameManager
    {
        int Score(int i_UserId);

        int SpaceShipBulletsLimit { get; }

        int SoulsCounter(int i_UserId);

        Color UserColor(int i_UserId);

        List<int> UserIds { get; }

        Vector2 EnemySize { get; }

        Vector2 SpacesInEnemiesMat { get; }

        int MatRows { get; set; }

        int MatCols { get; set; }

        Vector2 EnemyMatSize { get; }

        Vector2 MatStartPosition { get; }

        Vector2 MatStartDirection { get; }

        Vector2 MatJumpsSize { get; }

        float SecBetweenJumps { get; set; }

        float AccelerationFactor { get; }

        Color BulletsColor(int i_UserId);

        int MotherShipRandomParameter { get; }

        int ShootingRandomParameter { get; }

        Vector2 MotherShipStartDirection { get; }

        Vector2 MotherShipVelocity { get; }

        Vector2 SpaceShipVelocity { get; }

        Vector2 BulletsVelocity { get; }

        void UpdateGameState(eGameEvent i_Event, int i_UserId);
    }
}
