using Microsoft.Xna.Framework;


namespace BricksGame
{
    /// <summary>
    /// Classe contenant le pouvoir permettant d'augmenter les dégâts de la balle durant un temps donné
    public class DamagePower : Power
    {
        public override bool powerCharged { get; set; }
        public override bool powerAvailable { get; set; }
        public override bool powerUsed { get; set; }
        public override string description { get; set; }

        public bool powerIsOn;

        public int multiplicator;
        public int duration;
        public float timer;
        public bool timerStarted;
        public DamagePower() : base()
        {
            multiplicator = 10;
            duration = 8;
            description = "DAMAGE BOOSTER ! (5 sec)";
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
