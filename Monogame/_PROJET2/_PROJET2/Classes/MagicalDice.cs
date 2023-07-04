using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BricksGame.Classes
{
    internal class MagicalDice : Dice
    {

        public bool diceCanBeUsed;
        public bool powerAvailable;
  
        public MagicalDice() : base(Gamesystem.dice.dMagic, false)
        {
            SetVisible(false);
            diceCanBeUsed = true;
        }

        public override void Update(GameTime p_GameTime)
        {
            base.Update(p_GameTime);
        }

        public void EnableMagicalDice(bool boolean)
        {
            if (boolean)
            {
                SetVisible(true);
                RollDice();
                powerAvailable = true;
                diceCanBeUsed = false;
            }
            else
            {
                SetVisible(false);
                DiceRolled = false;
                powerAvailable = false;
                diceCanBeUsed = true;
     
            }
               
        }



        public override void Draw(SpriteBatch p_SpriteBatch)
        {
            if (powerAvailable)
            {
                p_SpriteBatch.DrawString(AssetsManager.Font10, "Press SPACE to use power", new Vector2(Position.X-50, Position.Y + 50), Color.White);
            }
          
            base.Draw(p_SpriteBatch);
        }
    }
}
