using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame
{
    /// <summary>
    /// Classe héritante de PowerManager qui gère les pouvoirs propres à la balle. Utilisé pour le pouvoir DamagePower
    /// </summary>
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

        // Update du PowerManager : si le pouvoir est un DamagePower, on modifie la force de la balle durant un temps donné, ou alors on remet la force initial si le temps est terminé.
        // Si c'est un autre pouvoir et que le pouvoir est chargé, on appelle la méthode OnPowerChanged sur la ball, sinon PowerUsed.
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

        // Dessin du temps restant pour le DamagePower s'il s'agit de ce pouvoir là
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

        // Active le pouvoir en cours de la ball
        public override void ActivatePower()
        {
            if (Power != null)
            {
                Power.Activate();
            }
        }

        // Active un pouvoir donné via paramètre sur la ball
        public void ActivatePower(Power power)
        {
            Power = power;
            Power.powerAvailable = true;
            Power.Activate();
            Power.powerCharged = true;
        }

        // Trigger le pouvoir en cours sur la balle
        public override void TriggerPower()
        {
            if (Power != null)
            {
                Power.powerUsed = true;
            }
        }

        // Trigger un pouvoir donné via paramètre sur la ball
        public void TriggerPower(Monster p_monster)
        {
            if (Power != null)
            {
                Power.Trigger(p_monster, ServiceLocator.GetService<LevelManager>().GameGrid);
                Power.powerUsed = true;
            }
        }

        // Vérifie si un pouvoir est actuellement chargé ou non
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

        // Reset les pouvoirs de la balle en mettant Power sur null
        public override void ResetPower()
        {
            Power = null;
        }

       
    }
}
