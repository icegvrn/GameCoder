using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BricksGame
{
    public class LevelManager
    {
        private int currentLevel = 1;
        public int CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
        private List<int> currentLevelDices;
        private BaseGrid gameGrid;
        public BaseGrid GameGrid { get { return gameGrid; } }
        public PlayingAera playingAera;
        private DicesFactory dicesFactory;
        private MonsterFactory monsterFactory;
        public enum LevelState { dices, play, gameOver, win, end };
        public LevelState currentState;

        public LevelManager()
        {

        }

        public void LoadLevel(int level)
        {
            currentLevel = level;
            playingAera = new PlayingAera(63,122, 483, 560);
            ServiceLocator.RegisterService(playingAera);
            string json = File.ReadAllText("Content/Levels/level1.json");
            LevelList levelList = JsonSerializer.Deserialize<LevelList>(json);

            if (level > levelList.LevelsNb)
            {
                currentState = LevelState.end;
            }

            else
            {
                currentState = LevelState.dices;
                dicesFactory = new DicesFactory();
                monsterFactory = new MonsterFactory();
                gameGrid = new BaseGrid(9, 12);
                currentLevelDices = new List<int>();

                Level levelData = levelList.Levels[level - 1];

                foreach (int[] lines in levelData.Dices)
                {

                    foreach (int dice in lines)
                    {
                       
                            currentLevelDices.Add(dice);
                       

                        
                    }
                }
                List<Dice> dicesList = dicesFactory.Load(currentLevelDices);

                for (int n = 0; n < dicesList.Count; n++)
                {
                    gameGrid.AddBrickable(dicesList[n], n);
                    ServiceLocator.GetService<Scene>().AddToGameObjectsList(dicesList[n]);
                }
            }
        }

        public void NextLevel()
        {
            EndLevel();
            LoadLevel(currentLevel + 1);
        }

        public void EndLevel()
        {
           gameGrid.Clear();
        }

        public void NoBallActions()
        {
            gameGrid.Down();
        }

        public void Update(GameTime gameTime)
        {
            int diceCount = 0;
            int monsterCount = 0;

            if (currentState == LevelState.dices)
            {
                for (int n = 0; n < gameGrid.GridElements.Count(); n++)
                {
                    if (gameGrid.GridElements[n] is Dice )
                    {
                        Dice dice = (Dice)gameGrid.GridElements[n];

                        if (dice.facesNb != 0)
                        {
                            diceCount++;
                            if (dice.DiceRolled)
                            {
                                Monster c_monster = monsterFactory.CreateMonster(dice.DiceResult);
                                gameGrid.AddBrickable(c_monster, n);
                                ServiceLocator.GetService<Scene>().AddToGameObjectsList(c_monster);
                                gameGrid.GridElements[n] = null;
                                dice.Destroy();
                            }
                        }

                    }

                    else if (gameGrid.GridElements[n] is Monster)
                    {
                        monsterCount++;
                    }

                }

                if ((diceCount == 0) && (monsterCount > 0))
                {
                    for (int n = 0; n < gameGrid.GridElements.Count(); n++)
                    {
                        if (gameGrid.GridElements[n] is Monster)
                        {
                            monsterCount++;
                        }
                        else if ((gameGrid.GridElements[n] is Dice))
                        {
                            gameGrid.GridElements[n] = null; // Enlève les dès "0" qui sont restés sur la grille
                        }
                        currentState = LevelState.play;
                    }
                }
            }

            else if (currentState == LevelState.play)
            {
                monsterCount = 0;

                for (int n = 0; n < gameGrid.GridElements.Count(); n++)
                {
                    if (gameGrid.GridElements[n] is Monster)
                    {
                        if (!((Monster)gameGrid.GridElements[n]).IsDead)
                        {
                            monsterCount++;
                        }
                
                      
                    }
                }

              
                if (monsterCount == 0)
                {
                    currentState = LevelState.win;
                }

            }

            }
        }
    }

