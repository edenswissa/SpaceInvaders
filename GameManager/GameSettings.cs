using System;
using Microsoft.Xna.Framework;

namespace GameManager
{
    public class GameSettings
    {
        public Vector2 SpaceShipVelocity { get; set; }

        public Vector2 MotherShipVelocity { get; set; }

        public Vector2 BulletsVelocity { get; set; }

        public Vector2 MotherShipStartDirection { get; set; }

        public Vector2 EnemySize { get; set; }

        public Vector2 SpacesInEnemiesMat { get; set; }

        public Vector2 MatDimentionsLengths { get; set; }

        public Vector2 MatStartPosition { get; set; }

        public Vector2 MatStartDirection { get; set; }

        public float MatJumpWidth { get; set; }

        public float MatJumpHeight { get; set; }

        public float AccelerationFactor { get; set; }

        public float SecondsBetweenJumps { get; set; }

        public int SpaceShipsBulletsLimit { get; set; }

        public int SoulsLimit { get; set; }

        public int MotherShipRandomParameter { get; set; }

        public int ShootingRandomParameter { get; set; }

        public GameObjectsValues ObjectsValues { get; set; }

        public GameSettings()
        {
            init();
        }

        private void init()
        {
            SpaceShipVelocity = new Vector2(160, 0);
            MotherShipVelocity = new Vector2(105, 0);
            BulletsVelocity = new Vector2(0, 120);
            MotherShipStartDirection = new Vector2(-1f, 0);
            EnemySize = new Vector2(32, 32);
            SpacesInEnemiesMat = new Vector2(32 * 0.6f, 32 * 0.6f);
            MatDimentionsLengths = new Vector2(9, 5);
            SpaceShipsBulletsLimit = 2;
            SoulsLimit = 3;
            MatStartPosition = new Vector2(0, 3 * EnemySize.Y);
            MatStartDirection = new Vector2(1f, 0);
            MatJumpWidth = EnemySize.X / 2;
            MatJumpHeight = EnemySize.Y / 2;
            SecondsBetweenJumps = 0.5f;
            AccelerationFactor = 0.04f;
            MotherShipRandomParameter = 601;
            ShootingRandomParameter = 2501;
            ObjectsValues = new GameObjectsValues(new int[] { 1200, 650, 240, 170, 140 });
        }
    }
}