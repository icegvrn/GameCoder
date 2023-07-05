using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public int CurrentLevel { get; private set; }
        public enum LevelState { dices, play, gameOver, win, end };
        public LevelState CurrentState { get; set; }

        public BaseGrid GameGrid { get; private set; }
        private PlayerArea playingArea;
        private Texture2D map;

        private List<int> currentLevelDices;

        private DicesManager dicesManager;
        private LevelLoader levelLoader;


        public LevelManager()
        {
            CurrentLevel = 1;
            CurrentState = LevelState.dices;
            InitLevelLoader();
        }

        public void LoadLevel(int level)
        {
            CreatePlayingArea();
            CurrentLevel = level;
            CreateLevelIfIsPlayable(level);
        }

        public void Update(GameTime gameTime)
        {
            dicesManager.Update(gameTime);
            CheckIfTheLevelIsWin(GameGrid);

            if (ServiceLocator.GetService<IInputService>().IsKeyReleased(Keys.M))
            {
                GameGrid.Down();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(map, Vector2.Zero, Color.White);
            GameGrid.DrawGrid(spriteBatch);
            //GameGrid.DrawSlots(spriteBatch);
        }

        public void InitLevelLoader()
        {
            levelLoader = new LevelLoader(this);
        }

        public void NextLevel()
        {
            EndLevel();
            LoadLevel(CurrentLevel + 1);
        }

        public void EndLevel()
        {
            GameGrid.Clear();
        }

        public void OnNoBallInGame()
        {
            GameGrid.Down();
        }

        private void CreatePlayingArea()
        {
            playingArea = new PlayerArea(63, 122, 483, 560);
            ServiceLocator.RegisterService(playingArea);
        }

        private void CreateLevelIfIsPlayable(int level)
        {
            if (level > levelLoader.GetLevelsNb())
            {
                CurrentState = LevelState.end;
            }

            else if (levelLoader.IsThereLevels())
            {
                CreateLevel(level);
            }
        }

        private void CreateLevel(int level)
        {
            CurrentState = LevelState.dices;
            CreateBaseGrid(9, 12);
            InitDicesManager();
            CreateDicesFromData(level, GameGrid);
            LoadMap();
        }

        public void InitDicesManager()
        {
            dicesManager = new DicesManager(this, GameGrid);
        }

        private void CreateDicesFromData(int level, BaseGrid grid)
        {
            currentLevelDices = levelLoader.ReadDicesData(level);
            dicesManager.CreateDices(grid, currentLevelDices);  
        }

        private void CreateBaseGrid(int lin, int col)
        {
            GameGrid = new BaseGrid(lin,col);
        }


        private int RegisterMonster(BaseGrid p_gameGrid, int monsterCount)
        {
            for (int n = 0; n < GameGrid.GridElements.Count(); n++)
            {
                if (p_gameGrid.GridElements[n] is Monster && !((Monster)GameGrid.GridElements[n]).IsDead)
                {
                    monsterCount++;
                }
            }  
            return monsterCount;
        }

     
        private void CheckIfTheLevelIsWin(BaseGrid gameGrid)
        {
            if (CurrentState == LevelState.play)
            {
                int monsterCount = 0;
                monsterCount = RegisterMonster(gameGrid, monsterCount);

                if (monsterCount == 0)
                {
                    CurrentState = LevelState.win;
                }
            }
        }

        public void OnAllDicesRolled()
        {
            CurrentState = LevelState.play;

        }

        private void LoadMap()
        {
                map = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot() + "map1");
        }

    }


}

