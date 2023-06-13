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
        private DicesManager dicesManager;
        private GameManager gameManager;

        public LevelManager(GameManager gameManager) { this.gameManager = gameManager; }

        public void LoadLevel(int level)
        {
            dicesManager = new DicesManager();
            gameGrid = new BaseGrid(6,6);
            currentLevelDices = new List<int>();

            string json = File.ReadAllText("Content/Levels/level"+level+".json");
            Level levelData = JsonSerializer.Deserialize<Level>(json);
            
            foreach (int[] lines in levelData.Dices)
            {
                Debug.WriteLine("TEST");
                foreach (int dice in lines)
                {
            
                    currentLevelDices.Add(dice);
                }
            }
            List<Dice> dicesList = dicesManager.Load(currentLevelDices);
          
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
            for (int n= 0; n < gameGrid.GridElements.Count(); n++)
            {
                if (gameGrid.GridElements[n] is Dice)
                {
                    Dice dice = (Dice)gameGrid.GridElements[n];
                    if (dice.DiceRolled)
                    {
                        Debug.WriteLine("TRUE");
                        List<Texture2D> list = new List<Texture2D>();
                        Texture2D texture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/brick");
                        list.Add(texture);
                        Bricks brick = new Bricks(list, dice.DiceResult);
                        dice.Destroy();
                        gameGrid.AddBrickable(brick, n);
                        gameManager.RegisterActor(brick);
                    }

                }
            }
        }
    }
}
