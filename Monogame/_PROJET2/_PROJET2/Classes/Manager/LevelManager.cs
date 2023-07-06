using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;


namespace BricksGame
{
    /// <summary>
    /// Le levelManager contient la machine à état des niveaux et crée le niveau en s'appuyant sur le levelLoader (lecture JSON), une baseGrid (grille) et le dicesManager(création de dés)
    /// </summary>
    public class LevelManager
    {

        //Composants
        private LevelLoader levelLoader;
        private DicesManager dicesManager;

        // Etats du niveau
        public int CurrentLevel { get; private set; }
        public enum LevelState { dices, play, gameOver, win, end };
        public LevelState CurrentState { get; set; }

        // Grille de jeu et map
        public BaseGrid GameGrid { get; private set; }
        private PlayerArea playingArea;
        private Texture2D map;

        // Dés composants le niveau
        private List<int> currentLevelDices;

        public LevelManager()
        {
            // Chargement du niveau 1 à la création ; initialisation du levelLoader (lit le JSON)
            CurrentState = LevelState.dices;
            CurrentLevel = 1;
            InitLevelLoader();
        }

        public void Update(GameTime gameTime)
        {
            dicesManager.Update(gameTime);
            CheckIfTheLevelIsWin(GameGrid);
            DebugEnable();    
        }

        // Dessin de la map et de la grille
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(map, Vector2.Zero, Color.White);
            GameGrid.DrawGrid(spriteBatch);
        }

        // Chargement du levelLoader : c'est lui qui lit le JSON des levels
        public void InitLevelLoader()
        {
            levelLoader = new LevelLoader(this);
        }

        // Création du level en fonction du int qui lui est passé : création arie de jeu, création niveau si niveau disponible
        public void LoadLevel(int level)
        {
            CreatePlayingArea();
            CurrentLevel = level;
            CreateLevelIfIsPlayable(level);
        }

        // Permet de mettre fin au level en cours et de passer au level suivant
        public void NextLevel()
        {
            EndLevel();
            LoadLevel(CurrentLevel + 1);
        }

        // Fin de level : on nettoie la grille pour être sûr qu'il n'y a rien qui traine
        public void EndLevel()
        {
            GameGrid.Clear();
        }

        // Méthode appelée par le GameManager quand il n'y a plus de balle en jeu : la grille de monstre descend
        public void OnNoBallInGame()
        {
            GameGrid.Down();
        }

        private void CreatePlayingArea()
        {
            playingArea = new PlayerArea(63, 122, 483, 560);
            ServiceLocator.RegisterService(playingArea);
        }

        // Méthode vérifiant que le level est jouable et disponible avant de le créer. Mets fin au jeu s'il n'y a plus de level disponible
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

        // Création du niveau en tant que tel : création de la grille, créations des dés depuis le JSON, chargement de la map en fond
        private void CreateLevel(int level)
        {
            CurrentState = LevelState.dices;
            CreateBaseGrid(9, 12);
            InitDicesManager();
            CreateDicesFromData(level, GameGrid);
            LoadMap();
        }

        // Création de la grille
        private void CreateBaseGrid(int lin, int col)
        {
            GameGrid = new BaseGrid(lin, col);
        }

        // Init du diceManager qui gère les dés à afficher sur la grille
        public void InitDicesManager()
        {
            dicesManager = new DicesManager(this, GameGrid);
        }

        // Création des dés à partir du JSON
        private void CreateDicesFromData(int level, BaseGrid grid)
        {
            currentLevelDices = levelLoader.ReadDicesData(level);
            dicesManager.CreateDices(grid, currentLevelDices);  
        }

        // Méthode vérifiant si le niveau est gagné en regardant combien de monstre il y a encore en jeu
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

        // Méthode vérifiant le nombre de monstres encore en jeu
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

        // Méthode passant le jeu en mode play quand tous les dés ont été lancés
        public void OnAllDicesRolled()
        {
            CurrentState = LevelState.play;
        }

        // Chargement de la map
        private void LoadMap()
        {
                map = ServiceLocator.GetService<ContentManager>().Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot() + "map1");
        }

        // Pour le debug : faire descendre la grille sur une touche
        private void DebugEnable()
        {
            if (ServiceLocator.GetService<IInputService>().IsKeyReleased(Keys.M))
            {
                GameGrid.Down();
            }
        }

    }


}

