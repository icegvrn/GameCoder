using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
         //   Debug.WriteLine("POUVOIR ACTIVE");
        }
        public override void Trigger(Bricks brick, BaseGrid grid)
        {
            if (!powerUsed)
            {
                Debug.WriteLine("SAME TYPE : POUVOIR ENVOYE !");

                if (brick is Monster)
                {
                    Monster m_brick = (Monster)brick;
                    m_brick.Kill();
                }
                Debug.WriteLine(" DICE MOUVEMENT : POUVOIR UTILISE");
                powerUsed = true;
                powerCharged = false;

            }
               
        }
    }
}
