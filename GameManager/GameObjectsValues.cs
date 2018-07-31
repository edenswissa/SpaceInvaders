using System;

namespace GameManager
{
    public class GameObjectsValues
    {
        public GameObjectsValues(int[] i_Values)
        {
            SpaceShip = i_Values[0];
            MotherShip = i_Values[1];
            PinkEnemy = i_Values[2];
            PaleBlueEnemy = i_Values[3];
            YellowEnemy = i_Values[4];
        }

        public int SpaceShip { get; set; }

        public int MotherShip { get; set; }

        public int PinkEnemy { get; set; }

        public int PaleBlueEnemy { get; set; }

        public int YellowEnemy { get; set; }
    }
}
