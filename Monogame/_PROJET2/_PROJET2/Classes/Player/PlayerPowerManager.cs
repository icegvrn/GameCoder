using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BricksGame
{
    public class PlayerPowerManager
    {
        private Player player;
        private MagicalDice magicalDice;
        private Power power;
        private bool magicalDiceResultChecked;
        private PlayerPowerUI powerUI;

        public PlayerPowerManager(Player player)
        {
            PlayerState.SetPoints(2950);
            PlayerState.SetMaxPoints(3000);
            this.player = player;
            magicalDice = new MagicalDice();
            magicalDice.Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - 125, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 80);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(magicalDice);
            powerUI = new PlayerPowerUI(this);
        }

        public void Update(GameTime gameTime)
        {
            if (!magicalDiceResultChecked && player.IsReady && PlayerState.Points == PlayerState.MaxPoints && magicalDice.diceCanBeUsed)
            {
                magicalDice.EnableMagicalDice(true);
                magicalDiceResultChecked = true;
            }
            else if (magicalDice.DiceRolled && power is null)
            {
                CheckMagicalDiceResult();
                magicalDiceResultChecked = false;
            }
            powerUI.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            powerUI.Draw(spriteBatch);
        }

        private void CheckMagicalDiceResult()
        {

            switch (magicalDice.DiceResult)
            {
                case 1:
                    power = new DoubleBallsPower();
                    power.powerAvailable = true;
                    player.CreateNewBall();
                    break;
                case 2:
                    power = new LinePower();
                    power.powerAvailable = true;
                    break;
                case 3:
                    power = new KillPower();
                    power.powerAvailable = true;
                    break;
                case 4:
                    power = new SameTypePower();
                    power.powerAvailable = true;
                    break;
                case 5:
                    power = new ExplodePower();
                    power.powerAvailable = true;
                    break;
                case 6:
                    power = new DamagePower();
                    power.powerAvailable = true;
                    break;
                default:
                    break;
            }
        }

        public void ActivatePower()
        {

            if (power is not null)
            {
                if (power is not DoubleBallsPower)
                {
                    if (power.powerAvailable)
                    {
                        player.BallsList[0].power = power;
                        player.BallsList[0].power.powerAvailable = true;
                        player.BallsList[0].ActivePower();
                        player.BallsList[0].power.powerCharged = true;
                        PlayerState.SetPoints(0);
                    }
                    else if (power.powerCharged)
                    {
                        player.BallsList[0].TriggerPower();
                    }
                }
                else if (power is DoubleBallsPower)
                {
                    if (power.powerAvailable)
                    {
                        player.BallsList[1].Fire();
                        player.BallsList[1].SpeedVector *= 0.5f;
                        player.BallsList[1].Speed *= 0.5f;
                        power.powerAvailable = false;
                        power.powerUsed = true;
                        PlayerState.SetPoints(0);

                    }
                }
                ResetPower();
            }
        }

        public void ResetPower()
        {
            power = null;
            magicalDice.EnableMagicalDice(false);
        }
    }
}