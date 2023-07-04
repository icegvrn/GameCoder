using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class DamagePower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }

        public bool powerIsOn;

        public int multiplicator;
        public int duration;
        public float timer;
        public bool timerStarted;
        public DamagePower() : base()
        {
            multiplicator = 10;
            duration = 8;
        }

        public void Update(GameTime gameTime)
        {
            if (powerIsOn)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timer <= 0)
                {
                    powerIsOn = false;
                    powerCharged = false;
                }
            }
        }

        public override void Activate()
        {
            powerAvailable = false;
        }
        public override void Trigger(Bricks brick, BaseGrid grid)
        {

            if (!powerUsed)
            {
                timerStarted = true;
                powerIsOn = true;
                timer = duration;
                powerUsed = true;
            }

        }
    }

}
