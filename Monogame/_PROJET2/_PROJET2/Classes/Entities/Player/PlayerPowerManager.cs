using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
    /// <summary>
    /// Classe qui gère les pouvoirs dont dispose le joueur et les points qui permettent de les obtenirs. 
    /// </summary>
    public class PlayerPowerManager : PowerManager
    {
        private Player player;
        private MagicalDice magicalDice;

        private bool magicalDiceResultChecked;
        private PlayerPowerUI powerUI;
        private ISessionService playerState;

        public PlayerPowerManager(Player p_player)
        {
            player = p_player;
            InitPlayerPoints();
            InitMagicalDice();
            powerUI = new PlayerPowerUI(this);

        }

        // Update permettant de vérifier si le joueur a atteint le maximum de points et peut donc disposer d'un pouvoir. Si oui, le magicDice se lance. Si le magicDice a un résultat alors on associe au joueur le pouvoir qu'il faut.
        // Update de l'UI lié aux pouvoirs
        public override void Update(GameTime gameTime)
        {
            if (!magicalDiceResultChecked && player.IsReady && playerState.GetPoints() == playerState.GetMaxPoints() && magicalDice.DiceCanBeUsed)
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

        // Dessin de l'UI des pouvoirs
        public override void Draw(SpriteBatch spriteBatch)
        {
            powerUI.Draw(spriteBatch);
        }

        // Initiation des points du joueur
        public void InitPlayerPoints()
        {
            playerState = ServiceLocator.GetService<ISessionService>();
            playerState.SetPoints(0);
            playerState.SetMaxPoints(3000);
        }

        // Initiation du dé magique utilisé pour déterminer quel pouvoir le joueur va gagner.
        public void InitMagicalDice()
        {
            magicalDice = new MagicalDice();
            magicalDice.Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - 125, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 80);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(magicalDice);
        }

        // Méthode permettant de choisir le pouvoir à attribuer au joueur en fonction du résultat du magicDice
        private void CheckMagicalDiceResult()
        {
            switch (magicalDice.DiceResult)
            {
                case 1:
                    Power = new DoubleBallsPower();
                    player.playerFighter.CreateNewBall();       
                    break;
                case 2:
                    Power = new LinePower();
                    break;
                case 3:
                    Power = new KillPower();
                    break;
                case 4:
                    Power = new SameTypePower();
                    break;
                case 5:
                    Power = new ExplodePower();
                    break;
                case 6:
                    Power = new DamagePower();
                    break;
                default:
                    Power = new KillPower();
                    break;
            }
            magicalDice.ChangeDescription(Power.description);
            Power.powerAvailable = true;
        }

        // Fonction permettant d'activer le pouvoir ; comportement différent si le pouvoir est DoubleBalls car il n'a pas a être réactivé : il trigger directement.
        // Cela passe par le PlayerFighter qui va transmettre les infos pouvoir à la balle
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

        // Méthode appelée quand un pouvoir a Trigger
        public override void TriggerPower()
        {
            ResetPower();
        }

        // Méthode permettant de reset les pouvoirs quand un pouvoir a trigger
        public override void ResetPower()
        {
            Power = null;
            magicalDice.EnableMagicalDice(false);
        }
    }
}