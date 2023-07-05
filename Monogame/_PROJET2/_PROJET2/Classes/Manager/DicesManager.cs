
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static BricksGame.LevelManager;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework;

namespace BricksGame
{
    public class DicesManager
    {
        private List<int> currentLevelDices;
        private BaseGrid gameGrid;
        private DicesFactory dicesFactory;
        private MonsterFactory monsterFactory;
        private LevelManager levelManager;

        public DicesManager(LevelManager p_levelManager, BaseGrid p_gameGrid)
        {
            gameGrid = p_gameGrid;
            levelManager = p_levelManager;
            InitMonsterFactory();
            InitDicesFactory();
        }

        public void CreateDices(BaseGrid grid, List<int> dices)
        {
            InitDicesFactory();

            List<Dice> dicesList = dicesFactory.Load(dices, true);
            for (int n = 0; n < dicesList.Count; n++)
            {
                grid.AddBrickable(dicesList[n], n);
                ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(dicesList[n]);
            }
        }

        public void Update(GameTime gameTime)
        {
            int diceCount = 0;
            int monsterCount = 0;

            if (levelManager.CurrentState == LevelState.dices)
            {
                CheckIfDicesBecameMonsters(gameGrid, diceCount, monsterCount);
            }
        }

        private void InitDicesFactory()
        {
            dicesFactory = new DicesFactory();
        }

        private void InitMonsterFactory()
        {
            monsterFactory = new MonsterFactory();
        }

       
        public void CheckIfDicesBecameMonsters(BaseGrid gameGrid, int diceCount, int monsterCount)
        {

            diceCount = RegisterDice(gameGrid, diceCount);
            monsterCount = RegisterMonster(gameGrid, monsterCount);

            if (diceCount == 0 && monsterCount > 0)
            {
                ActionsIfAllDicesRolled(gameGrid, monsterCount);
            }
        }

        private int RegisterMonster(BaseGrid p_gameGrid, int monsterCount)
        {
            for (int n = 0; n < gameGrid.GridElements.Count(); n++)
            {
                if (p_gameGrid.GridElements[n] is Monster && !((Monster)gameGrid.GridElements[n]).IsDead)
                {
                    monsterCount++;
                }
            }
            return monsterCount;
        }

        private int RegisterDice(BaseGrid p_gameGrid, int diceCount)
        {
            for (int n = 0; n < gameGrid.GridElements.Count(); n++)
            {
                if (p_gameGrid.GridElements[n] is Dice)
                {
                Dice dice = (Dice)p_gameGrid.GridElements[n];

                    if (dice.facesNb != 0)
                    {
                        diceCount++;

                        if (dice.DiceRolled)
                        {
                            ConvertDiceToMonster(p_gameGrid, dice, n);
                        }
                    }
                }
            }
            return diceCount;
        }

        private void ActionsIfAllDicesRolled(BaseGrid gameGrid, int monsterCount)
        {
            monsterCount = RegisterMonster(gameGrid, monsterCount);
            DeleteDicesFromGrid(gameGrid);
            levelManager.OnAllDicesRolled();
        }

        private void DeleteDicesFromGrid(BaseGrid gameGrid)
        {
            for (int n = 0; n < gameGrid.GridElements.Count(); n++)
            {
                if ((gameGrid.GridElements[n] is Dice))
                {
                    gameGrid.GridElements[n] = null; // Enlève les dès "0" qui sont restés sur la grille
                }
            }
        }

        private void ConvertDiceToMonster(BaseGrid gameGrid, Dice dice, int index)
        {
            Monster c_monster = monsterFactory.CreateMonster(dice.DiceResult);
            gameGrid.AddBrickable(c_monster, index);
            ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(c_monster);
            gameGrid.GridElements[index] = null;
            dice.Destroy();
        }
    }
}

