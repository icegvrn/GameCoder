using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BricksGame
{
    public class LevelLoader
    {

        private LevelList listOfLevels;
        private LevelManager levelManager;

        public LevelLoader(LevelManager p_levelManager)
        {
            levelManager = p_levelManager;
            ReadLevelsData();
        }

        public void ReadLevelsData()
        {
            string json = File.ReadAllText(ServiceLocator.GetService<GameState>().currentLevelsJSON);
            listOfLevels = JsonSerializer.Deserialize<LevelList>(json);
        }

        public List<int> ReadDicesData(int level)
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

        public int GetLevelsNb()
        {
            return listOfLevels.LevelsNb;
        }

        public bool IsThereLevels()
        {
            if (listOfLevels != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

   
}
