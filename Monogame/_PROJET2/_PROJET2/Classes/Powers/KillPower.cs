using System;
using System.Diagnostics;


namespace BricksGame
{
    public class KillPower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }
        public KillPower() : base()
        {
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
