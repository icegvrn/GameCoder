using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
    public class PlayerPowerManager : PowerManager
    {
        private Player player;
        private MagicalDice magicalDice;

        private bool magicalDiceResultChecked;
        private PlayerPowerUI powerUI;
        private ISessionService playerState;
        public PlayerPowerManager(Player p_player)
        {
            playerState = ServiceLocator.GetService<ISessionService>();
            playerState.SetPoints(2950);
            playerState.SetMaxPoints(3000);

            player = p_player;

            magicalDice = new MagicalDice();
            magicalDice.Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - 125, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 80);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(magicalDice);
            powerUI = new PlayerPowerUI(this);

        }

        public override void Update(GameTime gameTime)
        {
            if (!magicalDiceResultChecked && player.IsReady && playerState.GetPoints() == playerState.GetMaxPoints() && magicalDice.diceCanBeUsed)
            {
                magicalDice.EnableMagicalDice(true);
                magicalDiceResultChecked = true;
            }
            else if (magicalDice.DiceRolled && Power is null)
            {
                CheckMagicalDiceResult();
                magicalDiceResultChecked = false;
            }
            powerUI.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            powerUI.Draw(spriteBatch);
        }

        private void CheckMagicalDiceResult()
        {

            switch (magicalDice.DiceResult)
            {
                case 1:
                    Power = new DoubleBallsPower();
                    Power.powerAvailable = true;
                    player.playerFighter.CreateNewBall();
                    break;
                case 2:
                    Power = new LinePower();
                    Power.powerAvailable = true;
                    break;
                case 3:
                    Power = new KillPower();
                    Power.powerAvailable = true;
                    break;
                case 4:
                    Power = new SameTypePower();
                    Power.powerAvailable = true;
                    break;
                case 5:
                    Power = new ExplodePower();
                    Power.powerAvailable = true;
                    break;
                case 6:
                    Power = new DamagePower();
                    Power.powerAvailable = true;
                    break;
                default:
                    break;
            }
        }

        public override void ActivatePower()
        {
       
            if (Power is not null)
            {
                if (Power is not DoubleBallsPower)
                {
                    if (Power.powerAvailable)
                    {
                        player.playerFighter.ActivatePowerOnBall(Power);
                        playerState.SetPoints(0);
                    }
                    else if (Power.powerCharged)
                    {
                        player.playerFighter.TriggerPower();
                    }
                }
                else if (Power is DoubleBallsPower)
                {
                    if (Power.powerAvailable)
                    {
                        player.playerFighter.FireASecondaryBall();
                        Power.powerAvailable = false;
                        Power.powerUsed = true;
                        playerState.SetPoints(0);

                    }
                }
                ResetPower();
            }
        }

        public override void TriggerPower()
        {
            ResetPower();
        }

        public override void ResetPower()
        {
            Power = null;
            magicalDice.EnableMagicalDice(false);
        }
    }
}