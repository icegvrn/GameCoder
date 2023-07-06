using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace BricksGame
{
    /// <summary>
    /// Le levelLoader est la classe qui permet de lire le fichier JSON des niveaux. Il récupère les infos dans un objet LevelList
    /// </summary>
    public class LevelLoader
    {

        private LevelList listOfLevels;
        private LevelManager levelManager;

        public LevelLoader(LevelManager p_levelManager)
        {
            levelManager = p_levelManager;
            ReadLevelsData();
        }

        // Récupère les datas du JSON
        public void ReadLevelsData()
        {
            string json = File.ReadAllText(ServiceLocator.GetService<GameState>().currentLevelsJSON);
            listOfLevels = JsonSerializer.Deserialize<LevelList>(json);
        }

        // Retourne une liste de int en fonction de ce qui se trouve dans les Datas JSON récupéré
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

        // Méthode renvoyant le nombre de niveaux présents dans le JSON
        public int GetLevelsNb()
        {
            return listOfLevels.LevelsNb;
        }

        // Méthode permettant de s'assurer qu'il y a bien au moins un niveau
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
