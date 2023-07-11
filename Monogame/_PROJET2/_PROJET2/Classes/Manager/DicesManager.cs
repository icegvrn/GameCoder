using static BricksGame.LevelManager;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace BricksGame
{
    /// <summary>
    /// Le DicesManager gère le spawn des dés sur la grille et leur conversion en IBrickable de type monstre. Il connait le nombre de dés présents etc.
    /// </summary>
    public class DicesManager
    {
        private BaseGrid gameGrid;
        private DicesFactory dicesFactory;
        private MonstersFactory monsterFactory;
        private LevelManager levelManager;

        public DicesManager(LevelManager p_levelManager, BaseGrid p_gameGrid)
        {
            gameGrid = p_gameGrid;
            levelManager = p_levelManager;
            InitMonsterFactory();
            InitDicesFactory();
        }

        // Vérification à chaque instant s'il reste encore des dés où s'il n'y a plus que des monstres (= tous les dés lancés = début du jeu)
        public void Update(GameTime gameTime)
        {
            int diceCount = 0;
            int monsterCount = 0;

            if (levelManager.CurrentState == LevelState.dices)
            {
                CheckIfDicesBecameMonsters(gameGrid, diceCount, monsterCount);
            }
        }

        // Créé des dés sur une grille à partir d'une liste de valeurs (souvent issu du JSON)
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

        private void InitDicesFactory()
        {
            dicesFactory = new DicesFactory();
        }

        private void InitMonsterFactory()
        {
            monsterFactory = new MonstersFactory();
        }

       // Méthode qui vérifie si tous les dés ont été lancé et s'il y a des monstres : si oui, cela signifie qu'on est dans la phase de début de jeu et on appelle la méthode indiquant que tous les dés ont été lancés.
        public void CheckIfDicesBecameMonsters(BaseGrid gameGrid, int diceCount, int monsterCount)
        {
            diceCount = RegisterDice(gameGrid, diceCount);
            monsterCount = RegisterMonster(gameGrid, monsterCount);

            if (diceCount == 0 && monsterCount > 0)
            {
                ActionsIfAllDicesRolled(gameGrid, monsterCount);
            }
        }

        // Methode permettant de compter le nombre de monstres restant dans la grille.
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

        // Méthode permettant de compter le nombre de dés restant dans la grille et son état. Si un dés a été lancé, on le converti en monstre.
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

        // Quand tous les dés ont été lancé, on supprime tous les dés, on compte le nombre de monstres et on signal au levelManager que tous les dés ont été lancés
        private void ActionsIfAllDicesRolled(BaseGrid gameGrid, int monsterCount)
        { 
            DeleteDicesFromGrid(gameGrid);
            levelManager.OnAllDicesRolled();
        }

        // Méthode permettant de supprimer tous les dés de la grille. 
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

        // Méthode permettant de remplacer le dé situé à un index donné par un monstre selon la valeur du dés
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

