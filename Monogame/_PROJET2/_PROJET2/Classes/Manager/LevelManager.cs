using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace BricksGame
{
    public class LevelManager
    {
        private int currentLevel;
        public int CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
        private List<int> currentLevelDices;
        private BaseGrid gameGrid;
        public BaseGrid GameGrid { get { return gameGrid; } }
        public PlayerArea playingArea;
        private DicesFactory dicesFactory;
        private MonsterFactory monsterFactory;

        private LevelList listOfLevels;
        public enum LevelState { dices, play, gameOver, win, end };
        public LevelState currentState;



        public LevelManager()
        {
            currentLevel = 1;
            currentState = LevelState.dices;
        }

        public void LoadLevel(int level)
        {

            CreatePlayingArea();
            currentLevel = level;
            ReadLevelsData();
            CreateDicesIfLevelIsPlayable(level);
            
        }

        public void Update(GameTime gameTime)
        {

            int diceCount = 0;
            int monsterCount = 0;

            if (currentState == LevelState.dices)
            {
                CheckIfDicesBecameMonsters(gameGrid, diceCount, monsterCount);
            }

            else if (currentState == LevelState.play)
            {
                CheckIfTheLevelIsWin(gameGrid);
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


        private void CreatePlayingArea()
        {
            playingArea = new PlayerArea(63, 122, 483, 560);
            ServiceLocator.RegisterService(playingArea);
        }

        private void ReadLevelsData()
        {
            string json = File.ReadAllText(ServiceLocator.GetService<GameState>().currentLevelsJSON);
            listOfLevels = JsonSerializer.Deserialize<LevelList>(json);
        }

        private void CreateDicesIfLevelIsPlayable(int level)
        {
            if (level > listOfLevels.LevelsNb)
            {
                currentState = LevelState.end;
            }

            else if (listOfLevels != null)
            {
                CreateLevel(level);
            }
            else
            {
                throw new Exception("Il n'y a pas de niveau de défini");
            }
        }

        private void CreateLevel(int level)
        {
            currentState = LevelState.dices;
            CreateBaseGrid(9, 12); 
            CreateDicesFromData(level, gameGrid);
            InitMonsterFactory();
        }

        private void CreateDicesFromData(int level, BaseGrid grid)
        {
            currentLevelDices = ReadDicesData(level);
            CreateDices(grid, currentLevelDices);  
        }

        private List<int> ReadDicesData(int level)
        {
            Level levelData = listOfLevels.Levels[level - 1];
            List<int> levelDices = new List<int>();

            foreach (int[] lines in levelData.Dices)
            {
                foreach (int dice in lines)
                {
                    levelDices.Add(dice);
                }
            }
            return levelDices;
        }

        private void CreateDices(BaseGrid grid, List<int> dices)
        {
            InitDicesFactory();

            List<Dice> dicesList = dicesFactory.Load(dices, true);
            for (int n = 0; n < dicesList.Count; n++)
            {
                grid.AddBrickable(dicesList[n], n);
                ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(dicesList[n]);
            }
        }

        private void InitMonsterFactory()
        {
            monsterFactory = new MonsterFactory();
        }

        private void InitDicesFactory()
        {
            dicesFactory = new DicesFactory();
        }

        private void CreateBaseGrid(int lin, int col)
        {
            gameGrid = new BaseGrid(lin,col);
        }

   
        private void CheckIfDicesBecameMonsters(BaseGrid gameGrid, int diceCount, int monsterCount)
        {
           
           diceCount = RegisterDice(gameGrid, diceCount);
           monsterCount = RegisterMonster(gameGrid, monsterCount);
            
            if ((diceCount == 0) && (monsterCount > 0))
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
            currentState = LevelState.play;

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

        private void CheckIfTheLevelIsWin(BaseGrid gameGrid)
        {
           int monsterCount = 0;
            monsterCount = RegisterMonster(gameGrid, monsterCount);

            if (monsterCount == 0)
            {
                currentState = LevelState.win;
            }
        }

    }


}

