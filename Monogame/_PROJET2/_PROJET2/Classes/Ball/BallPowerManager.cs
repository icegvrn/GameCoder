using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
    public class BallPowerManager : PowerManager
    {
        private Ball ball;
        private int initialStrenght;


        public BallPowerManager(Ball p_ball)
        {
            ball = p_ball;
            initialStrenght = ball.Strenght;
            Power = null;
        }

        public override void Update(GameTime p_GameTime)
        {
            if (Power != null && Power is DamagePower)
            {
                DamagePower dmgPower = ((DamagePower)Power);

                if (dmgPower.powerIsOn)
                {
                    ball.Strenght = initialStrenght * dmgPower.multiplicator;
                }
                else
                {
                    ball.Strenght = initialStrenght;
                }

                dmgPower.Update(p_GameTime);
            }

            if (Power is not null && Power.powerCharged)
            {
                ball.OnPowerCharged();
            }
            else
            {
                ball.OnPowerUsed();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Power != null && Power is DamagePower)
            {
                DamagePower dmgPower = ((DamagePower)Power);
                if (dmgPower.powerIsOn)
                {
                    spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.MainFont), ((int)dmgPower.timer).ToString(), new Vector2(ball.Position.X - 10, ball.Position.Y - 10), Color.Red);
                }
            }
        }

        public override void ActivatePower()
        {
            if (Power != null)
            {
                Power.Activate();
            }
        }

        public void ActivatePower(Power power)
        {
            Power = power;
            Power.powerAvailable = true;
            Power.Activate();
            Power.powerCharged = true;
        }
        public override void TriggerPower()
        {
            if (Power != null)
            {
                Power.powerUsed = true;
            }
        }
        public void TriggerPower(Monster p_monster)
        {
            if (Power != null)
            {
                Power.Trigger(p_monster, ServiceLocator.GetService<LevelManager>().GameGrid);
                Power.powerUsed = true;
            }
        }

        public override void ResetPower()
        {
            Power = null;
        }


        public bool IsPowerCharged()
        {
            if (Power is not null && Power.powerCharged)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
