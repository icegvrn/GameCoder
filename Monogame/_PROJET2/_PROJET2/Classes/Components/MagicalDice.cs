using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BricksGame.Classes
{
    public class MagicalDice : Dice
    {
        /// <summary>
        /// Classe héritante de Dice qui représente un dés magique octroyant des pouvoir au joueur. Permet de dire si le pouvoir est disponible ou non.
        /// </summary>
        public bool DiceCanBeUsed { get; private set; }
        private bool powerAvailable;
  
        public MagicalDice() : base(Gamesystem.dice.dMagic, false)
        {
            SetVisible(false);
            DiceCanBeUsed = true;
        }

        // Méthode affichant/masquant le dés à la demande et indiquant si un pouvoir est disponible.
        public void EnableMagicalDice(bool boolean)
        {
            if (boolean)
            {
                SetVisible(true);
                RollDice();
                powerAvailable = true;
                DiceCanBeUsed = false;
            }
            else
            {
                SetVisible(false);
                DiceRolled = false;
                powerAvailable = false;
                DiceCanBeUsed = true;
            }      
        }

        // Dessin d'un message si un pouvoir des disponibles
        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if (powerAvailable)
            {
                p_SpriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.Font10), "Clic left to use power", new Vector2(Position.X-50, Position.Y + 50), Color.White);
            }
          
            base.Draw(p_SpriteBatch);
        }
    }
}
