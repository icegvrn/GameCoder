﻿

namespace BricksGame
{
    /// <summary>
    /// Classe contenant le pouvoir permettant de tirer une seconde balle
    /// </summary>
    public class DoubleBallsPower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }

        public override string description { get; set; }
        public DoubleBallsPower() : base()
        {
            description = "SLOWER SECOND BALL!";
        }

        public override void Activate()
        {
            powerAvailable = false;
            powerUsed = true;
            powerCharged = false;
        }
        public override void Trigger(Bricks brick, BaseGrid grid)
        {
        }
    }
}