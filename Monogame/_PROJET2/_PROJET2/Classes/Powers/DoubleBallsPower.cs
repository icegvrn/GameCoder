using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class DoubleBallsPower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }
        public DoubleBallsPower() : base()
        {
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