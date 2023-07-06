using System.Collections.Generic;
using System.Numerics;

namespace BricksGame
{
    /// <summary>
    /// La DicesFactory retourne une liste de d'objet Dice à partir d'une liste de valeurs int.
    /// </summary>
    public class DicesFactory
    {
        List<Dice> dicesList;
        public List<Dice> Load(List<int> dices, bool rollable)
        {
            Dice c_dice;
            dicesList = new List<Dice>();
            foreach (int dice in dices)
            {
                switch (dice)
                {
                    case 3:
                        c_dice = new Dice(Gamesystem.dice.d3, rollable);
                        dicesList.Add(c_dice);
                        break;
                    case 4:
                        c_dice = new Dice(Gamesystem.dice.d4, rollable);
                        dicesList.Add(c_dice);
                        break;
                    case 6:
                        c_dice = new Dice(Gamesystem.dice.d6, rollable);
                        dicesList.Add(c_dice);
                        break;
                    case 8:
                        c_dice = new Dice(Gamesystem.dice.d8, rollable);
                        dicesList.Add(c_dice);
                        break;
                    case 10:
                        c_dice = new Dice(Gamesystem.dice.d10, rollable);
                        dicesList.Add(c_dice);
                        break;
                    case 12:
                        c_dice = new Dice(Gamesystem.dice.d12, rollable);
                        dicesList.Add(c_dice);
                        break;
                    case 20:
                        c_dice = new Dice(Gamesystem.dice.d20, rollable);
                        dicesList.Add(c_dice);
                        break;
                    default:
                        c_dice = new Dice(Gamesystem.dice.none, rollable);
                        dicesList.Add(c_dice);
                        break;
                }
            }
            return dicesList;
        }
    }
}
