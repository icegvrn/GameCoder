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
        private DicesFactory dicesFactory;
        private GameManager gameManager;
        private MonsterFactory monsterFactory;
        public LevelManager(GameManager gameManager) { this.gameManager = gameManager; }
        public enum LevelState {dices, play, gameOver, win};
        public LevelState currentState;

        public void LoadLevel(int level)
        {
            currentState = LevelState.dices;
            dicesFactory = new DicesFactory();
            monsterFactory = new MonsterFactory();
            gameGrid = new BaseGrid(6,6);
            currentLevelDices = new List<int>();

            string json = File.ReadAllText("Content/Levels/level"+level+".json");
            Level levelData = JsonSerializer.Deserialize<Level>(json);
            
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
                gameManager.RegisterActor(dicesList[n]);
            }
          
        }

        public void NoBallActions()
        {
            gameGrid.Down();
        }

        public void Update(GameTime gameTime)
        {

            int diceCount = 0;
            int monsterCount = 0;

            for (int n= 0; n < gameGrid.GridElements.Count(); n++)
            {
                if (gameGrid.GridElements[n] is Dice)
                { 
                    Dice dice = (Dice)gameGrid.GridElements[n];

                    if (dice.facesNb != 0) {
                        diceCount++;
                        if (dice.DiceRolled)
                        {
                            Monster c_monster = monsterFactory.CreateMonster(dice.DiceResult);
                             gameGrid.AddBrickable(c_monster, n);
                            gameManager.RegisterActor(c_monster);
                            gameGrid.GridElements[n] = null;
                            dice.Destroy();
                          
                        }
                    }
                   
                }

                else if(gameGrid.GridElements[n] is Monster)
                {
                    monsterCount++;
                }
            }
            if ((diceCount == 0) && (monsterCount > 0)) {
                currentState = LevelState.play;
            }
            Debug.WriteLine(diceCount);
            Debug.WriteLine(currentState);
        }
    }
}
