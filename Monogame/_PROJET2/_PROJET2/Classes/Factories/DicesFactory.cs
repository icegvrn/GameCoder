using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame
{
    public class DicesFactory
    {
        List<Dice> dicesList;
        List<Bricks> bricksList;
        public List<Dice> Load(List<int> dices)
        {
            Dice c_dice;
            dicesList = new List<Dice>();
            foreach (int dice in dices)
            {
                switch (dice)
                {
                    case 3:
                        c_dice = new Dice(Gamesystem.dice.d3);
                        dicesList.Add(c_dice);
                        break;
                    case 4:
                        c_dice = new Dice(Gamesystem.dice.d4);
                        dicesList.Add(c_dice);
                        break;
                    case 6:
                        c_dice = new Dice(Gamesystem.dice.d6);
                        dicesList.Add(c_dice);
                        break;
                    case 8:
                        c_dice = new Dice(Gamesystem.dice.d8);
                        dicesList.Add(c_dice);
                        break;
                    case 10:
                        c_dice = new Dice(Gamesystem.dice.d10);
                        dicesList.Add(c_dice);
                        break;
                    case 12:
                        c_dice = new Dice(Gamesystem.dice.d12);
                        dicesList.Add(c_dice);
                        break;
                    case 20:
                        c_dice = new Dice(Gamesystem.dice.d20);
                        dicesList.Add(c_dice);
                        break;
                    default:
                        c_dice = new Dice(Gamesystem.dice.none);
                        dicesList.Add(c_dice);
                        break;
                }
            }
            return dicesList;
        }

        public void AddDiceToGrid(List<Bricks> bricks)
        {
            bricksList = bricks;
            for (int n = 0; n < 6; n++)
            {
                for (int i = 0; i < 6; i++)
                {
                    dicesList[i + n * 6].Position = new Vector2(bricks[i + n * 6].Position.X + bricks[i + n * 6].BoundingBox.Width / 2, bricks[i + n * 6].Position.Y + bricks[i + n * 6].BoundingBox.Height / 2);

                }
            }
        }


    }
}
