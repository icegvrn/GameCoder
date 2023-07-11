using System;
using System.Diagnostics;


namespace BricksGame
{
    /// <summary>
    /// Classe contenant le pouvoir permettant de tuer un monstre directement
    /// </summary>
    public class KillPower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }

        public override string description { get; set; }
        public KillPower() : base()
        {
            description = "MONSTER ASSASSINATION!";
        }

        public override void Activate()
        {
            powerAvailable = false;
        }
        public override void Trigger(Bricks brick, BaseGrid grid)
        {
            if (!powerUsed)
            {
                if (brick is Monster)
                {
                    Monster m_brick = (Monster)brick;
                    m_brick.Kill();
                }
                powerUsed = true;
                powerCharged = false;

            }
               
        }
    }
}
