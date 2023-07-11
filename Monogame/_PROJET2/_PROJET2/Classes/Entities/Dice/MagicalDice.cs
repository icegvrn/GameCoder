using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BricksGame.Classes
{
    public class MagicalDice : Dice
    {
        /// <summary>
        /// Classe héritante de Dice qui représente un dés magique octroyant des pouvoir au joueur. Permet de dire si le pouvoir est disponible ou non.
        /// </summary>
        public bool DiceCanBeUsed { get; private set; }
        private bool powerAvailable;
        private string stringPower;
  
        public MagicalDice() : base(Gamesystem.dice.dMagic, false)
        {
            SetVisible(false);
            DiceCanBeUsed = true;
            stringPower = "";
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

        // Méthode utilisée pour décrire au joueur le pouvoir disponible
        public void ChangeDescription(string desc)
        {
            Debug.WriteLine("JE CHANGE LA DESCRIPTION SUR " + desc);
          stringPower = desc;
        }

        // Dessin d'un message si un pouvoir des disponibles
        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if (powerAvailable)
            {
                p_SpriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.Font10), "Clic left to use power", new Vector2(Position.X-50, Position.Y + 50), Color.White);
              if (DiceRolled)
                {
                    p_SpriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.MainFont), stringPower, new Vector2(10, 10), Color.White);
                }
            }
          
            base.Draw(p_SpriteBatch);
        }
    }
}
