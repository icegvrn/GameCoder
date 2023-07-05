
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace BricksGame
{
    /// <summary>
    /// La scène editor permet l'édition d'un nouveau niveau de jeu. Affiche une grille et offre la possibilité de venir y placer des dés, puis de sauvegarder.
    /// </summary>
    public class SceneEditor : Scene
    {

        private EditorUI editorUI;
        public Gamesystem.dice currentDice;

        // Création de la grille de base
        public BaseGrid GameGrid { get; private set; }
        private int gridCol = 9;
        private int gridLines = 12;

        // Pour la création des dés sur la grille
        private DicesFactory dicesFactory;

        // Chargement des levels déjà sauvegardés
        private string savedLevelsPath;
        private LevelList listOfLevels;

        //Nombre du dé actuellement choisi par l'utilisateur
        public int currentNb { get; set; }

        // List contenant temporairement les données générées par l'utilisateur
        private List<int> tempDicesGrid;

   
    
        public SceneEditor(MainGame p_mainGame) : base(p_mainGame) 
        {
            savedLevelsPath = ServiceLocator.GetService<IPathsService>().GetJSONSavedLevelPath();
        }

        public override void Load()
        {
            CreateSavedLevelFile();
            LoadBackgroundImage();
            editorUI = new EditorUI(this);
            CreateBaseGrid(gridCol, gridLines);
            LoadDices();
        }

        public override void Update(GameTime gameTime)
        {
            GetSlotRectFromMouse();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mainGame._spriteBatch.Draw(background, Vector2.Zero, Color.LightGray);
            mainGame._spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.MainFont), "E-D-I-T-O-R M-O-D-E", new Vector2(mainGame.Window.ClientBounds.Width / 2 - 110, 1), Color.White);
            GameGrid.DrawSlots(mainGame._spriteBatch, false);
            base.Draw(gameTime);
        }


        // Crée la grille de base avec le nombre de lignes et de colonnes spécifié
        private void CreateBaseGrid(int lin, int col)
        {
            CreatePlayingArea();
            GameGrid = new BaseGrid(lin, col);
        }

        // Création d'un playingArea pour positionner la grille
        private void CreatePlayingArea()
        {
            PlayerArea playingArea = new PlayerArea(63, 122, 483, 560);
            ServiceLocator.RegisterService(playingArea);
        }


        // Charge les dés à partir des données déjà sauvegardés
        private void LoadDices()
        { 
            InitDicesFactory();
            ReadLevelsData();
            CreateDicesFromData(1, GameGrid);
        }

        // Initialise la Factory de dés
        private void InitDicesFactory()
        {
            dicesFactory = new DicesFactory();
        }

        // Lit toutes les données des niveaux sauvegardés
        private void ReadLevelsData()
        {
            string json = File.ReadAllText(savedLevelsPath);
            listOfLevels = JsonSerializer.Deserialize<LevelList>(json);
        }

        // Créé les dés d'un niveau donné en passant le niveau en paramètre
        private void CreateDicesFromData(int level, BaseGrid grid)
        {
            CreateDices(grid, ReadDicesData(level));
        }

        // Retourne un tableau de dés à partir d'un niveau spécifiés
        private List<int> ReadDicesData(int level)
        {
            Level levelData = listOfLevels.Levels[level - 1];
            tempDicesGrid = new List<int>();

            foreach (int[] lines in levelData.Dices)
            {
                foreach (int dice in lines)
                {
                    tempDicesGrid.Add(dice);
                }
            }
            return tempDicesGrid;
        }


        // Crée de nouveaux dés à partir d'une liste de valeurs fournies
        private void CreateDices(BaseGrid grid, List<int> dices)
        {
            List<Dice> dicesList = dicesFactory.Load(dices, false);
            for (int n = 0; n < dicesList.Count; n++)
            {
                grid.AddBrickable(dicesList[n], n);
                ServiceLocator.GetService<GameState>().CurrentScene.AddToGameObjectsList(dicesList[n]);
            }
        }

        // Converti la position de la souris en slot de grille et update le dé au clic
        private void GetSlotRectFromMouse()
        {
            Vector2 mouse = ServiceLocator.GetService<IInputService>().GetMousePosition();

            foreach (Rectangle rectangle in GameGrid.GetSlotsByRectangles())
            {
                if (rectangle.Contains(mouse.X, mouse.Y) && rectangle.Y <= GameGrid.maxDestination)
                {
                    int slotIndex = GameGrid.GetSlotIndexFromPosition(new Vector2(rectangle.X, rectangle.Y));

                    if (ServiceLocator.GetService<IInputService>().OnActionDown())
                    {
                        UpdateDiceNumber(slotIndex, currentNb);
                    }
                    else if (ServiceLocator.GetService<IInputService>().OnSecondaryActionDown())
                    {
                        UpdateDiceNumber(slotIndex, 0);
                    }
                }
            }
        }

        // Met à jour le dés d'une case donnée par numéro d'index slot.
        private void UpdateDiceNumber(int index, int nb)
        {
            tempDicesGrid[index] = nb;
            GameGrid.Clear();
            CreateDices(GameGrid, tempDicesGrid);
        }

        // Crée un fichier vierge de niveau sauvegardé si le fichier est inexistant
        private void CreateSavedLevelFile()
        {
            if (!File.Exists(savedLevelsPath))
            {
                LevelList levelList = new LevelList();
                levelList.Levels[0].Dices = new int[gridLines][];

                for (int i = 0; i < gridLines; i++)
                {
                    levelList.Levels[0].Dices[i] = new int[gridCol];
                }

                string json = JsonSerializer.Serialize(levelList);
                File.WriteAllText(savedLevelsPath, json);
            }
        }


        // Méthode de sauvegarde de ce qu'à fait l'utilisateur en convertissant le tempsDicesGrid en format compatible JSON. 
        public void Save(Button p_Button)
        {
            // Conversion du tempDicesGrid en un tableau à deux entrées
            List<int[]> array = new List<int[]>();
            int col = 0;
            bool empty = true;
            
            for (int i=0; i < tempDicesGrid.Count; i++)
            {
                // Si l'entrée est un multiple du nombre de colones de la grille, ça veut dire qu'on est arrivé au bout d'une ligne et on en crée une nouvelle
                if (i % GameGrid.WidthInColumns == 0)
                {  
                    int[] colArray = new int[GameGrid.WidthInColumns];
                    array.Add(colArray);
                    col = 0;
                }

                // Ajout l'entrée dans le JSON
                array[array.Count - 1][col] = tempDicesGrid[i];
                col++;

                if (tempDicesGrid[i] != 0) {
                    empty = false;
                }
            }

            // Permet de s'assurer de ne pas afficher une grille vide quand on load une grille sauvegardée, même si on l'a sauvegardée vide en ajoutant un d3 sur la première case.
            if (empty)
            {
                int[] tempArray = new int[GameGrid.WidthInColumns];
                tempArray[0] = 3;
                array.RemoveAt(0);
                array.Insert(0, tempArray);
            }

            int[][] level = array.ToArray();
       
            listOfLevels.Levels[0].Dices = level;   
            string json = JsonSerializer.Serialize(listOfLevels);
            File.WriteAllText(savedLevelsPath, json);
            mainGame.gameState.ChangeScene(GameState.SceneType.Menu);
        }


        // Principalement utilisé en debug : Permet de dessiner une grille avec le numéro de chaque dés issus du JSON. 
        private void DrawDicesNumber()
        {
            Level levelData = listOfLevels.Levels[0];
            List<int> levelDices = new List<int>();

            int nb = 0;
            foreach (int[] lines in levelData.Dices)
            {
                foreach (int dice in lines)
                {
                    mainGame._spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.MainFont), dice.ToString(), GameGrid.GetPositionFromGrid(nb), Color.White);
                    nb++;
                }
            }
        }

        private void LoadBackgroundImage()
        {
            ContentManager content = ServiceLocator.GetService<ContentManager>();
            background = content.Load<Texture2D>(ServiceLocator.GetService<IPathsService>().GetImagesRoot() + "map1");
        }

        public override void UnLoad()
        {
            base.UnLoad();
        }

    }


}
